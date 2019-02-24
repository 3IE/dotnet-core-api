using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

// Utiliser les generics
namespace TodoApi.DataAccess
{
    public interface IRepositoryBase<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetElem(int id);
        Task AddElem(T elem);
        Task UpdateElem(int id, T elem);
        Task DeleteElem(T elem);
    }

    public class RepositoryBase<T> where T : class
    {
        protected DataContext _context;

        public RepositoryBase(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetElem(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task AddElem(T elem)
        {
            _context.Set<T>().Add(elem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateElem(int id, T elem)
        {
            _context.Entry(elem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteElem(T elem)
        {
            _context.Set<T>().Remove(elem);
            await _context.SaveChangesAsync();
        }
    }
}
