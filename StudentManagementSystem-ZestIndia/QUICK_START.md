# Quick Reference Guide

## Project Setup Checklist

- [ ] Clone/open project
- [ ] Verify SQL Server is running
- [ ] Update connection string in `appsettings.json` if needed
- [ ] Run migrations: `dotnet ef database update`
- [ ] Start application: `dotnet run`
- [ ] Access Swagger: `https://localhost:5001`

## First Time Users

### Step 1: Get Authentication Token
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'
```

Copy the `token` from response.

### Step 2: Create a Student
```bash
curl -X POST https://localhost:5001/api/students \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "name": "John Doe",
    "email": "john@example.com",
    "age": 20,
    "course": "Computer Science"
  }'
```

### Step 3: Get All Students
```bash
curl -X GET https://localhost:5001/api/students \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

## File Structure at a Glance

```
Controllers/          → API endpoints
Models/              → Database entities
DTOs/                → Request/response objects
Data/                → EF Core context
Repositories/        → Data access layer
Services/            → Business logic layer
Helpers/             → Utility functions
Middleware/          → Custom middleware
Migrations/          → Database versions
```

## Key Configuration Files

### appsettings.json
- Connection string (SQL Server)
- JWT secret and settings
- Serilog logging configuration

### Program.cs
- Service registration (DI)
- Database context setup
- JWT authentication
- Swagger configuration
- Middleware pipeline

## API Endpoints Summary

| Method | Endpoint | Auth | Purpose |
|--------|----------|------|---------|
| POST | /api/auth/login | ❌ | Get JWT token |
| GET | /api/students | ✅ | Get all students |
| GET | /api/students/{id} | ✅ | Get student by ID |
| POST | /api/students | ✅ | Create student |
| PUT | /api/students/{id} | ✅ | Update student |
| DELETE | /api/students/{id} | ✅ | Delete student |

## Common Commands

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run
```

### Create Migration
```bash
dotnet ef migrations add MigrationName
```

### Apply Migrations
```bash
dotnet ef database update
```

### View Migrations
```bash
dotnet ef migrations list
```

### Clean Build
```bash
dotnet clean
dotnet build
```

## Debugging

### Enable Debug Logging
In `appsettings.json`:
```json
"MinimumLevel": "Debug"
```

### View Logs
Logs are in `Logs/` directory with daily rotation.

### Visual Studio Debugging
1. Press `F5` to start with debugger
2. Set breakpoints by clicking line numbers
3. Use Debug menu for stepping through code

## Common Issues

### Issue: Cannot connect to database
**Solution:**
- Check SQL Server is running
- Verify connection string in appsettings.json
- Ensure database exists or migrations will create it

### Issue: JWT token invalid/expired
**Solution:**
- Get new token from login endpoint
- Check token hasn't expired (default 60 minutes)
- Ensure "Bearer " prefix in Authorization header

### Issue: Email already exists error
**Solution:**
- Use different email for new student
- Check existing records

### Issue: Build fails
**Solution:**
- Run `dotnet restore`
- Check NuGet packages are correct version
- Delete bin/ and obj/ folders and rebuild

## Performance Tips

1. **Connection Pooling**: Automatic, no configuration needed
2. **Async Operations**: All I/O is async
3. **Indexing**: Add database indexes on frequently filtered columns
4. **Logging Levels**: Use Warning/Error in production to reduce I/O
5. **Response Caching**: Consider for GET endpoints

## Security Reminders

⚠️ **Before Production:**
1. Change JWT secret to strong random string (32+ characters)
2. Implement real password hashing (bcrypt)
3. Enable HTTPS
4. Configure CORS for actual client domains
5. Set up environment-specific secrets
6. Review and update appsettings.json
7. Enable database backups
8. Add rate limiting

## Testing Endpoints

### Using Postman
1. Create new request
2. Select method (GET, POST, etc.)
3. Enter URL
4. Add Authorization header: `Bearer {token}`
5. For POST/PUT: Add request body
6. Click Send

### Using VS Code REST Client
Create `test.http`:
```http
### Login
POST https://localhost:5001/api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "admin123"
}

### Get All Students
@token = eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
GET https://localhost:5001/api/students
Authorization: Bearer @token
```

## Documentation Files

| File | Purpose |
|------|---------|
| README.md | Project overview and setup |
| API_GUIDE.md | Detailed API documentation |
| ARCHITECTURE.md | Architecture and patterns explained |
| This file | Quick reference |

## Support

For detailed information:
- See **README.md** for full project documentation
- See **API_GUIDE.md** for API endpoint details
- See **ARCHITECTURE.md** for architectural patterns
- Check code comments in relevant files

## Version Info

- **.NET**: 10.0
- **C#**: 14.0
- **SQL Server**: Supported versions
- **Entity Framework Core**: 10.0.7
- **Swagger/Swashbuckle**: 6.4.0

## Next Steps

1. ✅ Setup complete
2. ✅ Run the application
3. ✅ Get authentication token
4. ✅ Test endpoints
5. ✅ Explore code structure
6. ✅ Read ARCHITECTURE.md for deeper understanding
7. 📝 Customize business logic as needed
8. 🧪 Add unit tests (xUnit)
9. 🐳 Consider Docker containerization
10. ☁️ Plan deployment strategy

## Key Metrics

- **Total Controllers**: 2 (Auth, Students)
- **API Endpoints**: 6 (1 public, 5 protected)
- **Database Tables**: 1 (Students)
- **Service Layers**: 3 (Controllers → Services → Repositories)
- **Lines of Code**: ~1500+ production code
- **NuGet Packages**: 8

---

**Happy Coding! 🚀**
