# Student Management System - ASP.NET Core Web API

A production-grade Student Management System API built with ASP.NET Core 10, Entity Framework Core, JWT Authentication, and SQL Server.

## Project Overview

This project demonstrates enterprise-level architecture and best practices for building a RESTful API with proper layering, authentication, error handling, and logging.

## Features

✅ **Complete CRUD Operations** for Student management
✅ **JWT Authentication** with token generation and validation
✅ **Role-based Authorization** on protected endpoints
✅ **Global Exception Handling** middleware
✅ **Serilog Logging** (Console + File)
✅ **Swagger API Documentation**
✅ **Entity Framework Core** with SQL Server
✅ **Repository Pattern** for data access
✅ **Service Layer** for business logic
✅ **Dependency Injection** configuration
✅ **Async/Await** patterns throughout
✅ **DTOs** for requests and responses
✅ **Comprehensive Error Handling**

## Technology Stack

- **Framework**: ASP.NET Core 10 (.NET 10)
- **Database**: SQL Server
- **ORM**: Entity Framework Core 10
- **Authentication**: JWT (JSON Web Tokens)
- **Logging**: Serilog
- **API Documentation**: Swagger/OpenAPI
- **Language**: C# 14

## Project Architecture

```
StudentManagementSystem-ZestIndia/
├── Controllers/          # API endpoints
│   ├── AuthController.cs           # Authentication/Login
│   └── StudentsController.cs       # Student CRUD endpoints
├── Models/               # Database entities
│   └── Student.cs
├── DTOs/                 # Data Transfer Objects
│   ├── StudentDTO.cs
│   ├── CreateStudentDTO.cs
│   ├── UpdateStudentDTO.cs
│   ├── ApiResponse.cs
│   ├── LoginRequest.cs
│   └── LoginResponse.cs
├── Data/                 # Data access layer
│   └── ApplicationDbContext.cs
├── Repositories/         # Repository pattern
│   ├── Interfaces/
│   │   └── IStudentRepository.cs
│   └── Implementations/
│       └── StudentRepository.cs
├── Services/             # Business logic
│   ├── Interfaces/
│   │   └── IStudentService.cs
│   └── Implementations/
│       └── StudentService.cs
├── Helpers/              # Helper classes
│   └── JwtTokenHelper.cs
├── Middleware/           # Custom middleware
│   └── GlobalExceptionHandlingMiddleware.cs
├── Migrations/           # EF Core migrations
├── Program.cs            # Application startup
└── appsettings.json      # Configuration

```

## Setup Instructions

### 1. Prerequisites
- .NET 10 SDK
- SQL Server (LocalDB or Express)
- Visual Studio 2026 or VS Code

### 2. Database Setup

Update the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS01;Database=StudentDBZestIndia;Trusted_Connection=True;TrustServerCertificate=True"
}
```

Apply migrations:
```bash
dotnet ef database update
```

### 3. JWT Configuration

Update `JwtSettings` in `appsettings.json`:

```json
"JwtSettings": {
  "Secret": "your-super-secret-key-at-least-32-characters-long-for-hs256",
  "Issuer": "StudentManagementSystem",
  "Audience": "StudentManagementAPI",
  "ExpirationInMinutes": 60
}
```

⚠️ **Important**: Change the secret to a secure, random string of at least 32 characters in production!

### 4. Run the Application

```bash
dotnet run
```

The application will start at `https://localhost:5001` (or the configured port).

## API Endpoints

### Authentication

#### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "admin123"
}
```

**Response** (201 Created):
```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "username": "admin",
    "expiresAt": "2025-01-10T12:30:00Z"
  },
  "statusCode": 200
}
```

### Students (Requires JWT Token)

Include the token in the Authorization header:
```
Authorization: Bearer {token}
```

#### Get All Students
```http
GET /api/students
Authorization: Bearer {token}
```

#### Get Student by ID
```http
GET /api/students/{id}
Authorization: Bearer {token}
```

#### Create Student
```http
POST /api/students
Content-Type: application/json
Authorization: Bearer {token}

{
  "name": "John Doe",
  "email": "john@example.com",
  "age": 20,
  "course": "Computer Science"
}
```

#### Update Student
```http
PUT /api/students/{id}
Content-Type: application/json
Authorization: Bearer {token}

{
  "name": "Jane Doe",
  "email": "jane@example.com",
  "age": 21,
  "course": "Data Science"
}
```

#### Delete Student
```http
DELETE /api/students/{id}
Authorization: Bearer {token}
```

## Demo Credentials

| Field | Value |
|-------|-------|
| Username | admin |
| Password | admin123 |

⚠️ **Note**: This is for demo purposes only. In production, implement proper user authentication with password hashing.

## HTTP Status Codes

| Code | Meaning |
|------|---------|
| 200 | OK - Request successful |
| 201 | Created - Resource created successfully |
| 400 | Bad Request - Invalid input data |
| 401 | Unauthorized - Authentication required |
| 404 | Not Found - Resource not found |
| 500 | Internal Server Error - Server error occurred |

## Error Response Format

All error responses follow a consistent format:

```json
{
  "success": false,
  "message": "Error description",
  "statusCode": 400
}
```

## Logging

Logs are written to:
- **Console**: Real-time output in the terminal
- **File**: `Logs/log-{date}.txt` files (daily rotation)

Log levels: Information, Warning, Error, Fatal

## Architecture Patterns Used

### 1. Layered Architecture
- **Controllers**: Handle HTTP requests/responses
- **Services**: Business logic and validation
- **Repositories**: Data access abstraction
- **Data**: Entity Framework Core DbContext

### 2. Repository Pattern
Abstracts data access logic and provides a cleaner separation of concerns.

### 3. Dependency Injection
All dependencies are injected via constructor for loose coupling and testability.

### 4. DTO Pattern
Separates internal models from API contracts, providing API versioning flexibility.

### 5. Global Exception Handling
Centralized exception handling through middleware for consistent error responses.

## Best Practices Implemented

✅ Async/await for I/O operations
✅ Proper null-safety with nullable annotations
✅ Comprehensive logging at all layers
✅ Input validation and error messages
✅ RESTful API standards
✅ Separation of concerns
✅ DRY (Don't Repeat Yourself)
✅ SOLID principles
✅ Constructor injection
✅ XML documentation comments

## Configuration Files

### appsettings.json
- Connection strings
- JWT settings
- Serilog logging configuration
- Feature flags and application settings

### Program.cs
- Service registration
- Middleware configuration
- Authentication and authorization setup
- Swagger/OpenAPI configuration
- Database migration execution

## NuGet Packages

| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.EntityFrameworkCore.SqlServer | 10.0.7 | SQL Server provider for EF Core |
| Microsoft.AspNetCore.Authentication.JwtBearer | 10.0.5 | JWT authentication |
| System.IdentityModel.Tokens.Jwt | 8.0.1 | JWT token creation and validation |
| Serilog | 4.2.0 | Structured logging |
| Serilog.AspNetCore | 8.1.0 | ASP.NET Core integration |
| Serilog.Sinks.Console | 6.1.0 | Console output sink |
| Serilog.Sinks.File | 6.0.0 | File output sink |
| Swashbuckle.AspNetCore | 6.4.0 | Swagger/OpenAPI support |

## Running the Application

### Development Mode
```bash
dotnet run
```

### Production Build
```bash
dotnet publish -c Release
```

### Entity Framework Migrations

Create a new migration:
```bash
dotnet ef migrations add MigrationName
```

Apply migrations:
```bash
dotnet ef database update
```

View pending migrations:
```bash
dotnet ef migrations list
```

## Testing the API

### Using cURL
```bash
# Get JWT Token
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'

# Get All Students (replace TOKEN with actual token)
curl -X GET https://localhost:5001/api/students \
  -H "Authorization: Bearer TOKEN"
```

### Using Swagger UI
Navigate to `https://localhost:5001` to access Swagger UI.
1. Click "Authorize" button
2. Paste your JWT token with "Bearer " prefix
3. Test endpoints directly from the UI

### Using Postman
1. Create a new request
2. Set Authorization type to "Bearer Token"
3. Paste the JWT token
4. Make requests to the API

## Database Schema

### Students Table
```sql
CREATE TABLE Students (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Age INT NOT NULL CHECK (Age > 0 AND Age <= 120),
    Course NVARCHAR(100) NOT NULL,
    CreatedDate DATETIME NOT NULL
)
```

## Security Considerations

1. **JWT Secret**: Use a strong, random secret (minimum 32 characters)
2. **Password Hashing**: Hash passwords using bcrypt or similar in production
3. **HTTPS**: Always use HTTPS in production
4. **CORS**: Configure CORS appropriately for your client origins
5. **SQL Injection**: EF Core parameterized queries prevent SQL injection
6. **Token Expiration**: Tokens expire after the configured time
7. **Rate Limiting**: Consider adding rate limiting middleware
8. **Input Validation**: All inputs are validated before processing

## Troubleshooting

### Database Connection Issues
- Verify SQL Server is running
- Check connection string in appsettings.json
- Ensure database exists or migrations will create it

### JWT Token Errors
- Verify secret is at least 32 characters
- Check token hasn't expired
- Ensure "Bearer " prefix is included in Authorization header

### Swagger Not Loading
- Clear browser cache
- Check that Swagger is enabled in Program.cs
- Verify application is running

## Future Enhancements

- [ ] Unit and integration tests with xUnit
- [ ] Role-based access control (RBAC)
- [ ] Pagination for GetAll endpoint
- [ ] Search and filtering capabilities
- [ ] Audit logging
- [ ] Docker containerization
- [ ] API versioning
- [ ] Rate limiting
- [ ] Refresh token implementation
- [ ] Email verification
- [ ] React/Angular frontend

## Contributing

Follow these guidelines:
- Use meaningful variable and method names
- Add XML comments for public members
- Keep methods focused on single responsibility
- Use async/await for I/O operations
- Add appropriate error handling and logging

## License

This project is for educational purposes.

## Support

For issues or questions, refer to the inline code documentation and appsettings.json.
