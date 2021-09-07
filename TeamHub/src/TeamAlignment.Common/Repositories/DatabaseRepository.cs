using TeamAlignment.Core.Model.Database;
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
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                var developers = dataContext.Employees.Include(d => d.Specialization)
                                                   .Include(d => d.TeamChanges)
                                                   .ThenInclude(tc => tc.Team).ToList();
                return developers;
            }
        }

        public List<Employee> GetEmployeesByManagerId(Guid managerId)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                var developers = dataContext.Employees.Where(e => e.ReportsTo.Equals(managerId))
                                                    .Include(d => d.Specialization)
                                                    .Include(d => d.TeamChanges)
                                                    .ThenInclude(tc => tc.Team).ToList();
                return developers;
            }
        }

        public async Task<Questionnaire> GetQuestionnaire(Guid id)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                var questionnaire = dataContext.Questionnaires.Include(q => q.QuestionnaireSections)
                                                              .ThenInclude(qs => qs.QuestionnaireQuestions)
                                                              .Where(q => q.Id.Equals(id)).FirstOrDefault();

                return questionnaire;
            }
        }

        public async Task<QuestionnairePeriod> GetPeriod(Guid id)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                var period = dataContext.QuestionnairePeriods.Where(p => p.Id.Equals(id)).FirstOrDefault();
                return period;
            }
        }

        public async Task<bool> SaveQuestionnaireAnswers(Questionnaire questionnaire, Employee employee, QuestionnairePeriod period)
        {
            try
            {
                using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
                {
                    var snapshot = new Snapshot();
                    snapshot.Id = Guid.NewGuid();
                    snapshot.EmployeeId = employee.Id;
                    snapshot.Content = JsonConvert.SerializeObject(questionnaire, new JsonSerializerSettings { MaxDepth = 1, ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    snapshot.Type = (int)SnapshotEnum.QuestionnaireAnswers;
                    snapshot.Timestamp = period.To;
                    dataContext.Snapshots.Add(snapshot);
                    dataContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                //log exception
                return false;
            }
        }

        //TODO: Migrate to quesionnaire extensions
        public async Task<bool> IsQuestionnaireAnswered(Questionnaire questionnaire, Employee employee, QuestionnairePeriod period)
        {
            if (questionnaire == null || employee == null || period == null)
            {
                return false;
            }

            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                var snapshot = dataContext.Snapshots.Where(s => s.Type.Equals(3) &&
                                                                s.Timestamp.Equals(period.To) &&
                                                                s.EmployeeId.Equals(employee.Id)).FirstOrDefault();

                if (snapshot == null)
                {
                    return false;
                }
                else
                {
                    var questionnaireAnswers = JsonConvert.DeserializeObject<Questionnaire>(snapshot.Content);
                    if (questionnaireAnswers.Id.Equals(questionnaire.Id))
                    {
                        return true;
                    }
                    return false;
                }
            }
        }

        public async Task<List<Specialization>> GetSpecializationsAsync()
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                return dataContext.Specializations.ToList();
            }
        }

        public async Task<Employee> GetTeamMemberAsync(string teamMember)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
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
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
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
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                return dataContext.Employees.AsNoTracking().Where(d => d.Id.Equals(id))
                                         .Include(d => d.TeamChanges)
                                         .ThenInclude(tc => tc.Team)
                                         .Include(d => d.Leaves)
                                         .ThenInclude(to => to.Category)
                                         .Include(d => d.Specialization).FirstOrDefault();
            }
        }
        public async Task<List<PublicHolidayLocation>> GetPublicHolidays()
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                return dataContext.PublicHolidaysLocations.Include(ph => ph.Location).Include(ph => ph.PublicHoliday).OrderBy(ph => ph.PublicHoliday.Date).ToList();
            }
        }

        public async Task<List<Location>> GetLocations()
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                return dataContext.Locations.OrderBy(o => o.Name).ToList();
            }
        }

        public async Task<List<Milestone>> GetMilestones()
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                return dataContext.Milestones.Include(m => m.Team).ToList();
            }
        }
        public async Task<Team> GetTeamAsync(string name)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
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
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                return dataContext.LeaveTypes.ToList();
            }
        }
        public async Task SaveTimeOffAsync(Guid developerId, byte timeOffCategoryId, DateTime dateFrom, DateTime dateTo)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                Leave timeOff = new Leave();
                timeOff.TeamMemberId = developerId;
                timeOff.CategoryId = timeOffCategoryId;
                timeOff.StartDate = dateFrom;
                timeOff.EndDate = dateTo;
                timeOff.IsPlanned = true;
                dataContext.Leaves.Add(timeOff);
                await dataContext.SaveChangesAsync();
            }
        }

        public async Task SaveTeamChangeAsync(TeamChange teamChange)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                dataContext.TeamChanges.Add(teamChange);
                await dataContext.SaveChangesAsync();
            }
        }
        public async Task<List<Leave>> GetTeamTimeOffAsync(Team team)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                return dataContext.Leaves.Include(to => to.TeamMember)
                                           .Include(to => to.Category)
                                           .Where(to => team.GetDevelopers().Select(d => d.Id).Contains(to.TeamMemberId))
                                           .OrderByDescending(to => to.StartDate).ToList();
            }
        }
        public async Task<List<Leave>> GetDeveloperTimeOffAsync(Guid developerId)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                return dataContext.Leaves.Include(to => to.TeamMember)
                                           .Include(to => to.Category)
                                           .Where(to => to.TeamMemberId.Equals(developerId))
                                           .OrderByDescending(to => to.StartDate).ToList();
            }
        }
        public async Task DeleteTimeOffAsync(Guid timeOffId)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
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
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
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
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                var teams = dataContext.Teams.ToList();
                return teams;
            }
        }
        public async Task<Product> GetProductAsync(string productName)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                var product = dataContext.Products.Include(p => p.Configuration)
                                              .Include(p => p.Teams)
                                              .ThenInclude(t => t.Location)
                                              .ThenInclude(l => l.TimeZone)
                                              .Include(p => p.Teams)
                                              .ThenInclude(t => t.Location)
                                              .ThenInclude(l => l.PublicHolidaysLocations)
                                              .ThenInclude(ph => ph.PublicHoliday)
                                              .Where(p => p.Name.Equals(productName)).FirstOrDefault();
                return product;
            }
        }

        /*Save*/

        public async Task SaveMilestoneAsync(Guid teamId, DateTime date, string description)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                Milestone milestone = new Milestone();
                milestone.TeamId = teamId;
                milestone.Date = date;
                milestone.Description = description;
                dataContext.Milestones.Add(milestone);
                await dataContext.SaveChangesAsync();
            }
        }

        public async Task SavePublicHolidayAsync(Guid locationId, DateTime date, string description)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                PublicHoliday publicHoliday = new PublicHoliday();
                publicHoliday.Date = date;
                publicHoliday.Description = description;
                dataContext.PublicHolidays.Add(publicHoliday);
                await dataContext.SaveChangesAsync();

                PublicHolidayLocation publicHoliday1 = new PublicHolidayLocation();
                publicHoliday1.LocationId = locationId;
                publicHoliday1.PublicHoliday = publicHoliday;
                dataContext.PublicHolidaysLocations.Add(publicHoliday1);
                await dataContext.SaveChangesAsync();
            }
        }

        public async Task DeleteMilestoneAsync(Guid milestoneId)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
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
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
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
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                var carryOverPoints = dataContext.CarryOverPoints.Where(co => co.Id.Equals(id)).FirstOrDefault();
                return carryOverPoints == null ? 0 : carryOverPoints.CarryOverPoints1;
            }
        }

        public async Task SaveCarryOverPointsAsync(string id, int points)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                var carryOver = dataContext.CarryOverPoints.Where(co => co.Id.Equals(id)).FirstOrDefault();
                if (carryOver != null)
                {
                    if (points > 0)
                    {
                        carryOver.CarryOverPoints1 = points;
                        dataContext.Entry(carryOver).State = EntityState.Modified;
                    }
                    else
                    {
                        dataContext.CarryOverPoints.Remove(carryOver);
                    }
                }
                else
                {
                    var newCarryOver = new CarryOverPoints();
                    newCarryOver.Id = id;
                    newCarryOver.CarryOverPoints1 = points;
                    dataContext.CarryOverPoints.Add(newCarryOver);
                }

                await dataContext.SaveChangesAsync();
            }
        }

        public async Task<bool> SaveEmployee(Employee employee)
        {
            try
            {
                using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
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

        public async Task<Snapshot> GetSnapshotAsync(Product product, short type)
        {
            using (TeamAlignmentContext dataContext = new TeamAlignmentContext())
            {
                return dataContext.Snapshots.Where(s => s.Type.Equals(type) &&
                                                       s.ProductId.Equals(product.Id)).FirstOrDefault();
            }
        }
    }
}
