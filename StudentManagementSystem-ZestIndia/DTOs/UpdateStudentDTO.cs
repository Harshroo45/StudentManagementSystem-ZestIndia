namespace StudentManagementSystem_ZestIndia.DTOs
{
    /// <summary>
    /// DTO for updating a student
    /// </summary>
    public class UpdateStudentDTO
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Age { get; set; }
        public string Course { get; set; } = null!;
    }
}
