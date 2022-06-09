using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System.Net.Http;

namespace ParkyWeb.Repository
{
    public class TrailsRepository : Repository<Trail>, ITrailsRepository
    {
        private readonly IHttpClientFactory _ClientFactory;
        public TrailsRepository(IHttpClientFactory ClientFactory) : base(ClientFactory)
        {
            _ClientFactory = ClientFactory;
        }
    }
}
