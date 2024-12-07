Root Path: `https://localhost:34381/`

Routes:
- Healthcheck - `/`
- Api Docs - `/scalar/v1`

Entity Framework:
To add a migration from the solution:
`dotnet ef migrations add "{{Migration Name}}" --project "PerformanceTesting.Infrastructure" --startup-project "PerformanceTest.API" --output-dir Persistence\Migrations`
ex:
`dotnet ef migrations add "InitialMigration" --project "PerformanceTesting.Infrastructure" --startup-project "PerformanceTest.API" --output-dir Persistence\Migrations`