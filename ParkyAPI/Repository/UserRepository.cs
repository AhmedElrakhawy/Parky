using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ParkyAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _Db;
        private readonly AppSettings _appsetting;
        public UserRepository(ApplicationDbContext Db, IOptions<AppSettings> appsettings)
        {
            _Db = Db;
            _appsetting = appsettings.Value;
        }
        public User Authenticate(string username, string Passward)
        {
            var user = _Db.Users.SingleOrDefault(x => x.Username == username && x.Password == Passward);
            if (user == null)
            {
                return null;
            }

            var TokenHandler = new JwtSecurityTokenHandler();
            var Key = Encoding.ASCII.GetBytes(_appsetting.Secret);
            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key)
                , SecurityAlgorithms.HmacSha256Signature)
            };
            var Token = TokenHandler.CreateToken(TokenDescriptor);
            user.Token = TokenHandler.WriteToken(Token);
            return user;
        }

        public bool IsUniqueUser(string username)
        {
            var User = _Db.Users.SingleOrDefault(x => x.Username == username);
            if (User == null)
                return true;

            return false;
        }

        public User Register(string username, string Password)
        {
            var User = new User
            {
                Username = username,
                Password = Password,
                Role = "Admin"
            };
            _Db.Users.Add(User);
            _Db.SaveChanges();
            User.Password = "";
            return User;
        }
    }
}
