namespace StudentManagementSystem_ZestIndia.DTOs
{
    /// <summary>
    /// DTO for login response with JWT token
    /// </summary>
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public string Username { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}
