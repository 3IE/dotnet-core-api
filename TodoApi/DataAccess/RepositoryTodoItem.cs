using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.DataAccess
{
    public interface IRepositoryTodoItem : IRepositoryBase<TodoItems>
    {
    }

    public class RepositoryTodoItem : RepositoryBase<TodoItems>, IRepositoryTodoItem
    {
        public RepositoryTodoItem(DataContext context) : base(context)
        {
        }
    }
}
