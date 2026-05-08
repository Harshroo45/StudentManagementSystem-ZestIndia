# API Usage Guide

## Quick Start

### 1. Start the Application
```bash
cd StudentManagementSystem-ZestIndia
dotnet run
```

Application runs at: `https://localhost:5001`

### 2. Access Swagger UI
Open: `https://localhost:5001`

### 3. Get Authentication Token

Use Postman, Insomnia, or any HTTP client:

**Request:**
```http
POST https://localhost:5001/api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "admin123"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy9zY2hlbWEvY2xhaW1zL25hbWUiOiJhZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3NjaGVtYS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiJhZG1pbiIsIm5iZiI6MTczNjQyMDEyMywiZXhwIjoxNzM2NDIzNzIzLCJpYXQiOjE3MzY0MjAxMjMsImlzcyI6IlN0dWRlbnRNYW5hZ2VtZW50U3lzdGVtIiwiYXVkIjoiU3R1ZGVudE1hbmFnZW1lbnRBUEkifQ.abc123...",
    "username": "admin",
    "expiresAt": "2025-01-10T12:30:00Z"
  },
  "statusCode": 200
}
```

Copy the `token` value.

---

## Complete API Reference

### Authentication Endpoints

#### POST /api/auth/login
Authenticate user and get JWT token.

**Request:**
```json
{
  "username": "admin",
  "password": "admin123"
}
```

**Response (200 OK):**
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

**Response (401 Unauthorized):**
```json
{
  "success": false,
  "message": "Invalid username or password",
  "statusCode": 401
}
```

---

### Student Endpoints

⚠️ **All student endpoints require JWT authentication**

Add header:
```
Authorization: Bearer {token}
```

---

#### GET /api/students
Get all students.

**Headers:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Retrieved 3 student(s) successfully",
  "data": [
    {
      "id": 1,
      "name": "John Doe",
      "email": "john@example.com",
      "age": 20,
      "course": "Computer Science",
      "createdDate": "2025-01-10T10:00:00Z"
    },
    {
      "id": 2,
      "name": "Jane Smith",
      "email": "jane@example.com",
      "age": 21,
      "course": "Data Science",
      "createdDate": "2025-01-10T10:05:00Z"
    }
  ],
  "statusCode": 200
}
```

**Response (401 Unauthorized):**
```json
{
  "success": false,
  "message": "Unauthorized",
  "statusCode": 401
}
```

---

#### GET /api/students/{id}
Get a specific student by ID.

**Parameters:**
- `id` (path, required): Student ID (integer)

**Example:**
```
GET https://localhost:5001/api/students/1
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Student retrieved successfully",
  "data": {
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com",
    "age": 20,
    "course": "Computer Science",
    "createdDate": "2025-01-10T10:00:00Z"
  },
  "statusCode": 200
}
```

**Response (404 Not Found):**
```json
{
  "success": false,
  "message": "Student with ID 999 not found",
  "statusCode": 404
}
```

**Response (400 Bad Request):**
```json
{
  "success": false,
  "message": "Invalid student ID",
  "statusCode": 400
}
```

---

#### POST /api/students
Create a new student.

**Request Body:**
```json
{
  "name": "Alice Johnson",
  "email": "alice@example.com",
  "age": 22,
  "course": "Machine Learning"
}
```

**Example:**
```http
POST https://localhost:5001/api/students
Content-Type: application/json
Authorization: Bearer {token}

{
  "name": "Alice Johnson",
  "email": "alice@example.com",
  "age": 22,
  "course": "Machine Learning"
}
```

**Response (201 Created):**
```json
{
  "success": true,
  "message": "Student created successfully",
  "data": {
    "id": 3,
    "name": "Alice Johnson",
    "email": "alice@example.com",
    "age": 22,
    "course": "Machine Learning",
    "createdDate": "2025-01-10T12:00:00Z"
  },
  "statusCode": 201
}
```

**Response (400 Bad Request - Email exists):**
```json
{
  "success": false,
  "message": "Email already exists",
  "statusCode": 400
}
```

**Response (400 Bad Request - Invalid data):**
```json
{
  "success": false,
  "message": "Invalid student data. All fields are required and age must be positive.",
  "statusCode": 400
}
```

---

#### PUT /api/students/{id}
Update an existing student.

**Parameters:**
- `id` (path, required): Student ID to update

**Request Body:**
```json
{
  "name": "Updated Name",
  "email": "newemail@example.com",
  "age": 23,
  "course": "Web Development"
}
```

**Example:**
```http
PUT https://localhost:5001/api/students/1
Content-Type: application/json
Authorization: Bearer {token}

{
  "name": "John Updated",
  "email": "johnupdated@example.com",
  "age": 21,
  "course": "Full Stack Development"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Student updated successfully",
  "data": {
    "id": 1,
    "name": "John Updated",
    "email": "johnupdated@example.com",
    "age": 21,
    "course": "Full Stack Development",
    "createdDate": "2025-01-10T10:00:00Z"
  },
  "statusCode": 200
}
```

**Response (404 Not Found):**
```json
{
  "success": false,
  "message": "Student with ID 999 not found",
  "statusCode": 404
}
```

**Response (400 Bad Request):**
```json
{
  "success": false,
  "message": "Invalid student data. All fields are required and age must be positive.",
  "statusCode": 400
}
```

---

#### DELETE /api/students/{id}
Delete a student.

**Parameters:**
- `id` (path, required): Student ID to delete

**Example:**
```http
DELETE https://localhost:5001/api/students/1
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Student deleted successfully",
  "statusCode": 200
}
```

**Response (404 Not Found):**
```json
{
  "success": false,
  "message": "Student with ID 999 not found",
  "statusCode": 404
}
```

---

## Testing with cURL

### Login
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'
```

### Get All Students
```bash
curl -X GET https://localhost:5001/api/students \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### Get Student by ID
```bash
curl -X GET https://localhost:5001/api/students/1 \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### Create Student
```bash
curl -X POST https://localhost:5001/api/students \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -d '{
    "name": "Test User",
    "email": "test@example.com",
    "age": 20,
    "course": "Test Course"
  }'
```

### Update Student
```bash
curl -X PUT https://localhost:5001/api/students/1 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -d '{
    "name": "Updated User",
    "email": "updated@example.com",
    "age": 21,
    "course": "Updated Course"
  }'
```

### Delete Student
```bash
curl -X DELETE https://localhost:5001/api/students/1 \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

---

## HTTP Status Codes

| Code | Status | Meaning |
|------|--------|---------|
| 200 | OK | Request successful |
| 201 | Created | Resource created successfully |
| 400 | Bad Request | Invalid input or validation error |
| 401 | Unauthorized | Missing or invalid authentication token |
| 404 | Not Found | Resource not found |
| 500 | Internal Server Error | Server-side error |

---

## Error Handling

All errors follow a consistent format:

```json
{
  "success": false,
  "message": "Descriptive error message",
  "statusCode": 400
}
```

Common error scenarios:
- **Missing/Invalid Token**: Get a new token via login endpoint
- **Email Exists**: Use a different email when creating/updating
- **Invalid Data**: Ensure all required fields are provided and valid
- **Not Found**: Check if the resource ID exists
- **Unauthorized**: Add valid JWT token in Authorization header

---

## Postman Collection

You can import the following into Postman:

1. Create a new collection named "Student Management System"
2. Add requests with the examples provided above
3. Set environment variables for:
   - `baseUrl`: https://localhost:5001
   - `token`: (paste token from login response)

Use `{{baseUrl}}/api/students` in requests.

---

## Notes

- JWT tokens expire after 60 minutes (configurable in appsettings.json)
- All timestamps are in UTC format
- Email addresses must be unique
- Age must be between 1 and 120
- All string fields have maximum length restrictions
