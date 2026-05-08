# Architecture & Best Practices Documentation

## Project Structure Explanation

### Controllers/ 
Contains API endpoint controllers that handle HTTP requests and responses.

**AuthController.cs**
- Handles user authentication
- Generates JWT tokens for API access
- Validates credentials (demo: admin/admin123)
- Returns token with expiration time

**StudentsController.cs**
- Handles all student-related CRUD operations
- Requires JWT authentication (protected with [Authorize])
- Returns consistent API responses
- Includes XML documentation for Swagger

---

### Models/
Contains Entity Framework Core model classes representing database tables.

**Student.cs**
- Represents a student record in the database
- Includes data annotations for validation
- Properties: Id, Name, Email, Age, Course, CreatedDate
- Uses nullable reference types for null safety

---

### DTOs/ (Data Transfer Objects)
Contains classes for API request/response contracts. DTOs decouple internal models from API contracts.

**StudentDTO.cs** - Response DTO for retrieving students
**CreateStudentDTO.cs** - Request DTO for creating students
**UpdateStudentDTO.cs** - Request DTO for updating students
**ApiResponse<T>** - Generic wrapper for all API responses
**ApiResponse** - Non-generic response wrapper for operations without data
**LoginRequest.cs** - Request DTO for authentication
**LoginResponse.cs** - Response DTO with JWT token

**Why DTOs?**
- Separate API contract from domain model
- Enable API versioning flexibility
- Hide sensitive internal fields
- Reduce data transfer size
- Enable automatic validation

---

### Data/
Contains Entity Framework Core database context.

**ApplicationDbContext.cs**
- Inherits from DbContext
- Configures DbSets for database tables
- Manages database connections
- Enables migrations

---

### Repositories/
Implements the Repository Pattern for data access abstraction.

**Interfaces/IStudentRepository.cs**
- Defines contract for student data operations
- Methods: GetAllStudents, GetStudentById, CreateStudent, UpdateStudent, DeleteStudent, StudentExists, EmailExists
- Promotes loose coupling and testability

**Implementations/StudentRepository.cs**
- Implements IStudentRepository interface
- Contains all database queries using EF Core
- Handles DbContext interaction
- Includes error logging for diagnostics
- Async operations for performance

**Repository Pattern Benefits:**
- Abstracts data access logic
- Enables switching databases without changing business logic
- Facilitates unit testing with mock repositories
- Centralizes query logic
- Improves code maintainability

---

### Services/
Contains business logic layer between controllers and repositories.

**Interfaces/IStudentService.cs**
- Defines business operations contract
- Returns standardized API responses
- Methods match controller endpoints

**Implementations/StudentService.cs**
- Implements IStudentService
- Contains business logic and validation
- Calls repository for data operations
- Performs data transformation (Model → DTO)
- Handles error cases with appropriate status codes
- Implements input validation
- Manages complex business rules (e.g., email uniqueness)

**Service Layer Responsibilities:**
- Business logic
- Input validation
- Error handling
- Response formatting
- Cross-cutting concerns

---

### Helpers/
Contains utility and helper classes.

**JwtTokenHelper.cs**
- Interface: IJwtTokenHelper
- Implementation: JwtTokenHelper
- Generates JWT tokens
- Configures token parameters from appsettings.json
- Validates JWT secret length
- Provides token expiration time
- Includes comprehensive logging

**JWT Configuration:**
- Symmetric key: HMAC-SHA256
- Signed with configured secret
- Includes issuer and audience validation
- Token expiration enforcement
- Claims-based identity

---

### Middleware/
Contains custom ASP.NET Core middleware.

**GlobalExceptionHandlingMiddleware.cs**
- Catches unhandled exceptions globally
- Returns consistent error responses
- Logs exceptions with full context
- Maps exception types to HTTP status codes
- Prevents sensitive exception details leaking to client

**Exception Handling:**
- ArgumentNullException → 400 Bad Request
- ArgumentException → 400 Bad Request
- InvalidOperationException → 400 Bad Request
- UnauthorizedAccessException → 401 Unauthorized
- KeyNotFoundException → 404 Not Found
- Other exceptions → 500 Internal Server Error

---

## Layered Architecture

```
┌─────────────────────────────────────────┐
│         Controllers (HTTP Layer)         │
│  • AuthController                       │
│  • StudentsController                   │
└─────────────────────┬───────────────────┘
                      │
┌─────────────────────▼───────────────────┐
│         Services (Business Layer)        │
│  • IStudentService                      │
│  • StudentService                       │
│  ✓ Validation                           │
│  ✓ Business Logic                       │
└─────────────────────┬───────────────────┘
                      │
┌─────────────────────▼───────────────────┐
│    Repositories (Data Access Layer)      │
│  • IStudentRepository                   │
│  • StudentRepository                    │
│  ✓ Database Queries                     │
│  ✓ CRUD Operations                      │
└─────────────────────┬───────────────────┘
                      │
┌─────────────────────▼───────────────────┐
│    Data Layer (Entity Framework)         │
│  • ApplicationDbContext                 │
│  • Models                               │
│  ✓ Database Connection                  │
│  ✓ Entity Mapping                       │
└─────────────────────────────────────────┘
```

**Benefits of Layering:**
- Clear separation of concerns
- Easy to test each layer independently
- Changes in one layer don't affect others
- Reusable components
- Scalable architecture

---

## Dependency Injection (DI)

All dependencies are registered in `Program.cs`:

```csharp
// Service registration
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IJwtTokenHelper, JwtTokenHelper>();
```

**Service Lifetimes:**
- **Transient**: New instance created each time (lightweight stateless objects)
- **Scoped**: One instance per HTTP request (database contexts)
- **Singleton**: One instance for application lifetime (configuration, logging)

---

## API Response Pattern

All endpoints return consistent response format:

```csharp
// Generic response with data
ApiResponse<T> {
  Success: bool,
  Message: string,
  Data: T,
  StatusCode: int
}

// Non-generic response without data
ApiResponse {
  Success: bool,
  Message: string,
  StatusCode: int
}
```

**Example Success Response:**
```json
{
  "success": true,
  "message": "Student created successfully",
  "data": { ... },
  "statusCode": 201
}
```

**Example Error Response:**
```json
{
  "success": false,
  "message": "Email already exists",
  "statusCode": 400
}
```

---

## Authentication Flow

1. **Login Request**
   ```
   POST /api/auth/login
   {username, password}
   ```

2. **Token Generation**
   - Validate credentials
   - Generate JWT with claims
   - Sign with secret key
   - Set expiration

3. **Token Response**
   ```
   LoginResponse {
     token: JWT,
     username: string,
     expiresAt: DateTime
   }
   ```

4. **Authenticated Request**
   ```
   Authorization: Bearer {token}
   ```

5. **Token Validation**
   - Extract token from header
   - Validate signature
   - Check expiration
   - Verify issuer/audience
   - Extract claims

---

## Validation Strategy

### Input Validation Levels

1. **Model Level** (Student.cs)
   - Data annotations: `[Required]`, `[EmailAddress]`, `[Range]`, `[StringLength]`
   - Automatic client/server-side validation

2. **DTO Level** (DTOs)
   - Implicit via model binding
   - Automatic ModelState validation

3. **Service Level** (StudentService.cs)
   - Business rule validation
   - Email uniqueness check
   - Age range validation
   - Data consistency checks

4. **Database Level**
   - Constraints and indexes
   - Referential integrity

### Example Validation in Service
```csharp
// Check input validity
if (string.IsNullOrWhiteSpace(createStudentDto.Name))
    return BadRequest("Name is required");

// Check business rule
var emailExists = await _studentRepository.EmailExistsAsync(email);
if (emailExists)
    return BadRequest("Email already exists");

// Validate age range
if (createStudentDto.Age <= 0 || createStudentDto.Age > 120)
    return BadRequest("Age must be between 1 and 120");
```

---

## Error Handling Strategy

1. **Validation Errors** → 400 Bad Request
2. **Authentication Errors** → 401 Unauthorized
3. **Authorization Errors** → 403 Forbidden
4. **Not Found** → 404 Not Found
5. **Server Errors** → 500 Internal Server Error

### Global Exception Middleware
- Catches all unhandled exceptions
- Logs full exception details
- Returns safe error response to client
- Prevents exception details leaking

---

## Logging Strategy

### Serilog Configuration

**Sinks:**
- Console output for real-time monitoring
- File output with daily rotation (Logs/ folder)

**Log Levels:**
- Information: General application flow
- Warning: Potentially problematic situations
- Error: Error events
- Fatal: Severe errors causing shutdown

**Output Template:**
```
{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}
```

### Logging Examples
```csharp
// Information
_logger.LogInformation("Creating new student: {StudentName}", student.Name);

// Warning
_logger.LogWarning("Student not found with ID: {StudentId}", id);

// Error
_logger.LogError(ex, "Error while fetching all students");
```

---

## Database Migration

### EF Core Migrations

Migrations are version-controlled database schema changes.

**Creating Migrations:**
```bash
dotnet ef migrations add InitialCreate
```

**Applying Migrations:**
```bash
dotnet ef database update
```

**Program.cs automatically applies migrations:**
```csharp
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}
```

**Benefits:**
- Version control for database schema
- Reproducible deployments
- Easy rollback capability
- Team collaboration

---

## Async/Await Pattern

All I/O operations use async/await:

```csharp
// Repository - Async database operations
public async Task<Student> GetStudentByIdAsync(int id)
{
    return await _context.Students.FindAsync(id);
}

// Service - Async method composition
public async Task<ApiResponse<StudentDTO>> GetStudentByIdAsync(int id)
{
    var student = await _studentRepository.GetStudentByIdAsync(id);
    return new ApiResponse<StudentDTO> { ... };
}

// Controller - Async endpoint
public async Task<ActionResult> GetStudentById(int id)
{
    var result = await _studentService.GetStudentByIdAsync(id);
    return Ok(result);
}
```

**Benefits:**
- Non-blocking I/O operations
- Better resource utilization
- Improved scalability
- Responsive server

---

## Security Practices

1. **JWT Configuration**
   - Strong secret (32+ characters)
   - Signed with HMAC-SHA256
   - Token expiration enforced
   - Issuer/Audience validation

2. **Password Handling**
   - Demo credentials only (use bcrypt in production)
   - Never log passwords
   - Always use HTTPS in production

3. **Input Validation**
   - Sanitize all inputs
   - EF Core parameterized queries (SQL injection prevention)
   - Model validation annotations

4. **Error Handling**
   - Never expose sensitive details
   - Log internal errors securely
   - Return generic error messages to client

5. **CORS Configuration**
   - AllowAll policy for development
   - Configure specific origins in production

---

## Code Quality

### SOLID Principles

**S - Single Responsibility Principle**
- Each class has one reason to change
- Controllers: HTTP handling
- Services: Business logic
- Repositories: Data access

**O - Open/Closed Principle**
- Classes open for extension, closed for modification
- Interface-based design

**L - Liskov Substitution Principle**
- Implementations can be substituted for interfaces
- Contracts honored by all implementations

**I - Interface Segregation Principle**
- Clients depend on focused interfaces
- Not forced to depend on methods they don't use

**D - Dependency Inversion Principle**
- Depend on abstractions, not concretions
- Dependency injection via constructor

### Best Practices

✅ Meaningful variable/method names
✅ DRY (Don't Repeat Yourself)
✅ Comments for complex logic only
✅ Consistent formatting
✅ Null-safety with nullable annotations
✅ Async/await for I/O
✅ Proper error handling
✅ Comprehensive logging
✅ XML documentation
✅ Constructor dependency injection

---

## Performance Considerations

1. **Async/Await**: Non-blocking I/O operations
2. **Eager/Lazy Loading**: Use appropriate EF Core loading strategies
3. **Indexing**: Ensure database indexes on frequently queried columns
4. **Connection Pooling**: Automatic with DbContext
5. **Caching**: Consider for frequently accessed data
6. **Pagination**: Implement for large result sets

---

## Testing Considerations

### Unit Testing
- Mock repositories
- Test service business logic
- Test validation logic
- Test error handling

### Integration Testing
- Test full request/response cycle
- Test database operations
- Test authentication flow

### Example Test Structure
```csharp
[Fact]
public async Task CreateStudent_WithValidData_ReturnsSuccess()
{
    // Arrange
    var mockRepository = new Mock<IStudentRepository>();
    var service = new StudentService(mockRepository.Object);
    
    // Act
    var result = await service.CreateStudentAsync(validDto);
    
    // Assert
    Assert.True(result.Success);
}
```

---

## Deployment Considerations

1. **Environment Configuration**: Use appsettings.{Environment}.json
2. **Secrets Management**: Use Azure Key Vault or environment variables
3. **Database**: Run migrations automatically or manually before deployment
4. **Logging**: Ensure proper log aggregation
5. **Monitoring**: Set up application monitoring
6. **HTTPS**: Required in production
7. **CORS**: Configure for actual client origins
8. **Rate Limiting**: Consider for public APIs

---

## Common Questions

**Q: Why DTOs instead of domain models?**
A: DTOs decouple API contracts from internal models, enabling versioning flexibility and hiding sensitive fields.

**Q: Why use Repository Pattern?**
A: Abstracts data access logic, enables easy testing with mocks, and simplifies database switching.

**Q: Why async/await everywhere?**
A: Improves scalability by not blocking threads during I/O operations.

**Q: Why dependency injection?**
A: Enables loose coupling, easier testing, and centralized dependency management.

**Q: Where should business logic go?**
A: In the Service layer, not Controllers or Repositories.

**Q: How is security implemented?**
A: JWT authentication protects endpoints, validation prevents bad data, middleware handles exceptions safely.
