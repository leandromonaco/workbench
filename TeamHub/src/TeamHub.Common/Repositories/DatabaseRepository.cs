using TeamHub.Model.Database;
using TeamAlignment.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamAlignment.Core.Common.Enums;

namespace TeamAlignment.Repositories
{
    public class DatabaseRepository
    {
        public async Task<List<Employee>> GetDevelopers()
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                var developers = dataContext.Employees.Include(d => d.Specialization)
                                                   .Include(d => d.TeamChanges)
                                                   .ThenInclude(tc => tc.Team).ToList();
                return developers;
            }
        }

        public List<Employee> GetEmployeesByManagerId(Guid managerId)
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                var developers = dataContext.Employees.Where(e => e.ReportsTo.Equals(managerId))
                                                    .Include(d => d.Specialization)
                                                    .Include(d => d.TeamChanges)
                                                    .ThenInclude(tc => tc.Team).ToList();
                return developers;
            }
        }

       

        public async Task<List<Specialization>> GetSpecializationsAsync()
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                return dataContext.Specializations.ToList();
            }
        }

        public async Task<Employee> GetTeamMemberAsync(string teamMember)
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                var dev = dataContext.Employees.Where(d => d.Email.ToLower().Equals(teamMember.ToLower()) ||
                                                     d.LoginUser.Equals(teamMember.ToLower()))
                                                 .Include(d => d.TeamChanges)
                                                 .ThenInclude(tc => tc.Team)
                                                 .Include(d => d.Specialization).FirstOrDefault();
                if (dev == null)
                {
                    dev = new Employee();
                    if (teamMember.Contains("vstfs://"))
                    {
                        var split = teamMember.Split("\\");
                        if (split.Length.Equals(0))
                        {
                            dev.Email = teamMember;
                            dev.LoginUser = teamMember;
                            dev.Name = teamMember;
                        }
                        else
                        {
                            dev.Name = split[split.Length - 1];
                        }
                    }
                    else
                    {
                        dev.Email = teamMember;
                        dev.LoginUser = teamMember;
                        dev.Name = teamMember;
                    }
                }
                return dev;
            }
        }
        public async Task<List<Employee>> GetEmployees()
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                return dataContext.Employees.AsNoTracking()
                                         .Include(d => d.TeamChanges)
                                         .ThenInclude(tc => tc.Team)
                                         .Include(d => d.Leaves)
                                         .ThenInclude(to => to.Category)
                                         .Include(d => d.Specialization).ToList();
            }
        }

        public async Task<Employee> GetEmployee(Guid id)
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                return dataContext.Employees.AsNoTracking().Where(d => d.Id.Equals(id))
                                         .Include(d => d.TeamChanges)
                                         .ThenInclude(tc => tc.Team)
                                         .Include(d => d.Leaves)
                                         .ThenInclude(to => to.Category)
                                         .Include(d => d.Specialization).FirstOrDefault();
            }
        }
        

        public async Task<List<Location>> GetLocations()
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                return dataContext.Locations.OrderBy(o => o.Name).ToList();
            }
        }

       
        public async Task<Team> GetTeamAsync(string name)
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                if (string.IsNullOrEmpty(name))
                {
                    return null;
                }
                var team = dataContext.Teams.AsNoTracking()
                                            .Include(t => t.Product)
                                            .ThenInclude(p => p.Configuration)
                                            .Include(t => t.TeamChanges)
                                            .ThenInclude(tc => tc.TeamMember)
                                            .Include(t => t.Location)
                                            .ThenInclude(l => l.TimeZone)
                                            .Include(t => t.Location)
                                            .ThenInclude(l => l.PublicHolidaysLocations)
                                            .ThenInclude(ph => ph.PublicHoliday)
                                            .Include(t => t.Milestones).Where(t => t.Name.Equals(name)).FirstOrDefault();
                return team;
            }
        }
        public List<LeaveType> GetTimeOffCategories()
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                return dataContext.LeaveTypes.ToList();
            }
        }
        public async Task SaveTimeOffAsync(Guid developerId, byte timeOffCategoryId, DateTime dateFrom, DateTime dateTo)
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                Leave leave = new Leave();
                leave.EmployeeId = developerId;
                leave.LeaveTypeId = timeOffCategoryId;
                leave.StartDate = dateFrom;
                leave.EndDate = dateTo;
                leave.IsPlanned = true;
                dataContext.Leaves.Add(leave);
                await dataContext.SaveChangesAsync();
            }
        }

        public async Task SaveTransferAsync(Transfer transfer)
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                dataContext.Transfers.Add(transfer);
                await dataContext.SaveChangesAsync();
            }
        }
        public async Task<List<Leave>> GetTeamTimeOffAsync(Team team)
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                return dataContext.Leaves.Include(to => to.TeamMember)
                                           .Include(to => to.Category)
                                           .Where(to => team.GetEmployees().Select(d => d.Id).Contains(to.TeamMemberId))
                                           .OrderByDescending(to => to.StartDate).ToList();
            }
        }
        public async Task<List<Leave>> GetDeveloperTimeOffAsync(Guid developerId)
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                return dataContext.Leaves.Include(to => to.TeamMember)
                                           .Include(to => to.Category)
                                           .Where(to => to.TeamMemberId.Equals(developerId))
                                           .OrderByDescending(to => to.StartDate).ToList();
            }
        }
        public async Task DeleteTimeOffAsync(Guid timeOffId)
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                var timeOff = dataContext.Leaves.Where(to => to.Id.Equals(timeOffId)).FirstOrDefault();
                if (timeOff != null)
                {
                    dataContext.Leaves.Remove(timeOff);
                    await dataContext.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateTeamAllocation(Guid teamChangeId, DateTime dateTo)
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                var teamChange = dataContext.TeamChanges.Where(tc => tc.Id.Equals(teamChangeId)).FirstOrDefault();
                if (teamChange != null)
                {
                    teamChange.LastDay = dateTo;
                    await dataContext.SaveChangesAsync();
                }
            }
        }
        public async Task<List<Team>> GetTeamsAsync()
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                var teams = dataContext.Teams.ToList();
                return teams;
            }
        }
       

        public async Task SavePublicHolidayAsync(Guid locationId, DateTime date, string description)
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                PublicHoliday publicHoliday = new PublicHoliday();
                publicHoliday.Date = date;
                publicHoliday.Description = description;
                publicHoliday.LocationId = locationId;
                dataContext.PublicHolidays.Add(publicHoliday);
                await dataContext.SaveChangesAsync();
            }
        }

        public async Task DeleteMilestoneAsync(Guid milestoneId)
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                var milestone = dataContext.Milestones.Where(to => to.Id.Equals(milestoneId)).FirstOrDefault();
                if (milestone != null)
                {
                    dataContext.Milestones.Remove(milestone);
                    await dataContext.SaveChangesAsync();
                }
            }
        }

        public async Task DeletePublicHolidayAsync(Guid publicHolidayId)
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                var publicHoliday1 = dataContext.PublicHolidaysLocations.Include(ph => ph.PublicHoliday).Where(ph => ph.PublicHolidayId.Equals(publicHolidayId)).FirstOrDefault();
                if (publicHoliday1 != null)
                {
                    dataContext.PublicHolidaysLocations.Remove(publicHoliday1);
                    dataContext.PublicHolidays.Remove(publicHoliday1.PublicHoliday);
                    await dataContext.SaveChangesAsync();
                }
            }
        }

        public async Task<int> GetCarryOverPointsAsync(string id)
        {
            using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
            {
                var carryOverPoints = dataContext.CarryOverPoints.Where(co => co.Id.Equals(id)).FirstOrDefault();
                return carryOverPoints == null ? 0 : carryOverPoints.CarryOverPoints1;
            }
        }

        public async Task<bool> SaveEmployee(Employee employee)
        {
            try
            {
                using (TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext())
                {
                    dataContext.Entry(employee).State = EntityState.Modified;
                    await dataContext.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                //log exception
                return false;
            }
        }
    }
}
