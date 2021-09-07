1 - Install EF command
    dotnet tool install -g dotnet-ef --version 3.0.0
2 - Update EF command (if required)
    dotnet tool update -g dotnet-ef --version 3.1.4
3 - Navigate TeamAlignment\src\TeamAlignment.Model
4 - Update EF Model Classes
    dotnet ef dbcontext scaffold "Server=localhost;Database=TeamAlignment;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Database -f