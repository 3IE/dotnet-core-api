using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using TodoApi.Models;
using TodoApi.DataAccess;

namespace TodoApiTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var context = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;
        }
    }
}
