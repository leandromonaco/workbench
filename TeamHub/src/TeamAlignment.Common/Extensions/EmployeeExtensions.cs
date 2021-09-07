using TeamAlignment.Core.Model.Database;
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

        public static List<Team> GetTeams(this Employee teamMember)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                var teams = dataContext.TeamChanges.Include(tc => tc.Team)
                                                   .ThenInclude(t => t.Location)
                                                   .OrderByDescending(tc => tc.FirstDay)
                                                   .Where(tc => tc.TeamMemberId.Equals(teamMember.Id))
                                                   .Select(tc => tc.Team).ToList();
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
