using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.DataAccess
{
    public interface IRepositoryUser : IRepositoryBase<Users>
    {
        Task<Users> GetAnyUsername(string username);
        Task<IEnumerable<TodoItems>> GetUserTodos(string name);
    }

    public class RepositoryUser :RepositoryBase<Users>, IRepositoryUser 
    {
        public RepositoryUser(DataContext context) : base(context)
        {
        }

        public async Task<Users> GetAnyUsername(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Username == username);
        }

        public async Task<IEnumerable<TodoItems>> GetUserTodos(string name)
        {
            var selectTodos = from user in _context.Users
                              join item in _context.TodoItems on user.Id equals item.UserId
                              where user.Username == name
                              select item;

            return await selectTodos.ToListAsync();
        }
    }
}
