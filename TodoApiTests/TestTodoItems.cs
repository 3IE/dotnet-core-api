using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
//using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.DataAccess;
using TodoApi.BusinessManagement;

namespace TodoApiTests
{
    public class TestTodoItems
    {
        private DataContext GetContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;
            var context = new DataContext(options);
            return context;
        }

        public async Task<DataContext> Add_test_user()
        {
            var context = GetContext();
            var access = new RepositoryUser(context);
            var bm = new UserServices(access);
            await bm.Create(new Users { Username = "user1" }, "test");
            return context;
        }
    }
}
