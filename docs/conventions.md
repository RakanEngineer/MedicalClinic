# Coding Conventions

## General Rules

- Use Clean Architecture boundaries
- Keep controllers thin
- Put business logic in services
- Put database access in repositories
- Use DTOs for all API requests and responses
- Use async/await for I/O operations
- Prefer dependency injection over manual object creation

## Naming

Use clear and consistent names.

Examples:

- PatientDto
- PatientCreateDto
- PatientUpdateDto
- IPatientService
- PatientService
- IPatientRepository
- PatientRepository

When adapting this template for a real system, rename the solution and projects consistently before adding feature work.

## Controllers

Controllers should:

- Receive HTTP requests
- Validate incoming models
- Call services
- Return HTTP responses
- Use `/api/v1` route prefixes for public endpoints

Controllers should not:

- Access DbContext directly
- Contain business logic
- Execute database queries
- Return EF entities directly

## Services

Services contain business logic.

Services may:

- Apply business rules
- Validate entities
- Call repositories
- Map entities to DTOs
- Throw meaningful exceptions

Services should not:

- Use IActionResult
- Access HttpContext directly
- Return HTTP-specific responses

## Repositories

Repositories handle database access only.

Repositories should:

- Use ApplicationDbContext
- Handle EF Core queries
- Return entities
- Support async operations

Repositories should not:

- Contain business logic
- Return DTOs
- Access controllers

## DTOs

Rules:

- Use DTOs for API input/output
- Never expose internal entities directly
- Never expose password hashes or refresh tokens
- Keep DTOs inside MedicalClinic.ManagementSystem.Shared

## Entity Framework Core

Rules:

- Use migrations for schema changes
- Keep configurations inside Infrastructure
- Keep EF Core query execution helpers inside Infrastructure or Services, not Shared
- Use AsNoTracking for read-only queries
- Use EnableRetryOnFailure for SQL Server
- Save changes through UnitOfWork

## Authentication And Authorization

Rules:

- Use JWT authentication
- Protect write endpoints with `[Authorize]`
- Use roles/policies for admin operations
- Keep reusable policy and role names in `MedicalClinic.ManagementSystem.Shared.Authorization`
- Never hardcode secrets
- Store secrets in User Secrets or Environment Variables

## Validation

Rules:

- Validate DTOs before business logic
- Prefer FluentValidation for request DTO validation
- Keep reusable validators close to DTO contracts
- Return meaningful validation messages

## Logging

Rules:

- Use Serilog for structured logs
- Prefer message templates over interpolated strings
- Do not log secrets, passwords, refresh tokens, or access tokens

## Error Handling

Rules:

- Use global exception handling
- Throw meaningful exceptions
- Return proper HTTP status codes

## Docker

Rules:

- Keep Docker configuration outside application layers
- Provide secrets through environment variables
- Do not commit real passwords, API keys, or production connection strings

## CORS

Rules:

- Configure allowed origins through `Cors:AllowedOrigins`
- Do not use `AllowAnyOrigin` in production code
- Expose only headers required by clients, such as `X-Pagination`

## Testing

Before finishing work run:

```powershell
dotnet restore MedicalClinic.ManagementSystem.sln
dotnet build MedicalClinic.ManagementSystem.sln
dotnet test MedicalClinic.ManagementSystem.sln
```

Tests should cover:

- Controllers
- Services
- Authentication
- Authorization
- Validation
- Error scenarios
