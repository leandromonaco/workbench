using AutoMapper;
using AutomationToolkit.AzDevOps.Model.PullRequest;

namespace PullRequestsMonitor.UI.Model
{
    public class Mapper
    {
        public static PullRequest Map(AzDevOpsPullRequest pullRequest)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<AzDevOpsPullRequest, PullRequest>());
            var mapper = config.CreateMapper();
            return mapper.Map<PullRequest>(pullRequest);
        }
    }
}
