using Xunit;
using System;
using System.Linq;
using System.Collections.Generic;
//using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.DataAccess;
using TodoApi.BusinessManagement;


namespace TodoApiTests
{
    public class TestUsers
    {
        private DataContext GetContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;
            var context = new DataContext(options);
            return context;
        }

        [Fact(DisplayName = "Insert new user in empty db")]
        public async void Create_new_user()
        {
            var context = GetContext();
            var access = new RepositoryUser(context);
            var bm = new UserServices(access);
            try
            {
                var user = await bm.Create(new Users { Username = "user1" }, "test");
                Assert.NotNull(user);
            }
            finally
            {
                context.Dispose();
            }
        }

        [Fact(DisplayName = "Insert new user with no password")]
        public async void Create_new_user_no_pasword()
        {
            var context = GetContext();
            var access = new RepositoryUser(context);
            var bm = new UserServices(access);
            try
            {
                var user = await bm.Create(new Users { Username = "user1" }, "");
                Assert.True(false);
            }
            catch
            {
                Assert.True(true);
                context.Dispose();
            }
            finally
            {
                context.Dispose();
            }
        }

        [Fact(DisplayName = "Insert 2 users in empty db")]
        public async void Create_new_users()
        {
            var context = GetContext();
            var access = new RepositoryUser(context);
            var bm = new UserServices(access);
            try
            {
                var user1 = await bm.Create(new Users { Username = "user1" }, "test");
                var user2 = await bm.Create(new Users { Username = "user2" }, "test");
                Assert.NotNull(user1);
                Assert.NotNull(user2);
            }
            finally
            {
                context.Dispose();
            }
        }

        [Fact(DisplayName = "Insert 2 users with same username")]
        public async void Create_new_users_with_same_usernames()
        {
            var context = GetContext();
            var access = new RepositoryUser(context);
            var bm = new UserServices(access);
            try
            {
                var user1 = await bm.Create(new Users { Username = "user1" }, "test");
                var user2 = await bm.Create(new Users { Username = "user1" }, "test");
                Assert.True(false);
            }
            catch
            {
                Assert.True(true);
                context.Dispose();
            }
            finally
            {
                context.Dispose();
            }
        }

        [Fact(DisplayName = "Create new user and read db")]
        public async void Create_new_user_and_read_db()
        {
            var context = GetContext();
            var access = new RepositoryUser(context);
            var bm = new UserServices(access);
            try
            {
                await bm.Create(new Users { Username = "user1" }, "test");
                var users = await bm.GetAllUsers();
                ICollection<Users> userList = users as ICollection<Users>;
                Assert.Equal(1, userList.Count);
                Assert.Equal("user1", userList.First().Username);
            }
            finally
            {
                context.Dispose();
            }
        }

        [Fact(DisplayName = "Update user correctly")]
        public async void Update_user_correctly()
        {
            var context = GetContext();
            var access = new RepositoryUser(context);
            var bm = new UserServices(access);
            try
            {
                await bm.Create(new Users { Username = "user1" }, "test");
                await bm.UpdateUser(new Users { Id = 1, Username = "user2" }, "test");
                var updatedUser = await bm.GetById(1);
                Assert.Equal("user2", updatedUser.Username);
            }
            catch
            {
                Assert.True(false);
                context.Dispose();
            }
            finally
            {
                context.Dispose();
            }
        }

        [Fact(DisplayName = "Update user with existing username")]
        public async void Update_user_no_password()
        {
            var context = GetContext();
            var access = new RepositoryUser(context);
            var bm = new UserServices(access);
            try
            {
                await bm.Create(new Users { Username = "user1" }, "test");
                await bm.Create(new Users { Username = "user2" }, "test");
                await bm.UpdateUser(new Users { Username = "user2" }, "");
                Assert.True(false);
            }
            catch
            {
                Assert.True(true);
                context.Dispose();
            }
            finally
            {
                context.Dispose();
            }
        }

        [Fact(DisplayName = "Delete user in db")]
        public async void Delete_user_in_db()
        {
            var context = GetContext();
            var access = new RepositoryUser(context);
            var bm = new UserServices(access);
            try
            {
                await bm.Create(new Users { Username = "user1" }, "test");
                await bm.Create(new Users { Username = "user2" }, "test");
                await bm.DeleteUser(1);
                var users = await bm.GetAllUsers();
                ICollection<Users> userList = users as ICollection<Users>;
                Assert.Equal(1, userList.Count);
                Assert.Equal("user2", userList.First().Username);
            }
            finally
            {
                context.Dispose();
            }
        }

        [Fact(DisplayName = "Delete non-existing user in db")]
        public async void Delete_non_existing_user_in_db()
        {
            var context = GetContext();
            var access = new RepositoryUser(context);
            var bm = new UserServices(access);
            try
            {
                await bm.Create(new Users { Username = "user1" }, "test");
                await bm.DeleteUser(2);
                Assert.True(false);
            }
            catch
            {
                Assert.True(true);
                context.Dispose();
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
