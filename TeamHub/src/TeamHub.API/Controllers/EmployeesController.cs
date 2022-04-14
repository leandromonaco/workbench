using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TeamHub.API.Database;

namespace TeamHub.API.Controllers
{
    public static class EmployeesController
    {
        public static void MapEmployeesControllerEndpoints(this WebApplication app, TeamHubDatabaseContext dataContext, JsonSerializerOptions jsonSerializerOptions)
        {
            //All
            app.MapGet("/employees", async () => 
            {
                var employees =  await dataContext.Employees.Include(e => e.Specialization).ToListAsync();
                return JsonSerializer.Serialize(employees, jsonSerializerOptions);
            });

            //Create
            app.MapPost("/employees", async (Employee employee) =>
            {
                employee.Id = Guid.NewGuid();
                await dataContext.Employees.AddAsync(employee);
                await dataContext.SaveChangesAsync();
                return Results.Ok($"Employee {employee.Id} was created");
            });

            //Retrieve
            app.MapGet("/employees/{id}", async (Guid id) =>
            {
                var employee = await dataContext.Employees.Include(e => e.Specialization)
                                                    .Include(e => e.ReportsToNavigation).SingleOrDefaultAsync(x => x.Id == id);
                return JsonSerializer.Serialize(employee, jsonSerializerOptions);
            });

            //Update
            app.MapPut("/employees", async (Employee employee) =>
            {
                var emp = await dataContext.Employees.FindAsync(employee.Id);
                if (emp != null)
                {
                    emp.Name = employee.Name;
                    await dataContext.SaveChangesAsync();
                }

                return Results.Ok($"Employee {employee.Id} was updated");
            });

            //Delete
            app.MapDelete("/employees/{id}", async (Guid id) =>
            {
                var employee = await dataContext.Employees.FindAsync(id);
                if (employee != null)
                {
                    dataContext.Employees.Remove(employee);
                    await dataContext.SaveChangesAsync();
                }

                return Results.Ok($"Employee {id} was removed");
            });

        }
    }
}
