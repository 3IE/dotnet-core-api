using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.DataAccess
{
    public interface IRepositoryUser<Users>
    {
        Task<IEnumerable<Users>> GetAll();
        Task<Users> GetElem(int id);
        Task AddElem(Users elem);
        Task UpdateElem(int id, Users elem);
        Task DeleteElem(Users elem);
    }

    public class RepositoryUser : RepositoryBase<Users>
    {
        private DataContext _context;

        public RepositoryUser(DataContext context)
        {
            _context = context;
        }
    }
}
