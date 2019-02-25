using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
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

        public async Task<DataContext> InitTodoTest()
        {
            var context = GetContext();
            var access = new RepositoryUser(context);
            var bm = new UserServices(access);
            await bm.Create(new Users { Username = "user1" }, "test");
            return context;
        }

        [Fact(DisplayName = "Add a new TodoItem")]
        public async void Add_todoItem()
        {
            var context = await InitTodoTest();
            var access = new RepositoryTodoItem(context);
            var bm = new TodoServices(access);
            try
            {
                await bm.PostTodoItem(new TodoItems { Name = "Walk dog", IsComplete = false, UserId = 1 });
                var item = await bm.GetTodoItem(1);
                Assert.NotNull(item);
            }
            finally
            {
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        [Fact(DisplayName = "Add 2 new TodoItem")]
        public async void Add_two_todoItem()
        {
            var context = await InitTodoTest();
            var access = new RepositoryTodoItem(context);
            var bm = new TodoServices(access);
            try
            {
                await bm.PostTodoItem(new TodoItems { Name = "Walk dog", IsComplete = false, UserId = 1 });
                await bm.PostTodoItem(new TodoItems { Name = "Do groceries", IsComplete = false, UserId = 1 });
                var item = await bm.GetTodoItem(1);
                Assert.NotNull(item);
            }
            finally
            {
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        [Fact(DisplayName = "Get all TodoItems")]
        public async void Get_all_todoItems()
        {
            var context = await InitTodoTest();
            var access = new RepositoryTodoItem(context);
            var bm = new TodoServices(access);
            try
            {
                await bm.PostTodoItem(new TodoItems { Name = "walk dog", IsComplete = false, UserId = 1 });
                await bm.PostTodoItem(new TodoItems { Name = "Do groceries", IsComplete = false, UserId = 1 });
                var items = await bm.GetTodoItems();
                ICollection<TodoItems> itemList = items as ICollection<TodoItems>;
                Assert.Equal(2, itemList.Count);
            }
            finally
            {
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }
    }
}
