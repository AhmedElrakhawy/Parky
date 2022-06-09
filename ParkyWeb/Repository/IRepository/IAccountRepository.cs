using ParkyWeb.Models;
using System.Threading.Tasks;

namespace ParkyWeb.Repository.IRepository
{
    public interface IAccountRepository : IRepository<User>
    {
        Task<User> LoginAsync(string Url, User user);
        Task<bool> RegisterAsync(string Url, User userToCreate);
    }
}
