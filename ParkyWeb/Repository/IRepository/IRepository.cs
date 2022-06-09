using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkyWeb.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(string Url, int Id, string token);
        Task<IEnumerable<T>> GetAllAsync(string Url, string token);
        Task<bool> CreateAsync(string Url, T ObjToCreate, string token);
        Task<bool> UpdateAsync(string Url, T ObjToUpdate, string token);
        Task<bool> DeleteAsync(string Url, int Id, string token);
    }
}
