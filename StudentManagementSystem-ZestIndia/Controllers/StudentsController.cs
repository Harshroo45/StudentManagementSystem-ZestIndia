using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem_ZestIndia.DTOs;
using StudentManagementSystem_ZestIndia.Services.Interfaces;

namespace StudentManagementSystem_ZestIndia.Controllers
{
    /// <summary>
    /// Controller for Student CRUD operations
    /// All endpoints require JWT authentication
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        /// <summary>
        /// Get all students
        /// </summary>
        /// <returns>List of all students</returns>
        /// <response code="200">Returns the list of students</response>
        /// <response code="401">If user is not authenticated</response>
        /// <response code="500">If an internal server error occurs</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentDTO>>>> GetAllStudents()
        {
            _logger.LogInformation("Getting all students");
            var result = await _studentService.GetAllStudentsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get a student by ID
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>Student details</returns>
        /// <response code="200">Returns the student</response>
        /// <response code="400">If student ID is invalid</response>
        /// <response code="401">If user is not authenticated</response>
        /// <response code="404">If student is not found</response>
        /// <response code="500">If an internal server error occurs</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<StudentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<StudentDTO>>> GetStudentById(int id)
        {
            _logger.LogInformation("Getting student with ID: {StudentId}", id);
            var result = await _studentService.GetStudentByIdAsync(id);

            if (result.StatusCode == 404)
                return NotFound(result);

            if (result.StatusCode == 400)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Create a new student
        /// </summary>
        /// <param name="createStudentDto">Student creation details</param>
        /// <returns>Created student details</returns>
        /// <response code="201">Student created successfully</response>
        /// <response code="400">If student data is invalid</response>
        /// <response code="401">If user is not authenticated</response>
        /// <response code="500">If an internal server error occurs</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<StudentDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<StudentDTO>>> CreateStudent([FromBody] CreateStudentDTO createStudentDto)
        {
            _logger.LogInformation("Creating new student: {StudentName}", createStudentDto.Name);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _studentService.CreateStudentAsync(createStudentDto);

            if (result.StatusCode == 400)
                return BadRequest(result);

            if (result.StatusCode == 500)
                return StatusCode(500, result);

            return CreatedAtAction(nameof(GetStudentById), new { id = result.Data?.Id }, result);
        }

        /// <summary>
        /// Update an existing student
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <param name="updateStudentDto">Updated student details</param>
        /// <returns>Updated student details</returns>
        /// <response code="200">Student updated successfully</response>
        /// <response code="400">If student data is invalid</response>
        /// <response code="401">If user is not authenticated</response>
        /// <response code="404">If student is not found</response>
        /// <response code="500">If an internal server error occurs</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<StudentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<StudentDTO>>> UpdateStudent(int id, [FromBody] UpdateStudentDTO updateStudentDto)
        {
            _logger.LogInformation("Updating student with ID: {StudentId}", id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _studentService.UpdateStudentAsync(id, updateStudentDto);

            if (result.StatusCode == 404)
                return NotFound(result);

            if (result.StatusCode == 400)
                return BadRequest(result);

            if (result.StatusCode == 500)
                return StatusCode(500, result);

            return Ok(result);
        }

        /// <summary>
        /// Delete a student
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>Deletion result</returns>
        /// <response code="200">Student deleted successfully</response>
        /// <response code="400">If student ID is invalid</response>
        /// <response code="401">If user is not authenticated</response>
        /// <response code="404">If student is not found</response>
        /// <response code="500">If an internal server error occurs</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> DeleteStudent(int id)
        {
            _logger.LogInformation("Deleting student with ID: {StudentId}", id);
            var result = await _studentService.DeleteStudentAsync(id);

            if (result.StatusCode == 404)
                return NotFound(result);

            if (result.StatusCode == 400)
                return BadRequest(result);

            if (result.StatusCode == 500)
                return StatusCode(500, result);

            return Ok(result);
        }
    }
}
