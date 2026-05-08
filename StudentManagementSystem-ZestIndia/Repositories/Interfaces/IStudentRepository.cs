using StudentManagementSystem_ZestIndia.Models;

namespace StudentManagementSystem_ZestIndia.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Student repository operations
    /// </summary>
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<Student> CreateStudentAsync(Student student);
        Task<Student?> UpdateStudentAsync(int id, Student student);
        Task<bool> DeleteStudentAsync(int id);
        Task<bool> StudentExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email);
    }
}
