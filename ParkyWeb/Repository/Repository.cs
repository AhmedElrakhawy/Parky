using Newtonsoft.Json;
using ParkyWeb.Repository.IRepository;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ParkyWeb.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _ClientFactory;
        public Repository(IHttpClientFactory ClientFactory)
        {
            _ClientFactory = ClientFactory;
        }
        public async Task<bool> CreateAsync(string Url, T ObjToCreate, string token = "")
        {
            var Request = new HttpRequestMessage(HttpMethod.Post, Url);
            if (ObjToCreate != null)
            {
                Request.Content = new StringContent(
                    JsonConvert.SerializeObject(ObjToCreate), Encoding.UTF8, "application/json");
            }
            var Client = _ClientFactory.CreateClient();
            if (token != null && token.Length > 0)
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage Response = await Client.SendAsync(Request);
            if (Response.StatusCode == HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string Url, int Id, string token = "")
        {
            var Request = new HttpRequestMessage(HttpMethod.Delete, Url + Id);
            var Client = _ClientFactory.CreateClient();
            if (token != null && token.Length > 0)
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var Response = await Client.SendAsync(Request);
            if (Response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(string Url, string token = "")
        {
            var Request = new HttpRequestMessage(HttpMethod.Get, Url);
            var Client = _ClientFactory.CreateClient();
            if (token != null && token.Length > 0)
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var Response = await Client.SendAsync(Request);
            if (Response.StatusCode == HttpStatusCode.OK)
            {
                var JsonString = await Response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(JsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<T> GetAsync(string Url, int Id, string token = "")
        {
            var Request = new HttpRequestMessage(HttpMethod.Get, Url + Id);
            var Client = _ClientFactory.CreateClient();
            if (token != null && token.Length > 0)
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var Response = await Client.SendAsync(Request);
            if (Response.StatusCode == HttpStatusCode.OK)
            {
                var JsonString = await Response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(JsonString);
            }
            else
            {
                return null;
            }

        }

        public async Task<bool> UpdateAsync(string Url, T ObjToUpdate, string token = "")
        {
            var Request = new HttpRequestMessage(HttpMethod.Patch, Url);
            if (ObjToUpdate != null)
            {
                Request.Content = new StringContent(
                    JsonConvert.SerializeObject(ObjToUpdate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var Client = _ClientFactory.CreateClient();
            if (token != null && token.Length > 0)
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var Response = await Client.SendAsync(Request);
            if (Response.StatusCode == HttpStatusCode.NoContent)
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
