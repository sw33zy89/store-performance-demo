Add Migration:
dotnet ef migrations add "InitialMigration" --project "PerformanceTesting.Infrastructure" --startup-project "PerformanceTest.API" --output-dir Persistence\Migrations