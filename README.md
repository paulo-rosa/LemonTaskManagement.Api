# Lemon Task Management API

A task management system built with .NET 9 following Clean Architecture principles, implementing Command Query Responsibility Segregation (CQRS) pattern with separate read and write data contexts.

## Table of Contents

- [Architecture Overview](#architecture-overview)
- [Project Structure](#project-structure)
- [Layer Descriptions](#layer-descriptions)
- [Technology Stack](#technology-stack)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Running with Docker](#running-with-docker)
- [Running Locally Without Docker](#running-locally-without-docker)
- [Database Configuration](#database-configuration)
- [Database Migrations](#database-migrations)
- [Creating Migrations](#creating-migrations)
- [Applying Migrations](#applying-migrations)
- [Removing Migrations](#removing-migrations)
- [Project Configuration](#project-configuration)
- [Development Guidelines](#development-guidelines)

## Architecture Overview

This project implements a Clean Architecture approach with CQRS (Command Query Responsibility Segregation) pattern. The architecture ensures:

- **Separation of Concerns**: Each layer has a distinct responsibility
- **Dependency Inversion**: Dependencies point inward toward the domain layer
- **Testability**: Business logic is decoupled from infrastructure concerns
- **Maintainability**: Clear boundaries between layers facilitate easier maintenance
- **Scalability**: CQRS pattern allows independent scaling of read and write operations

### Architecture Diagram

```
┌────────────────────────────────────────────┐
│          Presentation Layer                │
│          LemonTaskManagement.Api           │
│     (Controllers, Configurations, DTOs)    │
└──────────────────────┬─────────────────────┘
                       │
        ┌──────────────▼──────────────┐
        │                             │
┌───────▼──────────┐         ┌────────▼─────────┐
│  Domain.Queries  │         │ Domain.Commands  │
│  (Query Handlers)│         │(Command Handlers)│
└───────┬──────────┘         └────────┬─────────┘
        │                             │
        └──────────────┬──────────────┘
                       │
┌──────────────────────▼─────────────────────┐
│                Domain Core                 │
│        (Shared Models, Response Types)     │
└────────────────────────────────────────────┘
                       │
        ┌──────────────▼──────────────┐
        │                             │
┌───────▼──────────┐         ┌────────▼─────────┐
│ Infra.Data.Read  │         │Infra.Data.Write  │
│(Read-Only DbCtx) │         │(Write DbContext) │
└───────┬──────────┘         └────────┬─────────┘
        │                             │
        └──────────────┬──────────────┘
                       │
┌──────────────────────▼──────────────────────────┐
│             Infra.Data (Shared)                 │
│     (Base Contexts, Configurations, Interfaces) │
└──────────────────────┬──────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────┐
│             Domain.Entities                     │
│        (User, Board, BoardUser, etc.)           │
└─────────────────────────────────────────────────┘
```

## Project Structure

```
LemonTaskManagement.Api/
├── LemonTaskManagement.Api/                 # API Layer
├── LemonTaskManagement.Domain.Entities/     # Domain Entities
├── LemonTaskManagement.Domain.Core/         # Domain Core Models
├── LemonTaskManagement.Domain.Queries/      # Query Layer (Read)
├── LemonTaskManagement.Domain.Commands/     # Command Layer (Write)
├── LemonTaskManagement.Infra.Data/          # Shared Infrastructure
├── LemonTaskManagement.Infra.Data.Read/     # Read Infrastructure
├── LemonTaskManagement.Infra.Data.Write/    # Write Infrastructure
├── docker-compose.yml                       # Docker Compose Configuration
└── LemonTaskManagement.Api.slnx             # Solution File
```

## Layer Descriptions

### 1. LemonTaskManagement.Api
**Purpose**: Presentation layer that handles HTTP requests and responses.

**Responsibilities**:
- Exposes RESTful API endpoints through controllers
- Handles request validation and model binding
- Manages dependency injection configuration
- Configures middleware pipeline
- Implements API-specific cross-cutting concerns (logging, exception handling)

**Key Components**:
- Controllers: Handle HTTP requests and coordinate with domain services
- Configurations: Database setup, dependency injection, and application settings
- Models: API-specific DTOs and response models

### 2. LemonTaskManagement.Domain.Entities
**Purpose**: Core domain models representing business entities.

**Responsibilities**:
- Defines business entities (User, Board, BoardUser, etc.)
- Contains entity properties and relationships
- Provides base entity functionality (EntityBase with audit fields)

**Key Characteristics**:
- Technology agnostic (.NET Standard 2.1)
- No dependencies on infrastructure or application layers
- Pure domain objects with business-relevant properties

### 3. LemonTaskManagement.Domain.Core
**Purpose**: Shared domain models and common abstractions.

**Responsibilities**:
- Provides response wrappers and result types
- Contains error handling models
- Defines shared domain interfaces and base types

**Key Components**:
- Response<T>: Generic response wrapper for API operations
- Error: Standard error representation

### 4. LemonTaskManagement.Domain.Queries
**Purpose**: Implements the Query side of CQRS pattern.

**Responsibilities**:
- Defines query models and query handlers
- Implements query-specific business logic
- Contains DTOs for read operations
- Defines interfaces for query repositories

**Key Components**:
- Query Services: Process queries and transform data into DTOs
- Query Models: Represent query requests (GetUserQuery, GetUsersQuery)
- DTOs: Data Transfer Objects for read operations
- Repository Interfaces: Contracts for data retrieval

**Design Principle**: Optimized for read performance with no write operations.

### 5. LemonTaskManagement.Domain.Commands
**Purpose**: Implements the Command side of CQRS pattern.

**Responsibilities**:
- Defines command models and command handlers
- Implements write-specific business logic and validation
- Contains command-specific DTOs
- Defines interfaces for command repositories

**Design Principle**: Focused on write operations and business rule enforcement.

### 6. LemonTaskManagement.Infra.Data
**Purpose**: Shared infrastructure components for data access.

**Responsibilities**:
- Provides base DbContext implementation
- Defines entity configurations (Fluent API mappings)
- Contains shared database interfaces
- Implements repository base classes

**Key Components**:
- ILemonTaskManagementDbContext: Shared database context interface
- LemonTaskManagementBaseDbContext: Base context with common configuration
- Entity Configurations: EF Core entity mappings for User, Board, BoardUser

### 7. LemonTaskManagement.Infra.Data.Read
**Purpose**: Read-optimized data access infrastructure.

**Responsibilities**:
- Implements read-only DbContext
- Contains query repository implementations
- Optimizes queries for read performance

**Key Components**:
- LemonTaskManagementReadOnlyDbContext: Read-only database context
- Query Repositories: Implement query interfaces with AsNoTracking()

**Optimization**: Uses EF Core's AsNoTracking() for better read performance.

### 8. LemonTaskManagement.Infra.Data.Write
**Purpose**: Write-optimized data access infrastructure.

**Responsibilities**:
- Implements write DbContext with change tracking
- Handles database migrations
- Manages data seeding
- Implements audit field tracking (CreatedAt, CreatedBy, UpdatedAt, UpdatedBy)

**Key Components**:
- LemonTaskManagementDbContext: Full-featured write context
- Seeder: Database initialization and seed data
- Migration Management: Handles database schema evolution

## Technology Stack

- **Framework**: .NET 9.0
- **Database**: PostgreSQL (with in-memory option for development)
- **ORM**: Entity Framework Core
- **Containerization**: Docker & Docker Compose
- **API Documentation**: OpenAPI (Swagger)
- **Architecture Patterns**: 
  - Clean Architecture
  - CQRS (Command Query Responsibility Segregation)
  - Repository Pattern
  - Dependency Injection

## Prerequisites

### For Running Locally
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL 14+](https://www.postgresql.org/download/) (if not using in-memory database)

### For Running with Docker
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Docker Compose](https://docs.docker.com/compose/install/) (included with Docker Desktop)

## Getting Started

### Running with Docker

1. **Clone the repository**
   ```bash
   git clone https://github.com/paulo-rosa/LemonTaskManagement.Api.git
   cd LemonTaskManagement.Api
   ```

2. **Build and run with Docker Compose**
   ```bash
   docker-compose up --build
   ```

3. **Access the API**
   - API Base URL: `http://localhost:5130`
   - Swagger UI (Development): `http://localhost:5130/openapi/v1.json`

4. **Stop the application**
   ```bash
   docker-compose down
   ```

### Running Locally Without Docker

1. **Clone the repository**
   ```bash
   git clone https://github.com/paulo-rosa/LemonTaskManagement.Api.git
   cd LemonTaskManagement.Api
   ```

2. **Configure the database connection**
   
   Edit `LemonTaskManagement.Api/appsettings.json`:
   
   For PostgreSQL:
   ```json
   {
     "Database": {
       "UseInMemory": false
     },
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=LemonTaskManagement;Username=postgres;Password=yourpassword"
     }
   }
   ```
   
   For In-Memory Database (Development Only):
   ```json
   {
     "Database": {
       "UseInMemory": true
     }
   }
   ```

3. **Restore dependencies**
   ```bash
   dotnet restore
   ```

4. **Apply database migrations** (Skip if using in-memory database)
   ```bash
   cd LemonTaskManagement.Api
   dotnet ef database update --project ../LemonTaskManagement.Infra.Data.Write
   ```

5. **Run the application**
   ```bash
   dotnet run --project LemonTaskManagement.Api
   ```

6. **Access the API**
   - API Base URL: `https://localhost:5131` or `http://localhost:5130`
   - Swagger UI (Development): `https://localhost:5131/openapi/v1.json`

## Database Configuration

The application supports two database modes:

### In-Memory Database
Ideal for development and testing. No external database required.

Configuration in `appsettings.json`:
```json
{
  "Database": {
    "UseInMemory": true
  }
}
```

### PostgreSQL Database
Required for production environments.

Configuration in `appsettings.json`:
```json
{
  "Database": {
    "UseInMemory": false
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=LemonTaskManagement;Username=postgres;Password=yourpassword"
  }
}
```

## Database Migrations

Entity Framework Core migrations are managed in the **LemonTaskManagement.Infra.Data.Write** project, as this is the write-focused infrastructure layer responsible for database schema changes.

### Creating Migrations

To create a new migration after modifying entities or configurations:

```bash
# Navigate to the API project directory
cd LemonTaskManagement.Api

# Create a new migration
dotnet ef migrations add <MigrationName> --project ../LemonTaskManagement.Infra.Data.Write --startup-project .

# Example:
dotnet ef migrations add AddTaskEntity --project ../LemonTaskManagement.Infra.Data.Write --startup-project .
```

**Important Notes**:
- Migration files will be created in `LemonTaskManagement.Infra.Data.Write/Migrations/`
- Use descriptive migration names (e.g., AddTaskTable, UpdateUserEmailIndex)
- Always review the generated migration code before applying

### Applying Migrations

#### Manually Apply Migrations
```bash
# Navigate to the API project directory
cd LemonTaskManagement.Api

# Update the database to the latest migration
dotnet ef database update --project ../LemonTaskManagement.Infra.Data.Write --startup-project .

# Update to a specific migration
dotnet ef database update <MigrationName> --project ../LemonTaskManagement.Infra.Data.Write --startup-project .
```

#### Automatic Migration on Startup
The application automatically applies pending migrations on startup when using PostgreSQL:

```csharp
// DatabaseConfiguration.cs
if (!useInMemoryDatabase)
{
    context.Database.Migrate(); // Automatically applies pending migrations
}
```

### Removing Migrations

To remove the last unapplied migration:

```bash
cd LemonTaskManagement.Api
dotnet ef migrations remove --project ../LemonTaskManagement.Infra.Data.Write --startup-project .
```

**Warning**: Only remove migrations that have not been applied to any database.

### Viewing Migration History

```bash
cd LemonTaskManagement.Api

# List all migrations
dotnet ef migrations list --project ../LemonTaskManagement.Infra.Data.Write --startup-project .

# View SQL for a specific migration
dotnet ef migrations script --project ../LemonTaskManagement.Infra.Data.Write --startup-project .
```

### Migration Best Practices

1. **Always review generated migrations** before applying them
2. **Test migrations** in a development environment first
3. **Backup your database** before applying migrations in production
4. **Use descriptive names** that indicate what the migration does
5. **Keep migrations small** and focused on a single change
6. **Never modify** migrations that have been applied to production
7. **Version control** all migration files

## Project Configuration

### Environment Variables

The application can be configured using environment variables:

```bash
# Database Configuration
Database__UseInMemory=false
ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=LemonTaskManagement;Username=postgres;Password=yourpassword"

# ASP.NET Core Configuration
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_HTTP_PORTS=5130
ASPNETCORE_HTTPS_PORTS=5131
```

### User Secrets (Development)

For local development, use user secrets to store sensitive information:

```bash
cd LemonTaskManagement.Api

# Set connection string
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=LemonTaskManagement;Username=postgres;Password=yourpassword"
```

## Development Guidelines

### Adding New Entities

1. Create the entity in `LemonTaskManagement.Domain.Entities`
2. Add entity configuration in `LemonTaskManagement.Infra.Data/Configurations`
3. Update `ILemonTaskManagementDbContext` interface
4. Update `LemonTaskManagementBaseDbContext` with DbSet
5. Create and apply migration

### Implementing Queries (Read Operations)

1. Create query model in `LemonTaskManagement.Domain.Queries/Queries`
2. Create DTO in `LemonTaskManagement.Domain.Queries/DTOs`
3. Define repository interface in `LemonTaskManagement.Domain.Queries/Interfaces/Repositories`
4. Implement repository in `LemonTaskManagement.Infra.Data.Read`
5. Create query service in `LemonTaskManagement.Domain.Queries/QueryServices`
6. Register dependencies in `InjectorConfiguration`
7. Create controller endpoint in `LemonTaskManagement.Api/Controllers`

### Implementing Commands (Write Operations)

1. Create command model in `LemonTaskManagement.Domain.Commands/Commands`
2. Create command DTO if needed
3. Define repository interface in `LemonTaskManagement.Domain.Commands/Interfaces`
4. Implement repository using write context
5. Create command handler
6. Register dependencies in `InjectorConfiguration`
7. Create controller endpoint

### Code Organization Principles

- **Queries**: Should never modify data; use AsNoTracking() for performance
- **Commands**: Should handle all business validation and rules
- **DTOs**: Transform domain entities to API-friendly formats
- **Entities**: Keep them clean and focused on business logic
- **Configurations**: Use Fluent API for all entity configurations

## License

This project is licensed under the MIT License.

## Support

For issues, questions, or contributions, please visit the [GitHub repository](https://github.com/paulo-rosa/LemonTaskManagement.Api).