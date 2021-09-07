namespace IntegrationConnectors.Fortify.Model
{
    public class FortifyProject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string IssueTemplateId { get; set; }
    }
}