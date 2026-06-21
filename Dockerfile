FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["MedicalClinic.ManagementSystem.sln", "./"]
COPY ["MedicalClinic.ManagementSystem.Api/MedicalClinic.ManagementSystem.Api.csproj", "MedicalClinic.ManagementSystem.Api/"]
COPY ["MedicalClinic.ManagementSystem.Infrastructure/MedicalClinic.ManagementSystem.Infrastructure.csproj", "MedicalClinic.ManagementSystem.Infrastructure/"]
COPY ["MedicalClinic.ManagementSystem.Presentation/MedicalClinic.ManagementSystem.Presentation.csproj", "MedicalClinic.ManagementSystem.Presentation/"]
COPY ["MedicalClinic.ManagementSystem.Services/MedicalClinic.ManagementSystem.Services.csproj", "MedicalClinic.ManagementSystem.Services/"]
COPY ["MedicalClinic.ManagementSystem.Shared/MedicalClinic.ManagementSystem.Shared.csproj", "MedicalClinic.ManagementSystem.Shared/"]
COPY ["MedicalClinic.ManagementSystem.Domain.Contracts/MedicalClinic.ManagementSystem.Domain.Contracts.csproj", "MedicalClinic.ManagementSystem.Domain.Contracts/"]
COPY ["MedicalClinic.ManagementSystem.Domain.Models/MedicalClinic.ManagementSystem.Domain.Models.csproj", "MedicalClinic.ManagementSystem.Domain.Models/"]
COPY ["MedicalClinic.ManagementSystem.Service.Contracts/MedicalClinic.ManagementSystem.Service.Contracts.csproj", "MedicalClinic.ManagementSystem.Service.Contracts/"]
COPY ["MedicalClinic.ManagementSystem.Tests/MedicalClinic.ManagementSystem.Tests.csproj", "MedicalClinic.ManagementSystem.Tests/"]

RUN dotnet restore "MedicalClinic.ManagementSystem.sln"

COPY . .
RUN dotnet publish "MedicalClinic.ManagementSystem.Api/MedicalClinic.ManagementSystem.Api.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MedicalClinic.ManagementSystem.Api.dll"]
