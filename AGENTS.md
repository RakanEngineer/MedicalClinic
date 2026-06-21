# AGENTS.md

Read these files before making changes:

- docs/architecture.md
- docs/conventions.md

Follow the architecture and coding conventions defined there.

Important rules:

- Keep controllers thin
- Put business logic in services
- Use repositories for data access
- Use DTOs for API communication
- Protect write endpoints with [Authorize]
- Use async/await
- Do not expose EF Core entities directly
- Keep the medical clinic domain explicit
- Use `Patient`, `Doctor`, `Appointment`, and `MedicalRecord` as the core modules

Before finishing work always run:

```powershell
dotnet restore MedicalClinic.ManagementSystem.sln
dotnet build MedicalClinic.ManagementSystem.sln
dotnet test MedicalClinic.ManagementSystem.sln
```
