using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/Users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _UserRepository;
        public UsersController(IUserRepository userRepository)
        {
            _UserRepository = userRepository;
        }
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] Authentication Model)
        {
            var user = _UserRepository.Authenticate(Model.Username, Model.Password);
            if (user == null)
            {
                return BadRequest(new { Message = "UserName or Passward is incorrect" });
            }
            return Ok(user);
        }
        [HttpPost("Register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] Authentication user)
        {
            var IfUserIsUnique = _UserRepository.IsUniqueUser(user.Username);
            if (!IfUserIsUnique)
            {
                return BadRequest(new { Message = "UserName is Excist" });
            }
            var NewUser = _UserRepository.Register(user.Username, user.Password);
            if (NewUser == null)
            {
                return BadRequest(new { Message = "Something went wrong with registaring" });
            }
            return Ok();
        }
    }
}
