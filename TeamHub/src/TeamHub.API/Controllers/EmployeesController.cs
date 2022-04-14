using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TeamHub.API.Database;

namespace TeamHub.API.Controllers
{
    public static class EmployeesController
    {
        public static void MapEmployeesControllerEndpoints(this WebApplication app, TeamHubDatabaseContext dataContext, JsonSerializerOptions jsonSerializerOptions)
        {
            app.MapGet("/employees", async () => 
            {
                var employees =  dataContext.Employees.Include(e => e.Specialization).ToList();
                return JsonSerializer.Serialize(employees, jsonSerializerOptions);
            });

            app.MapGet("/employees/{id}", async (Guid id) =>
            {
                var employee = dataContext.Employees.Include(e => e.Specialization)
                                                    .Include(e => e.ReportsToNavigation).SingleOrDefault(x => x.Id == id);
                return JsonSerializer.Serialize(employee, jsonSerializerOptions);
            });

            app.MapDelete("/employees/{id}", async (Guid id) =>
            {
                var employee = dataContext.Employees.SingleOrDefault(x => x.Id == id);
                if (employee != null)
                {
                    dataContext.Employees.Remove(employee);
                    await dataContext.SaveChangesAsync();
                }

                return Results.Ok($"Employee {id} was removed");
            });

            app.MapPost("/employees", async (Employee employee) =>
            {
                employee.Id = Guid.NewGuid();
                await dataContext.Employees.AddAsync(employee);
                await dataContext.SaveChangesAsync();
                return Results.Ok($"Employee {employee.Id} was created");
            });

            app.MapPut("/employees", async (Employee employee) =>
            {
                var emp = dataContext.Employees.SingleOrDefault(x => x.Id == employee.Id);
                if (emp != null)
                {
                    emp.Name = employee.Name;
                    await dataContext.SaveChangesAsync();
                }

                return Results.Ok($"Employee {employee.Id} was updated");
            });

        }
    }
}
