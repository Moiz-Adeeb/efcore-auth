using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; 

namespace efcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService service;
        public AuthController(IAuthService service)
        {
            this.service = service;   
        }

        [HttpPost("register-user")]
        public async Task<ActionResult<UserOutputDto?>> RegisterUser(UserInputDto request)
        {
            var user = await service.RegisterUserAsync(request);
            if(user == null) {
                return BadRequest("User already exists.");
            }
            return Ok(user);
        }

        [HttpPost("register-manager")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserOutputDto?>> RegisterManager(UserInputDto request)
        {
            var user = await service.RegisterManagerAsync(request);
            if(user == null) {
                return BadRequest("User already exists.");
            }   
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserInputDto request)
        {
            var token = await service.LoginAsync(request);
            if (token is null)
            {
                return BadRequest("Username or Password is wrong");
            }
            return Ok(token);
        }

        [HttpGet("User-endpoint")]
        [Authorize(Roles = "User")]
        public ActionResult UserCheck()
        {
            return Ok("Login Success");
        }

        [HttpGet("Manager-endpoint")]
        [Authorize(Roles = "Manager")]
        public ActionResult ManagerCheck()
        {
            return Ok("Login Success");
        }

        [HttpGet("Admin-endpoint")]
        [Authorize(Roles = "Admin")]
        public ActionResult AdminCheck()
        {
            return Ok("Login Success");
        }
    }
}
