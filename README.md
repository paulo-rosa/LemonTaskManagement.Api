# Lemon Task Management API - Backend

A production-ready task management system built with .NET 9, implementing Clean Architecture and CQRS patterns with JWT authentication.

## Table of Contents

- [Architecture & Design](#architecture--design)
- [Technology Stack](#technology-stack)
- [Features](#features)
- [Quick Start](#quick-start)
- [API Documentation](#api-documentation)
- [Database Structure](#database-structure)
- [Authentication](#authentication)
- [Trade-offs & Assumptions](#trade-offs--assumptions)
- [Future Improvements & Scalability](#future-improvements--scalability)

---

## Architecture & Design

### Clean Architecture with CQRS

The project follows **Clean Architecture** principles with **CQRS (Command Query Responsibility Segregation)** pattern, ensuring separation of concerns and maintainability.

```
┌─────────────────────────────────────────┐
│         API Layer                       │
│  (Controllers, Auth, Configuration)     │
└──────────────┬──────────────────────────┘
               │
      ┌────────┴────────┐
      │                 │
┌─────▼──────┐    ┌─────▼──────┐
│  Queries   │    │  Commands  │
│  (Read)    │    │  (Write)   │
└─────┬──────┘    └─────┬──────┘
      │                 │
      └────────┬────────┘
               │
      ┌────────▼────────┐
      │  Domain Core    │
      │  (Shared Logic) │
      └────────┬────────┘
               │
      ┌────────▼────────┐
      │  Infrastructure │
      │  Read │ Write   │
      └────────┬────────┘
               │
      ┌────────▼────────┐
      │    Entities     │
      │   (Domain)      │
      └─────────────────┘
```

### Project Structure

```
LemonTaskManagement.Api/
├── LemonTaskManagement.Api/             # Presentation (Controllers, Auth)
├── LemonTaskManagement.Domain.Commands/ # Write operations & validation
├── LemonTaskManagement.Domain.Queries/  # Read operations & DTOs
├── LemonTaskManagement.Domain.Core/     # Shared models & responses
├── LemonTaskManagement.Domain.Entities/ # Domain entities
├── LemonTaskManagement.Infra.Data/      # Shared data infrastructure
├── LemonTaskManagement.Infra.Data.Read/ # Read-optimized repository
└── LemonTaskManagement.Infra.Data.Write/# Write repository & migrations
```

### Key Design Decisions

**1. CQRS Pattern**
- **Why**: Separates read and write concerns, allowing independent optimization
- **Trade-off**: Added complexity vs. better performance and scalability
- **Implementation**: Separate repositories for queries (AsNoTracking) and commands (with change tracking)

**2. Repository Pattern**
- **Why**: Abstracts data access, making it testable and maintainable
- **Trade-off**: Additional abstraction layer vs. flexibility and testability

**3. JWT Authentication**
- **Why**: Stateless, scalable authentication suitable for APIs
- **Trade-off**: Token management complexity vs. scalability and performance

**4. EF Core with In-Memory Database**
- **Why**: Quick setup for development/demo, easy switch to PostgreSQL
- **Trade-off**: In-memory loses data on restart vs. zero infrastructure setup

---

## Technology Stack

### Core Technologies
- **.NET 9.0** - Latest framework with performance improvements
- **Entity Framework Core** - ORM with migration support
- **PostgreSQL** - Production database (In-Memory for development)
- **BCrypt.Net** - Secure password hashing
- **JWT Bearer Authentication** - Stateless authentication
- **Swagger/OpenAPI** - API documentation

### Architecture Patterns
- Clean Architecture
- CQRS (Command Query Responsibility Segregation)
- Repository Pattern
- Dependency Injection

---

## Features

### Production MVP Features

**User Management**
- Secure user authentication with JWT
- BCrypt password hashing
- User profile management

**Board Management**
- Create and manage multiple boards
- Board-level access control
- User-board associations

**Card Management**
- Create cards in board columns
- Update card content and assignments
- Move cards between columns
- Card ordering within columns

**Security & Authorization**
- JWT-based authentication
- Board-level access control
- Protected API endpoints
- Token expiration handling

**Audit Trail**
- Automatic tracking of `CreatedAt`, `CreatedBy`
- `UpdatedAt`, `UpdatedBy` on modifications
- Full audit history for compliance

**API Documentation**
- Interactive Swagger UI
- JWT authentication support in Swagger
- Complete endpoint documentation

---

## Quick Start

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (for local development)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (for Docker Compose)
- (Optional) [PostgreSQL 14+](https://www.postgresql.org/download/) for production

### Running the Application

You can run the application in two ways:

#### Option 1: Using Docker Compose

1. **Ensure Docker Desktop is running**

2. **Start the application:**
```bash
docker-compose up --build
```

3. **Access the API:**
- API Base: `http://localhost:5130`

4. **Stop the application:**
```bash
docker-compose down
```

---

#### Option 2: Using .NET CLI (Local Development)

1. **Clone the repository** (if not already done)
```bash
git clone https://github.com/paulo-rosa/LemonTaskManagement.Api.git
cd LemonTaskManagement.Api
```

2. **Restore dependencies:**
```bash
dotnet restore
```

3. **Run the application:**
```bash
dotnet run --project LemonTaskManagement.Api --launch-profile http
```

Or simply:
```bash
dotnet run --project LemonTaskManagement.Api
```

4. **Access the API:**
- API Base: `http://localhost:5130`

5. **Stop the application:**
- Press `Ctrl+C` in the terminal

**Using Visual Studio:**
1. Open `LemonTaskManagement.Api.sln`
2. Select the **`http`** profile from the debug dropdown
3. Press `F5` or click "Start Debugging"

---

### Testing Authentication

Both running methods use the same configuration. You can test the API using curl or any HTTP client:

**Get Authentication Token:**
```bash
curl -X POST http://localhost:5130/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "Admin123!"
  }'
```

**Use Token in Requests:**
```bash
curl -X GET http://localhost:5130/api/users \
  -H "Authorization: Bearer <your-token-here>"
```

### Default Test Users

| Username | Password | Access Level |
|----------|----------|--------------|
| `admin` | `Admin123!` | All boards |
| `john.doe` | `Password123!` | Development Tasks, Team Collaboration |
| `jane.smith` | `Password123!` | Marketing Campaign, Team Collaboration |

### Database Configuration

**Development (In-Memory)** - Default for both methods
```json
{
  "Database": {
    "UseInMemory": true
  }
}
```
- Data resets on application restart
- No database installation required
- Fast for development and testing

**Production (PostgreSQL)**
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

---

## API Documentation

### Authentication Endpoint
```
POST /api/auth/login
```
**Request:**
```json
{
  "username": "admin",
  "password": "Admin123!"
}
```
**Response:**
```json
{
  "data": {
    "userId": "guid",
    "username": "admin",
    "email": "admin@example.com",
    "token": "eyJhbGc...",
    "expiresAt": "2025-01-11T12:00:00Z"
  }
}
```

### Protected Endpoints (Require `Authorization: Bearer <token>`)

**Users**
- `GET /api/users` - List all users
- `GET /api/users/{id}` - Get user by ID

**Boards**
- `GET /api/users/{userId}/boards` - Get user's boards
- `GET /api/users/{userId}/boards/{boardId}` - Get board details with cards

**Cards**
- `POST /api/users/{userId}/boards/{boardId}/cards` - Create card
- `PUT /api/users/{userId}/boards/{boardId}/cards/{cardId}` - Update card
- `PUT /api/users/{userId}/boards/{boardId}/cards/{cardId}/move` - Move card

---

## Database Structure

### Core Entities

**User**
- Id (Guid)
- Username, Email, PasswordHash
- Audit fields (CreatedAt, CreatedBy, UpdatedAt, UpdatedBy)

**Board**
- Id (Guid), Name, Description
- Audit fields

**BoardUser** (Access Control)
- UserId, BoardId
- Many-to-many relationship

**BoardColumn**
- Id (Guid), BoardId, Name, Order
- Represents columns like "TO-DO", "DOING", "DONE"

**Card**
- Id (Guid), BoardColumnId, Description, Order
- AssignedUserId (nullable)
- Audit fields

### Relationships
- User ↔ Board (Many-to-Many via BoardUser)
- Board → BoardColumn (One-to-Many)
- BoardColumn → Card (One-to-Many)
- Card → User (Many-to-One for assignment)

---

## Authentication

### JWT Token Flow

1. **Login**: User sends credentials to `/api/auth/login`
2. **Validation**: Server validates username/password with BCrypt
3. **Token Generation**: Server generates JWT with claims (user ID, username, email)
4. **Client Storage**: Client stores token (localStorage/sessionStorage)
5. **API Requests**: Client includes token in `Authorization: Bearer <token>` header
6. **Validation**: Server validates token signature and expiration

### Token Configuration
```json
{
  "JwtSettings": {
    "SecretKey": "SecureKeyHere",
    "Issuer": "LemonTaskManagement.Api",
    "Audience": "LemonTaskManagement.Client",
    "ExpirationHours": "24"
  }
}
```

### Security Features
- BCrypt password hashing (cost factor: 11)
- JWT with HS256 algorithm
- Token expiration (24 hours default)
- Board-level access control
- Protected endpoints with `[Authorize]` attribute

---

## Trade-offs & Assumptions

### Trade-offs

**1. In-Memory Database**
- **Pros**: Zero setup, fast development, easy testing
- **Cons**: Data loss on restart, not suitable for production
- **Mitigation**: Easy switch to PostgreSQL via configuration

**2. CQRS Pattern**
- **Pros**: Optimized read/write operations, scalable
- **Cons**: Added complexity, more code
- **Justification**: Demonstrates enterprise-level architecture

**3. JWT Authentication**
- **Pros**: Stateless, scalable, works across distributed systems
- **Cons**: Cannot revoke tokens before expiration, larger payload
- **Mitigation**: Short expiration times, future refresh token implementation

**4. Repository Pattern**
- **Pros**: Testable, abstracts EF Core, maintainable
- **Cons**: Additional abstraction layer
- **Justification**: Better for long-term maintainability

### Assumptions

1. **Single Tenant**: System assumes single organization (no multi-tenancy)
2. **Board Access**: Users must be explicitly added to boards to access them
3. **Card Movement**: Cards can only be moved within the same board
4. **User Roles**: No complex role system (all authenticated users have same permissions)
5. **English Only**: No internationalization/localization
6. **Timezone**: All timestamps in UTC (DateTimeOffset used throughout)

---

## Future Improvements & Scalability

### Short-term Improvements (Next Sprint)

1. **Refresh Tokens**
   - Implement refresh token mechanism
   - Allow token renewal without re-login
   - Add token revocation support

2. **Delete Operations**
   - Soft delete for cards and boards
   - Delete board endpoints
   - Delete card endpoints

3. **Advanced Search & Filtering**
   - Search cards by description
   - Filter by assigned user
   - Filter by date range

4. **Notifications**
   - Real-time notifications (SignalR)
   - Email notifications for card assignments
   - Webhook support for integrations

5. **Card Comments & Attachments**
   - Add comments to cards
   - File attachments
   - Activity log per card

### Medium-term Improvements (2-3 Sprints)

1. **Role-Based Access Control (RBAC)**
   - Board owner, admin, member roles
   - Granular permissions per role
   - Permission validation middleware

2. **Real-time Collaboration**
   - SignalR for live updates
   - Optimistic concurrency handling
   - Conflict resolution

3. **Performance Optimization**
   - Redis caching for frequently accessed data
   - Query optimization with indexes
   - Pagination for large datasets
   - Background jobs for heavy operations

4. **Enhanced Security**
   - Rate limiting on authentication
   - Account lockout after failed attempts
   - Password reset functionality
   - Two-factor authentication (2FA)

### Scalability Considerations

**Horizontal Scaling**
```
Load Balancer
    │
    ├─► API Instance 1 ──┐
    ├─► API Instance 2 ──┼─► PostgreSQL (Primary)
    └─► API Instance 3 ──┘       │
                                 └─► Read Replicas
```

**Implementation Plan:**
1. **Database**: 
   - PostgreSQL with read replicas
   - Connection pooling
   - Write to primary, read from replicas

2. **Caching Layer**:
   - Redis for session management
   - Cache board and user data
   - Invalidation strategy

3. **Message Queue**:
   - RabbitMQ/Azure Service Bus for async operations
   - Email notifications
   - Background processing

4. **Monitoring & Observability**:
   - Application Insights / ELK Stack
   - Performance metrics
   - Error tracking
   - Distributed tracing

### Long-term Vision (6+ Months)

1. **Microservices Architecture**
   - Auth Service (User management, authentication)
   - Board Service (Board operations)
   - Notification Service (Email, push notifications)
   - API Gateway (Rate limiting, routing)

2. **Multi-tenancy**
   - Organization-level isolation
   - Separate databases per tenant
   - Tenant-specific configuration

3. **Advanced Features**
   - Card templates
   - Recurring cards
   - Time tracking
   - Reports and analytics
   - Export functionality (PDF, Excel)
   - Integration with third-party tools (Slack, Teams)

4. **Mobile Apps**
   - Native iOS/Android apps
   - Offline support
   - Push notifications

---

## Notes & Explanation

### Why Clean Architecture?
- **Testability**: Business logic isolated from infrastructure
- **Maintainability**: Clear separation of concerns
- **Flexibility**: Easy to swap implementations (database, authentication)
- **Scalability**: Each layer can be optimized independently

### Why CQRS?
- **Performance**: Separate read/write optimization
- **Scalability**: Read and write databases can scale independently
- **Complexity Management**: Queries don't affect command logic
- **Future-proof**: Easy to add event sourcing later

### Why Repository Pattern?
- **Abstraction**: Hide EF Core implementation details
- **Testing**: Easy to mock data access
- **Flexibility**: Can swap ORM without changing business logic

### Why JWT?
- **Stateless**: No server-side session storage needed
- **Scalable**: Works across multiple API instances
- **Standard**: Industry-standard authentication mechanism
- **Cross-platform**: Works with any client (web, mobile, desktop)

---

## Contributing

This is a demo project for assessment purposes. For production use, consider implementing the future improvements mentioned above.

## License

This project is licensed under the MIT License.

## Author

Paulo Rosa - [GitHub](https://github.com/paulo-rosa)

---

## Related Projects

**Frontend Repository**: [Coming Soon]

This backend is designed to work seamlessly with a React/Vue frontend application using the same authentication and API contracts.