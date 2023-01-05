namespace IntegrationConnectors.Bitbucket
{
    public class BitBucketTarget
    {
        public BitBucketAuthor Author { get; set; }
        public DateTime Date { get; set; }
        public BitBucketRepository Repository { get; set; }
    }
}