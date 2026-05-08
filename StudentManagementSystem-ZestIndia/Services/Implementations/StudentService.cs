using StudentManagementSystem_ZestIndia.DTOs;
using StudentManagementSystem_ZestIndia.Models;
using StudentManagementSystem_ZestIndia.Repositories.Interfaces;
using StudentManagementSystem_ZestIndia.Services.Interfaces;

namespace StudentManagementSystem_ZestIndia.Services.Implementations
{
    /// <summary>
    /// Service implementation for Student business logic
    /// </summary>
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IStudentRepository studentRepository, ILogger<StudentService> logger)
        {
            _studentRepository = studentRepository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all students with response wrapper
        /// </summary>
        /// <returns>API response containing list of students</returns>
        public async Task<ApiResponse<IEnumerable<StudentDTO>>> GetAllStudentsAsync()
        {
            try
            {
                _logger.LogInformation("Service: Fetching all students");
                var students = await _studentRepository.GetAllStudentsAsync();
                var studentDtos = students.Select(MapToDTO).ToList();

                if (!studentDtos.Any())
                {
                    _logger.LogInformation("No students found in database");
                    return new ApiResponse<IEnumerable<StudentDTO>>
                    {
                        Success = true,
                        Message = "No students found",
                        Data = studentDtos,
                        StatusCode = 200
                    };
                }

                return new ApiResponse<IEnumerable<StudentDTO>>
                {
                    Success = true,
                    Message = $"Retrieved {studentDtos.Count} student(s) successfully",
                    Data = studentDtos,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllStudentsAsync");
                return new ApiResponse<IEnumerable<StudentDTO>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving students",
                    Data = null,
                    StatusCode = 500
                };
            }
        }

        /// <summary>
        /// Retrieves a specific student by ID
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>API response containing the student</returns>
        public async Task<ApiResponse<StudentDTO>> GetStudentByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Service: Fetching student with ID: {StudentId}", id);
                
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid student ID: {StudentId}", id);
                    return new ApiResponse<StudentDTO>
                    {
                        Success = false,
                        Message = "Invalid student ID",
                        Data = null,
                        StatusCode = 400
                    };
                }

                var student = await _studentRepository.GetStudentByIdAsync(id);

                if (student == null)
                {
                    _logger.LogWarning("Student not found with ID: {StudentId}", id);
                    return new ApiResponse<StudentDTO>
                    {
                        Success = false,
                        Message = $"Student with ID {id} not found",
                        Data = null,
                        StatusCode = 404
                    };
                }

                return new ApiResponse<StudentDTO>
                {
                    Success = true,
                    Message = "Student retrieved successfully",
                    Data = MapToDTO(student),
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetStudentByIdAsync with ID: {StudentId}", id);
                return new ApiResponse<StudentDTO>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the student",
                    Data = null,
                    StatusCode = 500
                };
            }
        }

        /// <summary>
        /// Creates a new student
        /// </summary>
        /// <param name="createStudentDto">Student creation DTO</param>
        /// <returns>API response containing created student</returns>
        public async Task<ApiResponse<StudentDTO>> CreateStudentAsync(CreateStudentDTO createStudentDto)
        {
            try
            {
                _logger.LogInformation("Service: Creating new student with name: {StudentName}", createStudentDto.Name);

                // Validate input
                if (string.IsNullOrWhiteSpace(createStudentDto.Name) ||
                    string.IsNullOrWhiteSpace(createStudentDto.Email) ||
                    string.IsNullOrWhiteSpace(createStudentDto.Course) ||
                    createStudentDto.Age <= 0)
                {
                    _logger.LogWarning("Invalid student data provided");
                    return new ApiResponse<StudentDTO>
                    {
                        Success = false,
                        Message = "Invalid student data. All fields are required and age must be positive.",
                        Data = null,
                        StatusCode = 400
                    };
                }

                // Check if email already exists
                var emailExists = await _studentRepository.EmailExistsAsync(createStudentDto.Email);
                if (emailExists)
                {
                    _logger.LogWarning("Email already exists: {Email}", createStudentDto.Email);
                    return new ApiResponse<StudentDTO>
                    {
                        Success = false,
                        Message = "Email already exists",
                        Data = null,
                        StatusCode = 400
                    };
                }

                var student = new Student
                {
                    Name = createStudentDto.Name,
                    Email = createStudentDto.Email,
                    Age = createStudentDto.Age,
                    Course = createStudentDto.Course,
                    CreatedDate = DateTime.UtcNow
                };

                var createdStudent = await _studentRepository.CreateStudentAsync(student);

                return new ApiResponse<StudentDTO>
                {
                    Success = true,
                    Message = "Student created successfully",
                    Data = MapToDTO(createdStudent),
                    StatusCode = 201
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateStudentAsync");
                return new ApiResponse<StudentDTO>
                {
                    Success = false,
                    Message = "An error occurred while creating the student",
                    Data = null,
                    StatusCode = 500
                };
            }
        }

        /// <summary>
        /// Updates an existing student
        /// </summary>
        /// <param name="id">Student ID to update</param>
        /// <param name="updateStudentDto">Student update DTO</param>
        /// <returns>API response containing updated student</returns>
        public async Task<ApiResponse<StudentDTO>> UpdateStudentAsync(int id, UpdateStudentDTO updateStudentDto)
        {
            try
            {
                _logger.LogInformation("Service: Updating student with ID: {StudentId}", id);

                if (id <= 0)
                {
                    _logger.LogWarning("Invalid student ID: {StudentId}", id);
                    return new ApiResponse<StudentDTO>
                    {
                        Success = false,
                        Message = "Invalid student ID",
                        Data = null,
                        StatusCode = 400
                    };
                }

                // Validate input
                if (string.IsNullOrWhiteSpace(updateStudentDto.Name) ||
                    string.IsNullOrWhiteSpace(updateStudentDto.Email) ||
                    string.IsNullOrWhiteSpace(updateStudentDto.Course) ||
                    updateStudentDto.Age <= 0)
                {
                    _logger.LogWarning("Invalid student data provided for update");
                    return new ApiResponse<StudentDTO>
                    {
                        Success = false,
                        Message = "Invalid student data. All fields are required and age must be positive.",
                        Data = null,
                        StatusCode = 400
                    };
                }

                var existingStudent = await _studentRepository.GetStudentByIdAsync(id);
                if (existingStudent == null)
                {
                    _logger.LogWarning("Student not found for update with ID: {StudentId}", id);
                    return new ApiResponse<StudentDTO>
                    {
                        Success = false,
                        Message = $"Student with ID {id} not found",
                        Data = null,
                        StatusCode = 404
                    };
                }

                // Check if new email is already used by another student
                if (existingStudent.Email != updateStudentDto.Email)
                {
                    var emailExists = await _studentRepository.EmailExistsAsync(updateStudentDto.Email);
                    if (emailExists)
                    {
                        _logger.LogWarning("Email already exists: {Email}", updateStudentDto.Email);
                        return new ApiResponse<StudentDTO>
                        {
                            Success = false,
                            Message = "Email already exists",
                            Data = null,
                            StatusCode = 400
                        };
                    }
                }

                var updatedStudent = new Student
                {
                    Id = id,
                    Name = updateStudentDto.Name,
                    Email = updateStudentDto.Email,
                    Age = updateStudentDto.Age,
                    Course = updateStudentDto.Course,
                    CreatedDate = existingStudent.CreatedDate
                };

                var result = await _studentRepository.UpdateStudentAsync(id, updatedStudent);

                if (result == null)
                {
                    return new ApiResponse<StudentDTO>
                    {
                        Success = false,
                        Message = "Failed to update student",
                        Data = null,
                        StatusCode = 500
                    };
                }

                return new ApiResponse<StudentDTO>
                {
                    Success = true,
                    Message = "Student updated successfully",
                    Data = MapToDTO(result),
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateStudentAsync with ID: {StudentId}", id);
                return new ApiResponse<StudentDTO>
                {
                    Success = false,
                    Message = "An error occurred while updating the student",
                    Data = null,
                    StatusCode = 500
                };
            }
        }

        /// <summary>
        /// Deletes a student
        /// </summary>
        /// <param name="id">Student ID to delete</param>
        /// <returns>API response indicating deletion success/failure</returns>
        public async Task<ApiResponse> DeleteStudentAsync(int id)
        {
            try
            {
                _logger.LogInformation("Service: Deleting student with ID: {StudentId}", id);

                if (id <= 0)
                {
                    _logger.LogWarning("Invalid student ID: {StudentId}", id);
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid student ID",
                        StatusCode = 400
                    };
                }

                var studentExists = await _studentRepository.StudentExistsAsync(id);
                if (!studentExists)
                {
                    _logger.LogWarning("Student not found for deletion with ID: {StudentId}", id);
                    return new ApiResponse
                    {
                        Success = false,
                        Message = $"Student with ID {id} not found",
                        StatusCode = 404
                    };
                }

                var result = await _studentRepository.DeleteStudentAsync(id);

                if (!result)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "Failed to delete student",
                        StatusCode = 500
                    };
                }

                return new ApiResponse
                {
                    Success = true,
                    Message = "Student deleted successfully",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteStudentAsync with ID: {StudentId}", id);
                return new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while deleting the student",
                    StatusCode = 500
                };
            }
        }

        /// <summary>
        /// Maps Student model to StudentDTO
        /// </summary>
        private StudentDTO MapToDTO(Student student)
        {
            return new StudentDTO
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Age = student.Age,
                Course = student.Course,
                CreatedDate = student.CreatedDate
            };
        }
    }
}
