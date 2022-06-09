using Newtonsoft.Json;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParkyWeb.Repository
{
    public class AccountRepository : Repository<User>, IAccountRepository
    {
        private readonly IHttpClientFactory _ClientFactory;
        public AccountRepository(IHttpClientFactory ClientFactory) : base(ClientFactory)
        {
            _ClientFactory = ClientFactory;
        }

        public async Task<User> LoginAsync(string Url, User user)
        {
            var Request = new HttpRequestMessage(HttpMethod.Post, Url);
            if (user != null)
            {
                Request.Content = new StringContent(
                    JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            }
            else
            {
                return new User();
            }
            var Client = _ClientFactory.CreateClient();
            var Response = await Client.SendAsync(Request);
            if (Response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await Response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(jsonString);
            }
            else
            {
                return new User();
            }
        }

        public async Task<bool> RegisterAsync(string Url, User userToCreate)
        {
            var Request = new HttpRequestMessage(HttpMethod.Post, Url);
            if (userToCreate != null)
            {
                Request.Content = new StringContent(
                    JsonConvert.SerializeObject(userToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var Client = _ClientFactory.CreateClient();
            var Response = await Client.SendAsync(Request);
            if (Response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
