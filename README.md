# Medical Clinic Management System

Clean Architecture ASP.NET Core API for managing patients, doctors, appointments, and medical records.

## Includes

- Clean Architecture projects and dependency direction
- JWT authentication and role-based authorization
- Swagger/OpenAPI
- FluentValidation
- Serilog request logging
- Health checks
- SQL Server support
- Dockerfile and Docker Compose

## API Modules

- `Patient`
- `Doctor`
- `Appointment`
- `MedicalRecord`

## Local Run

```powershell
dotnet restore MedicalClinic.ManagementSystem.sln
dotnet run --project MedicalClinic.ManagementSystem.Api/MedicalClinic.ManagementSystem.Api.csproj
```

Useful endpoints:

```text
https://localhost:7213/swagger
https://localhost:7213/health/ready
https://localhost:7213/api/v1/patients
https://localhost:7213/api/v1/doctors
https://localhost:7213/api/v1/appointments
https://localhost:7213/api/v1/medical-records
https://localhost:7213/api/v1/auth
```

## Docker

Create a local `.env` file with strong values:

```text
MSSQL_SA_PASSWORD=Your_strong_password_123!
JWT_SECRET_KEY=your-very-long-development-secret-key
```

Then run:

```powershell
docker compose up --build
```

Container endpoints:

```text
http://localhost:8080/swagger
http://localhost:8080/health/ready
http://localhost:8080/api/v1/patients
```
