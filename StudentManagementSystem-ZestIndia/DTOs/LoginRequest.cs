namespace StudentManagementSystem_ZestIndia.DTOs
{
    /// <summary>
    /// DTO for login request
    /// </summary>
    public class LoginRequest
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
