# Consultants Salary API

A Clean Architecture ASP.NET Core solution for managing consultants, roles, tasks, time entries, and calculating salaries with historical role rate tracking.

## Tech Stack
- ASP.NET Core Web API
- Entity Framework Core (SQL Server)
- Clean Architecture (Domain, Application, Infrastructure, API)
- JWT Authentication (planned)
- Swagger/OpenAPI (planned)

## Features (Planned)
- Create/Edit Consultants and assign Roles
- Upload Consultant Profile Image (dev: filesystem)
- Create/Edit Roles with Rate History (immutable past entries)
- Create Tasks and assign multiple Consultants
- Capture Time Entries per Task with 12h/day validation across tasks
- Calculate total due for a consultant over a timeframe using the rate effective at the time of work

## Getting Started (Dev)
1. Prerequisites: .NET SDK 9+, SQL Server (LocalDB or instance), Git
2. Configure connection string in `ConsultantsSalary.Api/appsettings.json`
3. Restore and build:
   ```bash
   dotnet restore
   dotnet build
   ```
4. Run API:
   ```bash
   dotnet run --project ConsultantsSalary.Api
   ```
5. Swagger UI will be available once configured (planned in later commit).

## Project Structure
- ConsultantsSalary.Domain: Entities and core rules
- ConsultantsSalary.Application: DTOs, interfaces, and services (use cases)
- ConsultantsSalary.Infrastructure: EF Core DbContext, migrations, service implementations
- ConsultantsSalary.Api: Presentation (Controllers/Minimal APIs), configuration

## Roadmap
This repo will follow staged commits implementing:
1) Setup & docs
2) Domain + DbContext + Migrations
3) Identity + JWT
4) Rate history service + endpoint + tests
5) Time entry service + 12h validation + tests
6) CRUD for consultants/roles/tasks + profile image upload
7) Time entries + salary report
8) Swagger, global error handling, comprehensive docs

## License
MIT
