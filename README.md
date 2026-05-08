# Student Management System - Zest India Technical Assignment

## Project Overview

This project is developed as part of the technical assessment for **Zest India IT Pvt. Ltd.**

The application is a secure and scalable **ASP.NET Core Web API** built using a clean layered architecture. It demonstrates practical implementation of:

* ASP.NET Core Web API
* SQL Server Integration
* JWT Authentication
* Entity Framework Core
* Swagger Documentation
* Serilog Logging
* Global Exception Handling
* Repository-Service Pattern

The API performs complete CRUD operations for managing student records.

---

# Assignment Requirements Covered

| Requirement                          | Status    |
| ------------------------------------ | --------- |
| Get All Students API                 | Completed |
| Add Student API                      | Completed |
| Update Student API                   | Completed |
| Delete Student API                   | Completed |
| JWT Authentication                   | Completed |
| Swagger API Documentation            | Completed |
| SQL Server Integration               | Completed |
| Global Exception Handling Middleware | Completed |
| Serilog Logging                      | Completed |
| Layered Architecture                 | Completed |
| Clean Structured Code                | Completed |
| Secure Endpoints                     | Completed |
| README Documentation                 | Completed |

---

# Technology Stack

| Technology            | Version            |
| --------------------- | ------------------ |
| .NET SDK              | 8.0.26             |
| ASP.NET Core Web API  | .NET 8             |
| Entity Framework Core | 8.0.8              |
| SQL Server            | SQL Server Express |
| Authentication        | JWT Bearer Token   |
| API Documentation     | Swagger            |
| Logging               | Serilog            |
| IDE                   | Visual Studio 2026 |
| Version Control       | Git & GitHub       |

---

# Project Architecture

The project follows a clean **Layered Architecture** for maintainability and scalability.

```text
Controller Layer
↓
Service Layer
↓
Repository Layer
↓
Database Layer (SQL Server)
```

## Folder Structure

```text
StudentManagementSystem-ZestIndia
│
├── Controllers
├── Services
│   ├── Interfaces
│   └── Implementations
├── Repositories
│   ├── Interfaces
│   └── Implementations
├── Models
├── DTOs
├── Middleware
├── Helpers
├── Data
├── Logs
├── Migrations
├── appsettings.json
└── Program.cs
```

---

# Features Implemented

## Student CRUD APIs

* Get all students
* Get student by ID
* Add new student
* Update existing student
* Delete student

---

## JWT Authentication

Secure authentication implemented using JWT Bearer Tokens.

### Features

* Token generation
* Token validation
* Secure endpoints using `[Authorize]`
* Swagger JWT integration

---

## Global Exception Handling Middleware

Custom middleware implemented for centralized exception handling.

### Benefits

* Consistent API error responses
* Cleaner controller logic
* Improved debugging
* Better API maintainability

---

## Logging with Serilog

Serilog configured for:

* Console logging
* File logging
* Error tracking
* Request monitoring

### Log Location

```text
Logs/log-yyyyMMdd.txt
```

---

## Swagger API Documentation

Swagger UI integrated for:

* API testing
* Endpoint documentation
* JWT authorization testing

### Swagger URL

```text
https://localhost:xxxx/swagger
```

---

# Database Design

## Student Table

| Column      | Data Type |
| ----------- | --------- |
| Id          | int       |
| Name        | nvarchar  |
| Email       | nvarchar  |
| Age         | int       |
| Course      | nvarchar  |
| CreatedDate | datetime  |

---

# API Endpoints

## Authentication

| Method | Endpoint        | Description        |
| ------ | --------------- | ------------------ |
| POST   | /api/auth/login | Generate JWT Token |

---

## Student APIs

| Method | Endpoint           | Description       |
| ------ | ------------------ | ----------------- |
| GET    | /api/students      | Get All Students  |
| GET    | /api/students/{id} | Get Student By Id |
| POST   | /api/students      | Add Student       |
| PUT    | /api/students/{id} | Update Student    |
| DELETE | /api/students/{id} | Delete Student    |

---

# Setup Instructions

## Step 1 — Clone Repository

```bash
git clone <repository-url>
```

---

## Step 2 — Open Project

Open solution in:

* Visual Studio 2022

---

## Step 3 — Configure Database

Update connection string inside:

```json
appsettings.json
```

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\SQLEXPRESS01;Database=StudentDBZestIndia;Trusted_Connection=True;TrustServerCertificate=True"
}
```

---

## Step 4 — Apply Migrations

Open Package Manager Console:

```powershell
Add-Migration InitialCreate
Update-Database
```

---

## Step 5 — Run Application

Run using:

* HTTPS profile

Application launches Swagger automatically.

---

# JWT Authentication Usage

## Step 1

Run Login API.

## Step 2

Copy generated JWT token.

## Step 3

Click:

```text
Authorize
```

inside Swagger.

## Step 4

Enter token:

```text
Bearer your_token_here
```

Now secured APIs can be accessed.

---

# Technical Challenges Faced

## 1. .NET 10 Compatibility Issues

### Problem

Initial development started using .NET 10 preview version which caused package conflicts.

### Solution

* Installed stable .NET 8 SDK
* Changed target framework to .NET 8
* Reconfigured Swagger packages

---

## 2. OpenAPI / Swagger Errors

### Problem

Swagger dependencies caused namespace and startup errors.

### Solution

* Removed incompatible OpenAPI packages
* Installed compatible Swashbuckle version
* Rebuilt solution cleanly

---

## 3. Runtime Version Mismatch

### Problem

Application failed to start because required runtime version was missing.

### Solution

Installed:

* .NET Runtime 8.0.26
* ASP.NET Core Runtime 8.0.26

---

## 4. JWT Integration

### Problem

Configuring JWT validation and Swagger authentication correctly.

### Solution

Implemented:

* JWT Bearer Authentication
* Secure token validation
* Swagger token support

---

# Security Features

* JWT Token Authentication
* Secure API Authorization
* Centralized Exception Handling
* SQL Injection Prevention via EF Core
* HTTPS Enabled

---

# Code Quality Practices

* Clean folder structure
* Dependency Injection
* Repository-Service Pattern
* Separation of Concerns
* Reusable components
* Async programming practices

---

# Future Improvements

* Unit Testing with xUnit
* Docker Support
* React Frontend UI
* Role-Based Authentication
* Pagination & Filtering
* Deployment Pipeline

---

# Suggested Screenshots for Review

* Swagger Home Page
* JWT Authorization
* CRUD API Testing
* SQL Server Database
* Serilog Log File

---

# GitHub Submission

The complete source code is uploaded to GitHub as required in the assignment.

Repository includes:

* Full source code
* README documentation
* Entity Framework migrations
* API implementation
* Logging setup
* Authentication setup

---

# Author

**Harshal Khadatare**

ASP.NET Core Full Stack Developer Candidate

---

# Conclusion

This project demonstrates practical implementation of modern ASP.NET Core Web API development practices including security, architecture, logging, exception handling, database integration, and clean coding standards.

The assignment was completed with focus on:

* Scalability
* Maintainability
* Security
* Professional project structure
* Production-style API development
