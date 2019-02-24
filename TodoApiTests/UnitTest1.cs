using Xunit;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using TodoApi.Models;
using TodoApi.BusinessManagement;
using TodoApi.DataAccess;
using System.Collections.Generic;
using System.Linq;

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
                Assert.True(userList.Count == 1);
                Assert.True(userList.FirstOrDefault().Username == "user1");
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
