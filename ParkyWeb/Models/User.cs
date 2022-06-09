using System.ComponentModel.DataAnnotations;

namespace ParkyWeb.Models
{
    public class User
    {
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
