using Microsoft.EntityFrameworkCore;
using StudentManagementSystem_ZestIndia.Data;
using StudentManagementSystem_ZestIndia.Models;
using StudentManagementSystem_ZestIndia.Repositories.Interfaces;

namespace StudentManagementSystem_ZestIndia.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for Student CRUD operations
    /// </summary>
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StudentRepository> _logger;

        public StudentRepository(ApplicationDbContext context, ILogger<StudentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all students from the database
        /// </summary>
        /// <returns>List of all students</returns>
        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all students from database");
                return await _context.Students.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all students");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a specific student by ID
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>Student if found, null otherwise</returns>
        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching student with ID: {StudentId}", id);
                return await _context.Students.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching student with ID: {StudentId}", id);
                throw;
            }
        }

        /// <summary>
        /// Creates a new student in the database
        /// </summary>
        /// <param name="student">Student object to create</param>
        /// <returns>Created student with generated ID</returns>
        public async Task<Student> CreateStudentAsync(Student student)
        {
            try
            {
                _logger.LogInformation("Creating new student: {StudentName}", student.Name);
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Student created successfully with ID: {StudentId}", student.Id);
                return student;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating student: {StudentName}", student.Name);
                throw;
            }
        }

        /// <summary>
        /// Updates an existing student in the database
        /// </summary>
        /// <param name="id">Student ID to update</param>
        /// <param name="student">Updated student object</param>
        /// <returns>Updated student if found, null otherwise</returns>
        public async Task<Student?> UpdateStudentAsync(int id, Student student)
        {
            try
            {
                _logger.LogInformation("Updating student with ID: {StudentId}", id);
                var existingStudent = await _context.Students.FindAsync(id);
                
                if (existingStudent == null)
                {
                    _logger.LogWarning("Student not found for update with ID: {StudentId}", id);
                    return null;
                }

                existingStudent.Name = student.Name;
                existingStudent.Email = student.Email;
                existingStudent.Age = student.Age;
                existingStudent.Course = student.Course;

                await _context.SaveChangesAsync();
                _logger.LogInformation("Student updated successfully with ID: {StudentId}", id);
                return existingStudent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating student with ID: {StudentId}", id);
                throw;
            }
        }

        /// <summary>
        /// Deletes a student from the database
        /// </summary>
        /// <param name="id">Student ID to delete</param>
        /// <returns>True if deleted successfully, false otherwise</returns>
        public async Task<bool> DeleteStudentAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting student with ID: {StudentId}", id);
                var student = await _context.Students.FindAsync(id);
                
                if (student == null)
                {
                    _logger.LogWarning("Student not found for deletion with ID: {StudentId}", id);
                    return false;
                }

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Student deleted successfully with ID: {StudentId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting student with ID: {StudentId}", id);
                throw;
            }
        }

        /// <summary>
        /// Checks if a student exists by ID
        /// </summary>
        /// <param name="id">Student ID to check</param>
        /// <returns>True if student exists, false otherwise</returns>
        public async Task<bool> StudentExistsAsync(int id)
        {
            return await _context.Students.AnyAsync(s => s.Id == id);
        }

        /// <summary>
        /// Checks if an email already exists
        /// </summary>
        /// <param name="email">Email to check</param>
        /// <returns>True if email exists, false otherwise</returns>
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Students.AnyAsync(s => s.Email == email);
        }
    }
}
