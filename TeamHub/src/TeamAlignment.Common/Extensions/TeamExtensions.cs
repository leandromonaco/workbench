using TeamAlignment.Core.Model.Database;
using TeamAlignment.Core.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamAlignment.Core.Common.Extensions
{
    public static class TeamExtensions
    {
        public static List<Employee> GetDevelopers(this Team team)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                var developers = dataContext.TeamChanges.Include(tc => tc.Team)
                                                  .Include(tc => tc.TeamMember)
                                                  .ThenInclude(d => d.Leaves)
                                                  .ThenInclude(to => to.Category).Where(tc => tc.Team.Name.Equals(team.Name)).Select(tc => tc.TeamMember).ToList();
                return developers;
            }
        }

        public static List<Leave> GetTimeOff(this Team team)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                List<Guid> teamMemberIds = team.GetDevelopers().Select(d => d.Id).ToList();
                var timeOffs = dataContext.Leaves.Include(to => to.TeamMember)
                                                 .Where(to => teamMemberIds.Contains(to.TeamMemberId)).ToList();
                return timeOffs;
            }
        }

        public static List<DateTime> GetDevTimeOffDates(this Team team)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                var result = new List<DateTime>();
                var timeOffs = dataContext.Leaves.Include(to => to.TeamMember)
                                                 .ThenInclude(d => d.Specialization).Where(to => team.GetDevelopers().Select(d => d.Id).Contains(to.TeamMemberId)).ToList();
                var timeOffsDev = timeOffs.Where(to => ((SpecializationEnum)to.TeamMember.SpecializationId).Equals(SpecializationEnum.Development)).ToList();
                timeOffsDev.ForEach(to => result.AddRange(to.GetDates()));
                return result;
            }
        }

        public static List<DateTime> GetQATimeOffDates(this Team team)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                var result = new List<DateTime>();
                var timeOffs = dataContext.Leaves.Include(to => to.TeamMember)
                                                 .ThenInclude(d => d.Specialization).Where(to => team.GetDevelopers().Select(d => d.Id).Contains(to.TeamMemberId)).ToList();
                var timeOffsDev = timeOffs.Where(to => ((SpecializationEnum)to.TeamMember.SpecializationId).Equals(SpecializationEnum.Testing)).ToList();
                timeOffsDev.ForEach(to => result.AddRange(to.GetDates()));
                return result;
            }
        }

        public static List<DateTime> GetQAWorkDates(this Team team)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                var result = new List<DateTime>();
                var teamChanges = dataContext.TeamChanges.Include(tc => tc.TeamMember)
                                                         .ThenInclude(d => d.Specialization).Where(tc => team.GetDevelopers().Select(d => d.Id).Contains(tc.TeamMemberId)).ToList();
                var teamChangesQA = teamChanges.Where(tc => ((SpecializationEnum)tc.TeamMember.SpecializationId).Equals(SpecializationEnum.Testing)).ToList();
                teamChangesQA.ForEach(tc => result.AddRange(tc.GetDates()));
                return result;
            }
        }

        public static List<DateTime> GetDevWorkDates(this Team team)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                var result = new List<DateTime>();
                var teamChanges = dataContext.TeamChanges.Include(tc => tc.TeamMember)
                                                         .ThenInclude(d => d.Specialization).Where(tc => team.GetDevelopers().Select(d => d.Id).Contains(tc.TeamMemberId)).ToList();
                var teamChangesDev = teamChanges.Where(tc => ((SpecializationEnum)tc.TeamMember.SpecializationId).Equals(SpecializationEnum.Development)).ToList();
                teamChangesDev.ForEach(tc => result.AddRange(tc.GetDates()));
                return result;
            }
        }

         public static List<DateTime> GetDevPublicHolidaysDates(this Team team)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                var result = new List<DateTime>();
                var publicHolidayDates = team.Location.PublicHolidaysLocations.Select(ph => ph.PublicHoliday.Date);
                var teamChanges = dataContext.TeamChanges.Include(tc => tc.TeamMember)
                                                         .ThenInclude(d => d.Specialization).Where(tc => team.GetDevelopers().Select(d => d.Id).Contains(tc.TeamMemberId)).ToList();
                var teamChangesDev = teamChanges.Where(tc => ((SpecializationEnum)tc.TeamMember.SpecializationId).Equals(SpecializationEnum.Development)).ToList();
                teamChangesDev.ForEach(tc => result.AddRange(tc.GetDates()));
                return result.Where(d => publicHolidayDates.Contains(d)).ToList();
            }
        }

        public static List<DateTime> GetQAPublicHolidaysDates(this Team team)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                var result = new List<DateTime>();
                var publicHolidayDates = team.Location.PublicHolidaysLocations.Select(ph => ph.PublicHoliday.Date);
                var teamChanges = dataContext.TeamChanges.Include(tc => tc.TeamMember)
                                                         .ThenInclude(d => d.Specialization).Where(tc => team.GetDevelopers().Select(d => d.Id).Contains(tc.TeamMemberId)).ToList();
                var teamChangesDev = teamChanges.Where(tc => ((SpecializationEnum)tc.TeamMember.SpecializationId).Equals(SpecializationEnum.Testing)).ToList();
                teamChangesDev.ForEach(tc => result.AddRange(tc.GetDates()));
                return result.Where(d => publicHolidayDates.Contains(d)).ToList();
            }
        }

        public static List<TeamChange> GetTeamChanges(this Team team)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                var teamChanges = dataContext.TeamChanges.Include(tc => tc.TeamMember)
                                                         .Include(tc => tc.Team).Where(tc => team.GetDevelopers().Select(d => d.Id).Contains(tc.TeamMemberId)).ToList();
                return teamChanges;
            }
        }
    }
}
