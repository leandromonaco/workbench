using TeamHub.Model.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamAlignment.Core.Common.Extensions
{
    public static class EmployeeExtensions
    {
        public static bool IsActive(this Employee developer)
        {
            var lastDay = developer.TeamChanges.OrderByDescending(tc => tc.FirstDay).First().LastDay;
            if (lastDay != null && lastDay < DateTime.Now)
            {
                return false;
            }

            return true;
        }

        public static List<Team> GetTeams(this Employee employee)
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                var teams = dataContext.Transfers.Include(tc => tc.FromTeam)
                                                   .Include(tc => tc.ToTeam)
                                                   .OrderByDescending(tc => tc.Date)
                                                   .Where(tc => tc.EmployeeId.Equals(employee.Id))
                                                   .Select(tc => tc.ToTeam).ToList();
                if (teams.Count.Equals(0))
                {
                    var team = new Team() { Name = "Unassigned Team" };
                    teams.Add(team);
                }

                return teams;
            }
        }

    }
}
