# Project Structure Reference

## Complete File Organization

```
StudentManagementSystem-ZestIndia/
│
├── 📁 Controllers/
│   ├── AuthController.cs              # Login endpoint (JWT generation)
│   └── StudentsController.cs          # CRUD endpoints (protected)
│
├── 📁 Models/
│   └── Student.cs                     # Database entity model
│
├── 📁 DTOs/
│   ├── StudentDTO.cs                  # Student response DTO
│   ├── CreateStudentDTO.cs            # Create request DTO
│   ├── UpdateStudentDTO.cs            # Update request DTO
│   ├── ApiResponse.cs                 # Response wrapper (generic & non-generic)
│   ├── LoginRequest.cs                # Login request DTO
│   └── LoginResponse.cs               # Login response with token
│
├── 📁 Data/
│   └── ApplicationDbContext.cs        # Entity Framework Core DbContext
│
├── 📁 Repositories/
│   ├── 📁 Interfaces/
│   │   └── IStudentRepository.cs      # Data access contract
│   └── 📁 Implementations/
│       └── StudentRepository.cs       # Data access implementation
│
├── 📁 Services/
│   ├── 📁 Interfaces/
│   │   └── IStudentService.cs         # Business logic contract
│   └── 📁 Implementations/
│       └── StudentService.cs          # Business logic implementation
│
├── 📁 Helpers/
│   └── JwtTokenHelper.cs              # JWT token generation utility
│
├── 📁 Middleware/
│   └── GlobalExceptionHandlingMiddleware.cs # Exception handling
│
├── 📁 Migrations/
│   ├── 20260508075119_InitialCreate.cs
│   ├── 20260508075119_InitialCreate.Designer.cs
│   └── ApplicationDbContextModelSnapshot.cs
│
├── 📁 Logs/
│   └── log-*.txt                      # Daily rotating log files
│
├── 📁 obj/
│   └── [Build artifacts]
│
├── 📁 bin/
│   └── [Compiled output]
│
├── 📄 Program.cs                      # Application startup & configuration
├── 📄 appsettings.json                # Application configuration
├── 📄 appsettings.Development.json    # Development settings (optional)
├── 📄 StudentManagementSystem-ZestIndia.csproj  # Project file
│
├── 📄 README.md                       # Main documentation
├── 📄 API_GUIDE.md                    # API endpoint reference
├── 📄 ARCHITECTURE.md                 # Architecture patterns explained
├── 📄 QUICK_START.md                  # Quick reference guide
├── 📄 IMPLEMENTATION_SUMMARY.md       # This project summary
│
└── 📄 .gitignore                      # Git ignore file
```

## Layer Dependencies (Flow)

```
HTTP Request
     ↓
[AuthController] ────────────────→ Login endpoint
     ↓
[StudentsController]  ──────────→ Protected endpoints (requires JWT)
     ↓
[StudentService]  ───────────────→ Business logic & validation
     ↓
[StudentRepository]  ──────────→ Database queries
     ↓
[ApplicationDbContext]  ───────→ Entity Framework Core
     ↓
[SQL Server Database]  ────────→ Persistent storage
```

## Key File Purposes

### Controllers
| File | Purpose |
|------|---------|
| AuthController.cs | Handles user authentication, generates JWT tokens |
| StudentsController.cs | Handles student CRUD operations, requires JWT auth |

### Models
| File | Purpose |
|------|---------|
| Student.cs | Represents student entity with validation annotations |

### DTOs (Data Transfer Objects)
| File | Purpose |
|------|---------|
| StudentDTO.cs | Response model when retrieving students |
| CreateStudentDTO.cs | Request model for creating new students |
| UpdateStudentDTO.cs | Request model for updating students |
| ApiResponse.cs | Generic and non-generic response wrappers |
| LoginRequest.cs | Request model for authentication |
| LoginResponse.cs | Response model with JWT token |

### Data Layer
| File | Purpose |
|------|---------|
| ApplicationDbContext.cs | EF Core DbContext, manages database connection |

### Repository Pattern
| File | Purpose |
|------|---------|
| IStudentRepository.cs | Contract for student data operations |
| StudentRepository.cs | Implementation of student data operations |

### Service Layer
| File | Purpose |
|------|---------|
| IStudentService.cs | Contract for student business logic |
| StudentService.cs | Business logic, validation, error handling |

### Helpers
| File | Purpose |
|------|---------|
| JwtTokenHelper.cs | JWT token generation and configuration |

### Middleware
| File | Purpose |
|------|---------|
| GlobalExceptionHandlingMiddleware.cs | Catches and handles all exceptions globally |

### Configuration
| File | Purpose |
|------|---------|
| Program.cs | Application startup, dependency injection, middleware pipeline |
| appsettings.json | Configuration: connection string, JWT, logging, etc. |
| .csproj | Project file: target framework, NuGet packages |

### Documentation
| File | Purpose |
|------|---------|
| README.md | Complete project overview and setup instructions |
| API_GUIDE.md | Detailed API endpoint documentation with examples |
| ARCHITECTURE.md | Architecture patterns, design decisions, best practices |
| QUICK_START.md | Quick reference for common tasks |
| IMPLEMENTATION_SUMMARY.md | Project completion report |

## Data Flow Example

### Creating a New Student

```
1. HTTP Request
   POST /api/students
   Body: { name, email, age, course }

2. StudentsController.CreateStudent()
   - Receives request
   - Validates ModelState
   - Calls StudentService.CreateStudentAsync()

3. StudentService.CreateStudentAsync()
   - Validates business rules
   - Checks email uniqueness
   - Transforms DTO to Model
   - Calls StudentRepository.CreateStudentAsync()
   - Maps Model to DTO
   - Returns ApiResponse<StudentDTO>

4. StudentRepository.CreateStudentAsync()
   - Creates Student entity
   - Adds to DbContext
   - Calls SaveChangesAsync()
   - Logs operation
   - Returns created student

5. ApplicationDbContext.SaveChangesAsync()
   - Tracks entities
   - Generates SQL
   - Executes INSERT statement
   - Saves to SQL Server

6. HTTP Response
   201 Created
   Body: ApiResponse { success: true, data: StudentDTO }
```

## Configuration Loading

```
appsettings.json
     ↓
Program.cs reads configuration
     ↓
    ├─→ Connection Strings
    ├─→ JWT Settings
    ├─→ Serilog Settings
    └─→ Application Settings
         ↓
    Services configured with values
```

## Authentication Flow

```
1. Login Request
   POST /api/auth/login
   Body: { username, password }
        ↓
2. AuthController validates credentials
        ↓
3. JwtTokenHelper generates token
   - Creates claims
   - Signs with secret key
   - Sets expiration
        ↓
4. LoginResponse returned with token
        ↓
5. Client stores token
        ↓
6. Subsequent requests
   Authorization: Bearer {token}
        ↓
7. ASP.NET Core JWT middleware validates token
   - Verifies signature
   - Checks expiration
   - Validates issuer/audience
        ↓
8. Request proceeds if token valid
        ↓
9. Request rejected with 401 if invalid
```

## Exception Handling Flow

```
Controller Action
        ↓
  Try to execute
        ↓
  Unhandled Exception
        ↓
GlobalExceptionHandlingMiddleware
        ├─→ Catches exception
        ├─→ Logs full error
        ├─→ Determines exception type
        ├─→ Maps to HTTP status code
        └─→ Returns safe error response
        ↓
Client receives
   JSON error response
```

## Logging Flow

```
Code logs event/error
        ↓
ILogger<T> interface
        ↓
Serilog processes
        ├─→ Console Sink
        │    └─→ Real-time output to terminal
        │
        └─→ File Sink
             ├─→ Logs/log-2025-01-10.txt
             ├─→ Logs/log-2025-01-11.txt
             └─→ (Daily rotation)
```

## Request Validation Layers

```
HTTP Request
     ↓
[Layer 1: Model Binding]
   ✓ JSON deserialization
   ✓ Route parameter parsing
     ↓
[Layer 2: Model State]
   ✓ Data annotations validation
   ✓ [Required], [EmailAddress], etc.
     ↓
[Layer 3: Service Validation]
   ✓ Business rule validation
   ✓ Email uniqueness check
   ✓ Data consistency
     ↓
[Layer 4: Database Constraints]
   ✓ Unique constraints
   ✓ Foreign keys
   ✓ Check constraints
     ↓
Validated Data or Error Response
```

## Build & Deployment Flow

```
Source Code
     ↓
dotnet build
     ├─→ Restore NuGet packages
     ├─→ Compile C# code
     └─→ Verify references
     ↓
Build Artifacts (bin/)
     ├─→ .dll files
     ├─→ .pdb files (debug symbols)
     └─→ .deps.json (dependencies)
     ↓
dotnet run
     ├─→ Load assemblies
     ├─→ Execute Program.cs
     ├─→ Register services
     ├─→ Start web server
     └─→ Listen on https://localhost:5001
     ↓
Application Running
     ↓
Client Requests → API Responses
```

---

## File Statistics

- **Total Files**: 25+ (source code only)
- **Controllers**: 2
- **Models**: 1
- **DTOs**: 6
- **Interfaces**: 2
- **Implementations**: 2
- **Helpers**: 1
- **Middleware**: 1
- **Configuration**: 3
- **Documentation**: 5
- **Lines of Code**: ~1500+
- **Build Status**: ✅ Successful

---

## Quick Navigation

- **Start Here**: README.md
- **Setup Guide**: QUICK_START.md
- **API Endpoints**: API_GUIDE.md
- **Architecture Details**: ARCHITECTURE.md
- **Main Application**: Program.cs
- **Configuration**: appsettings.json
- **Authentication**: Controllers/AuthController.cs
- **Student CRUD**: Controllers/StudentsController.cs
- **Business Logic**: Services/Implementations/StudentService.cs
- **Data Access**: Repositories/Implementations/StudentRepository.cs
