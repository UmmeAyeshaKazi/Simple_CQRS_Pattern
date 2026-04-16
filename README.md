# MyDemoApp - CQRS Pattern Implementation

[![Release with Semantic Versioning](https://github.com/UmmeAyeshaKazi/Simple_CQRS_Pattern/actions/workflows/release.yml/badge.svg)](https://github.com/UmmeAyeshaKazi/Simple_CQRS_Pattern/actions/workflows/release.yml)
[![GitHub Release](https://img.shields.io/github/v/release/UmmeAyeshaKazi/Simple_CQRS_Pattern)](https://github.com/UmmeAyeshaKazi/Simple_CQRS_Pattern/releases)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A simple CQRS (Command Query Responsibility Segregation) application built with ASP.NET Core 10, Entity Framework Core, and SQLite without using MediatR library.

## Table of Contents
- [Architecture Overview](#architecture-overview)
- [Project Structure](#project-structure)
- [API Endpoints](#api-endpoints)
- [CQRS Flow](#cqrs-flow)
- [Getting Started](#getting-started)
- [Technology Stack](#technology-stack)
- [Releases](#releases)
- [Documentation](#documentation)

## Architecture Overview

This application demonstrates a clean CQRS pattern implementation organized in a single-layer project with separate folders for each logic component. The architecture follows SOLID principles and separates read operations (Queries) from write operations (Commands).

### Complete Request-Response Flow with CQRS & Handlers

```mermaid
graph TB
    subgraph "Client Layer"
        Client["Client/Postman"]
    end

    subgraph "API Layer"
        GET["GET /api/students"]
        GETID["GET /api/students/:id"]
        POST["POST /api/students"]
        PUT["PUT /api/students/:id"]
        DELETE["DELETE /api/students/:id"]
    end

    subgraph "Controller"
        StudentsController["StudentsController<br/>- Receives HTTP Requests<br/>- Injects Handlers"]
    end

    subgraph "Query Layer"
        GetStudentsQuery["GetStudentsQuery<br/>Get All Students"]
        GetStudentByIdQuery["GetStudentByIdQuery<br/>Get Single Student by ID"]
    end

    subgraph "Command Layer"
        CreateCommand["CreateStudentCommand<br/>Name, Email, Age"]
        UpdateCommand["UpdateStudentCommand<br/>Id, Name, Email, Age"]
        DeleteCommand["DeleteStudentCommand<br/>Id"]
    end

    subgraph "Handler Layer"
        QueryHandler1["GetStudentsQueryHandler<br/>IQueryHandler"]
        QueryHandler2["GetStudentByIdQueryHandler<br/>IQueryHandler"]
        CreateHandler["CreateStudentCommandHandler<br/>ICommandHandler"]
        UpdateHandler["UpdateStudentCommandHandler<br/>ICommandHandler"]
        DeleteHandler["DeleteStudentCommandHandler<br/>ICommandHandler"]
    end

    subgraph "Data Access Layer"
        DbContext["AppDbContext<br/>Entity Framework Core"]
    end

    subgraph "Database"
        SQLite["SQLite Database<br/>Data/MyDemoApp.db"]
        StudentsTable["Students Table<br/>Id, Name, Email, Age"]
    end

    Client -->|HTTP Request| GET
    Client -->|HTTP Request| GETID
    Client -->|HTTP Request| POST
    Client -->|HTTP Request| PUT
    Client -->|HTTP Request| DELETE
    
    GET -->|Route| StudentsController
    GETID -->|Route| StudentsController
    POST -->|Route| StudentsController
    PUT -->|Route| StudentsController
    DELETE -->|Route| StudentsController

    StudentsController -->|Create Query| GetStudentsQuery
    StudentsController -->|Create Query| GetStudentByIdQuery
    StudentsController -->|Create Command| CreateCommand
    StudentsController -->|Create Command| UpdateCommand
    StudentsController -->|Create Command| DeleteCommand

    GetStudentsQuery -->|Handle| QueryHandler1
    GetStudentByIdQuery -->|Handle| QueryHandler2
    CreateCommand -->|Handle| CreateHandler
    UpdateCommand -->|Handle| UpdateHandler
    DeleteCommand -->|Handle| DeleteHandler

    QueryHandler1 -->|Query| DbContext
    QueryHandler2 -->|Query| DbContext
    CreateHandler -->|Execute| DbContext
    UpdateHandler -->|Execute| DbContext
    DeleteHandler -->|Execute| DbContext

    DbContext -->|CRUD Operations| SQLite
    SQLite -->|Read/Write| StudentsTable

    StudentsTable -->|Return Data| SQLite
    SQLite -->|Return Response| DbContext
    DbContext -->|Map Results| QueryHandler1
    DbContext -->|Map Results| QueryHandler2
    DbContext -->|Confirm| CreateHandler
    DbContext -->|Confirm| UpdateHandler
    DbContext -->|Confirm| DeleteHandler

    QueryHandler1 -->|Response| StudentsController
    QueryHandler2 -->|Response| StudentsController
    CreateHandler -->|Response| StudentsController
    UpdateHandler -->|Response| StudentsController
    DeleteHandler -->|Response| StudentsController

    StudentsController -->|HTTP Response 200/201/204| Client

    style Client fill:#e1f5ff
    style StudentsController fill:#fff3e0
    style GetStudentsQuery fill:#f3e5f5
    style GetStudentByIdQuery fill:#f3e5f5
    style CreateCommand fill:#e8f5e9
    style UpdateCommand fill:#e8f5e9
    style DeleteCommand fill:#e8f5e9
    style QueryHandler1 fill:#ede7f6
    style QueryHandler2 fill:#ede7f6
    style CreateHandler fill:#c8e6c9
    style UpdateHandler fill:#c8e6c9
    style DeleteHandler fill:#c8e6c9
    style DbContext fill:#b2dfdb
    style SQLite fill:#ffccbc
    style StudentsTable fill:#ffab91
```

### CQRS Pattern Flow Diagram

```mermaid
graph LR
    subgraph "Request Processing"
        Request["HTTP Request"]
        Controller["StudentsController"]
    end

    subgraph "CQRS Pattern"
        Command["📝 COMMANDS<br/>Write Operations<br/>- CreateStudentCommand<br/>- UpdateStudentCommand<br/>- DeleteStudentCommand"]
        Query["📖 QUERIES<br/>Read Operations<br/>- GetStudentsQuery<br/>- GetStudentByIdQuery"]
    end

    subgraph "Handlers"
        CmdHandler["Command Handlers<br/>- Create/Update/DeleteHandler<br/>- Modify Data<br/>- Execute via DbContext"]
        QryHandler["Query Handlers<br/>- GetStudents/GetStudentById<br/>- Fetch Data<br/>- Execute via DbContext"]
    end

    subgraph "Data Access"
        DbCtx["AppDbContext<br/>Entity Framework Core<br/>SQLite Provider"]
    end

    subgraph "Storage"
        Database["SQLite Database<br/>Data/MyDemoApp.db<br/>Students Table"]
    end

    Request -->|POST/PUT/DELETE| Controller
    Request -->|GET| Controller
    Controller -->|Dispatch Command| Command
    Controller -->|Dispatch Query| Query
    Command -->|Execute| CmdHandler
    Query -->|Execute| QryHandler
    CmdHandler -->|SaveChangesAsync| DbCtx
    QryHandler -->|QueryAsync| DbCtx
    DbCtx -->|CRUD Operations| Database

    style Command fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    style Query fill:#f3e5f5,stroke:#9c27b0,stroke-width:2px
    style CmdHandler fill:#c8e6c9,stroke:#4caf50,stroke-width:2px
    style QryHandler fill:#ede7f6,stroke:#9c27b0,stroke-width:2px
    style DbCtx fill:#b2dfdb,stroke:#009688,stroke-width:2px
    style Database fill:#ffccbc,stroke:#ff7043,stroke-width:2px
```

### API Endpoints & CRUD Operations

```mermaid
graph TD
    API["📚 Student Management API<br/>Base URL: /api/students"]
    
    API -->|GET /api/students| ReadAll["🔍 GetAll<br/>Query: GetStudentsQuery<br/>Handler: GetStudentsQueryHandler<br/>Response: 200 OK<br/>List of Students"]
    API -->|GET /:id| ReadOne["🔍 GetById<br/>Query: GetStudentByIdQuery<br/>Handler: GetStudentByIdQueryHandler<br/>Response: 200 OK or 404"]
    API -->|POST /api/students| Create["✏️ Create<br/>Command: CreateStudentCommand<br/>Handler: CreateStudentCommandHandler<br/>Response: 201 Created"]
    API -->|PUT /:id| Update["✏️ Update<br/>Command: UpdateStudentCommand<br/>Handler: UpdateStudentCommandHandler<br/>Response: 204 No Content"]
    API -->|DELETE /:id| Delete["🗑️ Delete<br/>Command: DeleteStudentCommand<br/>Handler: DeleteStudentCommandHandler<br/>Response: 204 No Content"]

    style ReadAll fill:#e3f2fd,stroke:#1976d2,stroke-width:2px
    style ReadOne fill:#e3f2fd,stroke:#1976d2,stroke-width:2px
    style Create fill:#f1f8e9,stroke:#689f38,stroke-width:2px
    style Update fill:#f1f8e9,stroke:#689f38,stroke-width:2px
    style Delete fill:#ffebee,stroke:#c62828,stroke-width:2px
```

### Architecture Layers

```mermaid
graph TB
    subgraph Layers["CQRS Architecture Layers"]
        Layer1["🌐 Presentation Layer<br/>StudentsController<br/>- HTTP Endpoints<br/>- Request/Response Handling<br/>- Dependency Injection"]
        Layer2["📋 CQRS Layer<br/>Commands & Queries<br/>- Commands: Create, Update, Delete<br/>- Queries: GetAll, GetById<br/>- Responsibility Separation"]
        Layer3["⚙️ Handler Layer<br/>Business Logic<br/>- Command Handlers<br/>- Query Handlers<br/>- ICommandHandler & IQueryHandler Interfaces<br/>- Single Responsibility Principle"]
        Layer4["💾 Data Access Layer<br/>Entity Framework Core<br/>- AppDbContext<br/>- DbSet<Student>"]
        Layer5["🗄️ Database Layer<br/>SQLite<br/>- Data/MyDemoApp.db<br/>- Students Table<br/>- Migrations"]
    end

    Layer1 --> Layer2
    Layer2 --> Layer3
    Layer3 --> Layer4
    Layer4 --> Layer5

    style Layer1 fill:#bbdefb,stroke:#1565c0,stroke-width:2px
    style Layer2 fill:#e8f5e9,stroke:#2e7d32,stroke-width:2px
    style Layer3 fill:#fff9c4,stroke:#f57f17,stroke-width:2px
    style Layer4 fill:#b2dfdb,stroke:#00695c,stroke-width:2px
    style Layer5 fill:#ffccbc,stroke:#d84315,stroke-width:2px
```

## Project Structure

```
MyDemoApp.WebAPI/
├── Data/                          # Database context & configuration
│   ├── AppDbContext.cs           # Entity Framework DbContext
│   └── MyDemoApp.db              # SQLite database file
├── Models/                        # Domain models
│   └── Student.cs                # Student entity (Id, Name, Email, Age)
├── Commands/                      # Write operations (CQRS)
│   ├── CreateStudentCommand.cs   # Create new student
│   ├── UpdateStudentCommand.cs   # Update existing student
│   └── DeleteStudentCommand.cs   # Delete student
├── Queries/                       # Read operations (CQRS)
│   ├── GetStudentsQuery.cs       # Get all students
│   └── GetStudentByIdQuery.cs    # Get single student
├── Handlers/                      # Command & Query handlers
│   ├── IHandlers.cs              # Handler interfaces
│   ├── CreateStudentCommandHandler.cs
│   ├── UpdateStudentCommandHandler.cs
│   ├── DeleteStudentCommandHandler.cs
│   ├── GetStudentsQueryHandler.cs
│   └── GetStudentByIdQueryHandler.cs
├── Controllers/                   # API endpoints
│   └── StudentsController.cs     # CRUD operations
├── Migrations/                    # EF Core migrations
│   ├── 20260416104308_InitialCreate.cs
│   └── AppDbContextModelSnapshot.cs
├── Program.cs                     # DI configuration
├── appsettings.json              # Configuration
└── MyDemoApp.WebAPI.csproj       # Project file
```

## API Endpoints

### Get All Students
```http
GET /api/students
```
**Response:** 200 OK
```json
[
  {
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com",
    "age": 20
  }
]
```

### Get Student by ID
```http
GET /api/students/{id}
```
**Response:** 200 OK or 404 Not Found

### Create Student
```http
POST /api/students
Content-Type: application/json

{
  "name": "Jane Smith",
  "email": "jane@example.com",
  "age": 21
}
```
**Response:** 201 Created

### Update Student
```http
PUT /api/students/{id}
Content-Type: application/json

{
  "name": "Jane Smith Updated",
  "email": "jane.updated@example.com",
  "age": 22
}
```
**Response:** 204 No Content

### Delete Student
```http
DELETE /api/students/{id}
```
**Response:** 204 No Content

## Getting Started

### Prerequisites
- .NET 10 SDK
- SQLite (included with EF Core)

### Installation

1. **Clone the repository**
```bash
git clone <repository-url>
cd MyDemoApp.WebAPI
```

2. **Restore NuGet packages**
```bash
dotnet restore
```

3. **Create the database**
```bash
dotnet ef database update
```

4. **Run the application**
```bash
dotnet run
```

The API will be available at `https://localhost:5001` or `http://localhost:5000`

### API Documentation
Visit the Swagger UI: `https://localhost:5001/openapi`
Or visit the Scalar API reference: `https://localhost:5001/scalar`

## Technology Stack

- **Framework**: ASP.NET Core 10
- **Database**: SQLite
- **ORM**: Entity Framework Core 10
- **Pattern**: CQRS (Command Query Responsibility Segregation)
- **Architecture**: Single-layer with organized folders
- **Principles**: SOLID

## CQRS Pattern Explanation

### Commands (Write Operations)
Commands are objects that represent requests to perform actions that modify state. They are dispatched to command handlers that execute the business logic and persist changes to the database.

**Examples:**
- `CreateStudentCommand` → Creates a new student
- `UpdateStudentCommand` → Modifies existing student
- `DeleteStudentCommand` → Removes a student

### Queries (Read Operations)
Queries are objects that represent requests to fetch data. They are dispatched to query handlers that retrieve information from the database without modifying state.

**Examples:**
- `GetStudentsQuery` → Retrieves all students
- `GetStudentByIdQuery` → Retrieves a specific student

### Handlers
Handlers contain the business logic for processing commands and queries. They implement `ICommandHandler<T>` or `IQueryHandler<TQuery, TResult>` interfaces.

## Benefits of This Approach

✅ **Separation of Concerns**: Commands and Queries are clearly separated  
✅ **SOLID Principles**: Each handler has a single responsibility  
✅ **Scalability**: Easy to extend with new commands/queries  
✅ **Testability**: Handlers can be unit tested independently  
✅ **Maintainability**: Clear structure makes code easier to understand  
✅ **No External Dependencies**: Custom implementation without MediatR  

## Database Migrations

### Create a new migration
```bash
dotnet ef migrations add MigrationName
```

### Apply migrations
```bash
dotnet ef database update
```

### Remove last migration
```bash
dotnet ef migrations remove
```

## Releases & Versioning

This project uses **Semantic Versioning** with **Conventional Commits** and GitHub Actions for automated releases.

### How Releases Work

Releases are automatically created when code is pushed to the `main` branch using the following commit message format:

#### Commit Message Format (Conventional Commits)

**Patch Release** (v0.0.X → v0.0.X+1):
```bash
git commit -m "fix: correct validation logic"
```

**Minor Release** (v0.X.0 → v0.X+1.0):
```bash
git commit -m "feat: add search functionality"
```

**Major Release** (vX.0.0 → vX+1.0.0):
```bash
git commit -m "feat!: redesign API response format"
# or
git commit -m "refactor: restructure models

BREAKING CHANGE: API response structure changed"
```

### Workflow

1. Push code to `main` branch with conventional commit message
2. GitHub Actions automatically runs
3. Determines next semantic version
4. Creates Git tag and GitHub Release
5. Publishes application artifacts

### View Releases

- **GitHub Releases**: https://github.com/YOUR_USERNAME/MyDemoApp/releases
- **GitHub Actions**: https://github.com/YOUR_USERNAME/MyDemoApp/actions
- **Release Details**: Each release includes changelog, artifacts, and version info

### For More Information

See [Documentation/CICD_GITHUB_ACTIONS.md](./Documentation/CICD_GITHUB_ACTIONS.md) for detailed documentation on:
- Version bumping rules
- Commit message conventions
- How to trigger different release types
- Troubleshooting release workflows

## Documentation

All detailed documentation is located in the [Documentation/](./Documentation/) folder:

- **[CICD_GITHUB_ACTIONS.md](./Documentation/CICD_GITHUB_ACTIONS.md)** - Complete guide for GitHub Actions, semantic versioning, and release workflow

## License

This project is open source and available under the MIT License.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

When contributing, please follow the [Conventional Commits](https://www.conventionalcommits.org/) specification for commit messages to ensure proper semantic versioning.

---

**Created**: April 16, 2026  
**Framework**: ASP.NET Core 10  
**Pattern**: CQRS without MediatR  
**CI/CD**: GitHub Actions with Semantic Versioning
