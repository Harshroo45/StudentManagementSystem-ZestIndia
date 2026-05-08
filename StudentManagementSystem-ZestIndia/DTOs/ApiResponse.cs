namespace StudentManagementSystem_ZestIndia.DTOs
{
    /// <summary>
    /// DTO for API response wrapper
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public T? Data { get; set; }
        public int StatusCode { get; set; }
    }

    /// <summary>
    /// Non-generic API response wrapper
    /// </summary>
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public int StatusCode { get; set; }
    }
}
