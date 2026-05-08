using StudentManagementSystem_ZestIndia.DTOs;

namespace StudentManagementSystem_ZestIndia.Services.Interfaces
{
    /// <summary>
    /// Interface for Student business logic operations
    /// </summary>
    public interface IStudentService
    {
        Task<ApiResponse<IEnumerable<StudentDTO>>> GetAllStudentsAsync();
        Task<ApiResponse<StudentDTO>> GetStudentByIdAsync(int id);
        Task<ApiResponse<StudentDTO>> CreateStudentAsync(CreateStudentDTO createStudentDto);
        Task<ApiResponse<StudentDTO>> UpdateStudentAsync(int id, UpdateStudentDTO updateStudentDto);
        Task<ApiResponse> DeleteStudentAsync(int id);
    }
}
