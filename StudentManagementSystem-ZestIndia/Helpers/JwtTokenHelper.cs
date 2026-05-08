using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace StudentManagementSystem_ZestIndia.Helpers
{
    /// <summary>
    /// Helper class for JWT token generation and validation
    /// </summary>
    public interface IJwtTokenHelper
    {
        string GenerateToken(string username);
        DateTime GetTokenExpirationTime();
    }

    public class JwtTokenHelper : IJwtTokenHelper
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtTokenHelper> _logger;

        public JwtTokenHelper(IConfiguration configuration, ILogger<JwtTokenHelper> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Generates a JWT token for the given username
        /// </summary>
        /// <param name="username">Username for which to generate the token</param>
        /// <returns>JWT token string</returns>
        public string GenerateToken(string username)
        {
            try
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secret = jwtSettings["Secret"];
                var issuer = jwtSettings["Issuer"];
                var audience = jwtSettings["Audience"];
                var expirationInMinutes = int.Parse(jwtSettings["ExpirationInMinutes"] ?? "60");

                if (string.IsNullOrEmpty(secret) || secret.Length < 32)
                {
                    _logger.LogError("JWT Secret is not configured properly. Ensure it's at least 32 characters long.");
                    throw new InvalidOperationException("JWT Secret is not configured properly.");
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.NameIdentifier, username)
                };

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expirationInMinutes),
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                _logger.LogInformation("JWT token generated successfully for user: {Username}", username);

                return tokenString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token for user: {Username}", username);
                throw;
            }
        }

        /// <summary>
        /// Gets the token expiration time based on configuration
        /// </summary>
        /// <returns>DateTime when the token expires</returns>
        public DateTime GetTokenExpirationTime()
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var expirationInMinutes = int.Parse(jwtSettings["ExpirationInMinutes"] ?? "60");
            return DateTime.UtcNow.AddMinutes(expirationInMinutes);
        }
    }
}
