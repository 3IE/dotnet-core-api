using Xunit;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using TodoApi.Models;
using TodoApi.BusinessManagement;
using TodoApi.DataAccess;

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
    }
}
