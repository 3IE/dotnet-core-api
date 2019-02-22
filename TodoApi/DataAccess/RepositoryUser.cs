using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.DataAccess
{
    public interface IRepositoryUser : IRepositoryBase<Users>
    {
    }

    public class RepositoryUser :RepositoryBase<Users>, IRepositoryUser 
    {
        public RepositoryUser(DataContext context) : base(context)
        {
        }
    }
}
