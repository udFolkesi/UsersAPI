using BLL.Services;
using BLL.Services.Abstractions;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace UsersAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        public UsersController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("/token")]
        public async Task<IActionResult> GetToken(string login, string password)
        {
            var result = await _authService.AuthenticateAsync(login, password);

            if (!result.IsSuccess)
                return Unauthorized(new { error = result.Error });

            return Ok(new { token = result.Token });
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateUserDto userDto)
        {
            var creator = User.Identity?.Name!;
            User user = new User()
            {
                Login = userDto.Login,
                Password = userDto.Password,
                Name = userDto.Name,
                Gender = userDto.Gender,
                Birthday = userDto.Birthday,
                Admin = userDto.Admin,
            };
            var result = await _userService.Create(user, creator);
            if (!result.Success)
                return BadRequest(result.Error);
            return Ok(result.Success);
        }

/*        [HttpPut("update-profile/{login}")]
        public async Task<IActionResult> UpdateProfile(string login, [FromBody] UpdateUserDto dto)
        {
            var requester = User.Identity?.Name!;
            var isAdmin = User.IsInRole("Admin");

            var result = await _userService.UpdateUserInfoAsync(login, dto, requester);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.User);
        }*/

        [HttpPut("update-password/{login}")]
        public async Task<IActionResult> UpdatePassword(string login, string newPassword)
        {
            var requester = User.Identity?.Name!;
            var isAdmin = User.IsInRole("Admin");

            var result = await _userService.ChangePasswordAsync(login, newPassword, requester);
/*            if (!result.IsSuccess)
                return BadRequest(result.Error);*/

            return Ok("Password updated successfully.");
        }

        // 4. Обновление логина
        [HttpPut("update-login/{login}")]
        public async Task<IActionResult> UpdateLogin(string login, string newLogin)
        {
            var requester = User.Identity?.Name!;
            var isAdmin = User.IsInRole("Admin");

            var result = await _userService.ChangeLoginAsync(login,newLogin, requester);
/*            if (!result.IsSuccess)
                return BadRequest(result.Error);*/

            return Ok("Login updated successfully.");
        }

        [HttpGet("active")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllActive() => Ok(await _userService.GetActiveUsersAsync());

        [HttpGet("by-login/{login}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByLogin(string login)
        {
            var user = await _userService.GetByLoginAsync(login);
            if (user == null) return NotFound("User not found");

            return Ok(new
            {
                user.Name,
                user.Gender,
                user.Birthday,
                IsActive = user.RevokedOn == null
            });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.GetByCredentialsAsync(request.Login, request.Password);
            if (user == null) return Unauthorized("Invalid credentials");

            var token = _authService.GenerateToken(user);
            return Ok(new { token });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetSelf()
        {
            var login = User.Identity?.Name ?? "";
            var user = await _userService.GetByLoginAsync(login);
            if (user == null) return NotFound();
            return Ok(new { user.Name, user.Gender, user.Birthday, user.Login });
        }

        [HttpGet("older-than/{age}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOlderThan(int age) => Ok(await _userService.GetOlderThanAsync(age));

        [HttpDelete("{login}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string login)
        {
            var admin = User.Identity?.Name ?? "";
            var result = await _userService.Delete(login, admin);
            return result.Success ? Ok() : BadRequest(result.Error);
        }

        [HttpPut("restore/{login}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Restore(string login)
        {
            var result = await _userService.RestoreAsync(login);
            return result.Success ? Ok() : BadRequest(result.Error);
        }
    }

    public class LoginRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
