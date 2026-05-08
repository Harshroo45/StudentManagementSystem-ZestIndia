using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem_ZestIndia.DTOs;
using StudentManagementSystem_ZestIndia.Helpers;

namespace StudentManagementSystem_ZestIndia.Controllers
{
    /// <summary>
    /// Controller for authentication operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenHelper _jwtTokenHelper;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IJwtTokenHelper jwtTokenHelper, ILogger<AuthController> logger)
        {
            _jwtTokenHelper = jwtTokenHelper;
            _logger = logger;
        }

        /// <summary>
        /// Login endpoint - generates JWT token
        /// </summary>
        /// <param name="loginRequest">Login credentials (username/password)</param>
        /// <returns>JWT token for authorized API access</returns>
        /// <remarks>
        /// Demo credentials:
        /// - Username: admin
        /// - Password: admin123
        /// 
        /// Use the returned token in Authorization header: Bearer {token}
        /// </remarks>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                _logger.LogInformation("Login attempt for user: {Username}", loginRequest.Username);

                if (string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.Password))
                {
                    _logger.LogWarning("Login attempt with empty credentials");
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Username and password are required",
                        StatusCode = 400
                    });
                }

                // Demo authentication - In production, validate against database
                if (!ValidateCredentials(loginRequest.Username, loginRequest.Password))
                {
                    _logger.LogWarning("Invalid login credentials for user: {Username}", loginRequest.Username);
                    return Unauthorized(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid username or password",
                        StatusCode = 401
                    });
                }

                var token = _jwtTokenHelper.GenerateToken(loginRequest.Username);
                var expiresAt = _jwtTokenHelper.GetTokenExpirationTime();

                _logger.LogInformation("User logged in successfully: {Username}", loginRequest.Username);

                return Ok(new ApiResponse<LoginResponse>
                {
                    Success = true,
                    Message = "Login successful",
                    Data = new LoginResponse
                    {
                        Token = token,
                        Username = loginRequest.Username,
                        ExpiresAt = expiresAt
                    },
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user: {Username}", loginRequest.Username);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred during login",
                    StatusCode = 500
                });
            }
        }

        /// <summary>
        /// Validates user credentials (demo implementation)
        /// In production, this should validate against a database
        /// </summary>
        private bool ValidateCredentials(string username, string password)
        {
            // Demo credentials - In production, query database and use password hashing
            return username.Equals("admin", StringComparison.OrdinalIgnoreCase) && password == "admin123";
        }
    }
}
