using EmployeeTimeTracking_API.DTOs;
using EmployeeTimeTracking_API.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTimeTracking_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequestDto request)
        {
            // The [ApiController] attribute automatically handles model validation.
            // If [Required] is missing, it will return a 400 Bad Request
            // before this code even runs.

            var response = await _authService.RegisterUserAsync(request);

            if (response.Success)
            {
                // Return 200 OK with the response data (user ID, email, etc.)
                return Ok(response);
            }

            // Return 400 Bad Request with the error message ("Email already exists.")
            return BadRequest(response);
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequestDto request)
        {
            var response = await _authService.LoginUserAsync(request);

            if (response.Success)
            {
                // Return 200 OK with the response data (token, email, role)
                return Ok(response);
            }

            // Return 401 Unauthorized with the error message ("Invalid email or password.")
            return Unauthorized(response);
        }
    }
}
