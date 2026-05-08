namespace StudentManagementSystem_ZestIndia.DTOs
{
    /// <summary>
    /// DTO for Student response
    /// </summary>
    public class StudentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Age { get; set; }
        public string Course { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
