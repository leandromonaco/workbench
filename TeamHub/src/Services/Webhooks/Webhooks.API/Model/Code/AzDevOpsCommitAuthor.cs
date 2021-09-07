using System;

namespace Webhooks.API.Model.Code
{
    public class AzDevOpsCommitAuthor
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
    }
}