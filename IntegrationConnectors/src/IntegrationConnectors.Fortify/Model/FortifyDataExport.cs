using System;

namespace IntegrationConnectors.Fortify.Model
{
    public class FortifyDataExport
    {
        public int Id { get; set; }
        public string DatasetName { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public DateTime ExportDate { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public int AppVersionId { get; set; }
        public string AppVersionName { get; set; }
        public int Expiration { get; set; }
    }
}