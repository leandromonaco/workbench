namespace ServiceName.Core.Model
{
    public class Query
    {
        public List<QueryField> FilterFields { get; set; }
        public string TableName { get; set; }
    }
}