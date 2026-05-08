namespace StudentManagementSystem_ZestIndia.DTOs
{
    /// <summary>
    /// DTO for creating a new student
    /// </summary>
    public class CreateStudentDTO
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Age { get; set; }
        public string Course { get; set; } = null!;
    }
}
