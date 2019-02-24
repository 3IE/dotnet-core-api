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
    public class UnitTest1
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
                Assert.True(user.Username == "user1");
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
    }
}
