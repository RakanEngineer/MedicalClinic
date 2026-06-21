# Architecture

## Overview

`MedicalClinic.ManagementSystem` is a reusable .NET 9 Clean Architecture Web API template.

The solution keeps framework concerns, API controllers, business logic, data access, contracts, domain models, and shared DTOs separated so future systems can add features without collapsing layer boundaries.

## Solution Structure

```text
src/
|-- MedicalClinic.ManagementSystem.Api
|-- MedicalClinic.ManagementSystem.Presentation
|-- MedicalClinic.ManagementSystem.Services
|-- MedicalClinic.ManagementSystem.Infrastructure
|-- MedicalClinic.ManagementSystem.Domain.Models
|-- MedicalClinic.ManagementSystem.Domain.Contracts
|-- MedicalClinic.ManagementSystem.Service.Contracts
`-- MedicalClinic.ManagementSystem.Shared

tests/
`-- MedicalClinic.ManagementSystem.Tests
```

## Layer Responsibilities

### MedicalClinic.ManagementSystem.Api

Responsible for:

- Application startup
- Dependency Injection
- Middleware configuration
- Authentication and authorization setup
- Swagger/OpenAPI configuration
- Serilog configuration
- Health checks
- CORS configuration
- Docker-facing runtime configuration

This layer should not contain business logic.

### MedicalClinic.ManagementSystem.Presentation

Contains API controllers.

Responsible for:

- Receiving HTTP requests
- Calling services
- Returning HTTP responses

Controllers must stay thin and use `/api/v1` routes for public endpoints.

### MedicalClinic.ManagementSystem.Services

Contains business logic.

Responsible for:

- Business rules
- Validation orchestration
- Repository coordination
- DTO mapping
- Domain exception handling

### MedicalClinic.ManagementSystem.Infrastructure

Contains data access implementation.

Responsible for:

- EF Core DbContext
- Repositories
- Migrations
- Database configurations
- UnitOfWork implementation

Only this layer should directly use EF Core.

### MedicalClinic.ManagementSystem.Domain.Models

Contains domain entities and domain-related models.

Examples:

- Patient
- Doctor
- Appointment
- MedicalRecord
- ApplicationUser
- Domain exceptions

### MedicalClinic.ManagementSystem.Domain.Contracts

Contains repository abstractions.

Examples:

- IPatientRepository
- IDoctorRepository
- IAppointmentRepository
- IMedicalRecordRepository
- IUnitOfWork

### MedicalClinic.ManagementSystem.Service.Contracts

Contains service abstractions.

Examples:

- IServiceManager
- IPatientService
- IDoctorService
- IAppointmentService
- IMedicalRecordService
- IAuthService

### MedicalClinic.ManagementSystem.Shared

Contains DTOs, shared request/response models, validation rules, authorization constants, and other API contract types.

API clients should use DTOs instead of EF entities.

## Sample Module

The system exposes clinic modules for patients, doctors, appointments, and medical records.

Use it as the implementation pattern for new modules:

- Entity in Domain.Models
- Repository contract in Domain.Contracts
- Repository implementation in Infrastructure
- DTOs and validators in Shared
- Service contract in Service.Contracts
- Service implementation in Services
- Thin controller in Presentation

## Request Flow

```text
HTTP Request
    |
    v
Controller
    |
    v
Service
    |
    v
Repository
    |
    v
DbContext
    |
    v
SQL Server
```

## Authentication And Authorization

The application uses:

- ASP.NET Core Identity
- JWT Bearer Authentication
- Refresh Tokens
- Role-based authorization policies

Rules:

- Protected endpoints must use `[Authorize]`
- Write endpoints must use a write/admin policy
- JWT secrets must come from User Secrets or Environment Variables
- Shared policy names live in `MedicalClinic.ManagementSystem.Shared.Authorization`

## Database

The application uses:

- SQL Server
- Entity Framework Core
- EF Core Migrations

Rules:

- Use migrations for schema changes
- Keep EF configurations inside Infrastructure
- Use EnableRetryOnFailure for SQL Server resilience
- Keep the template baseline migration named `InitialCreate`

## API Documentation

Swagger UI:

```text
https://localhost:7213/swagger
```

OpenAPI JSON:

```text
https://localhost:7213/openapi/v1.json
```

## Health Checks

Health checks:

```text
https://localhost:7213/health/live
https://localhost:7213/health/ready
```

`/health/live` is a lightweight liveness probe. `/health/ready` includes database readiness.

## Logging

The application uses Serilog for structured request and application logging.

## Docker

The template includes:

- Dockerfile for `MedicalClinic.ManagementSystem.Api`
- docker-compose.yml with SQL Server
- Environment variables for connection strings and JWT secrets

## CORS

CORS allowed origins are configured with `Cors:AllowedOrigins`. Do not use wildcard origins for production deployments.

## Testing

Before finishing work run:

```powershell
dotnet restore MedicalClinic.ManagementSystem.sln
dotnet build MedicalClinic.ManagementSystem.sln
dotnet test MedicalClinic.ManagementSystem.sln
```
