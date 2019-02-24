using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;
using TodoApi.DataAccess;

namespace TodoApi.BusinessManagement
{
    public interface ITodoServices
    {
        Task<IEnumerable<TodoItems>> GetTodoItems();
        Task<TodoItems> GetTodoItem(int id);
        Task PostTodoItem(TodoItems item);
        Task PutTodoItem(int id, TodoItems item);
        Task DeleteTodoItem(TodoItems item);
    }

    public class TodoServices : ITodoServices
    {
        private IRepositoryTodoItem _repository;

        public TodoServices(IRepositoryTodoItem repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TodoItems>> GetTodoItems()
        {
            return await _repository.GetAll();
        }

        public async Task<TodoItems> GetTodoItem(int id)
        {
            return await _repository.GetElem(id);
        }

        public async Task PostTodoItem(TodoItems item)
        {
            await _repository.AddElem(item);
        }

        public async Task PutTodoItem(int id, TodoItems item)
        {
            await _repository.UpdateElem(id, item);
        }

        public async Task DeleteTodoItem(TodoItems item)
        {
            await _repository.DeleteElem(item);
        }
    }
}
