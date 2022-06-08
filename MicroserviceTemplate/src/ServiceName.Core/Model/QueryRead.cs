namespace ServiceName.Core.Model
{
    public class QueryRead: Query
    {
        public object DefaultResult { get; set; }
        public List<string> SelectFields { get; set; }
    }
}
