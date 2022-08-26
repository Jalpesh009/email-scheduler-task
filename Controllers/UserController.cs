using EmailScheduler.Model;
using EmailScheduler.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EmailScheduler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userService.GetAll();
        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _userService.Insert(user);
            return Ok(user);
        }
    }
}