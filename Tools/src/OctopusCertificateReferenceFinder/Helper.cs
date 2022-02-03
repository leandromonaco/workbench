using IntegrationConnectors.Octopus.Model;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchInFiles
{
    class Helper
    {

        public static void ExportExcel(List<CertificateUsage> certificateUsages)
        {
            var outputFile = Path.Combine(Directory.GetCurrentDirectory(), "CertificatesReport.xlsx");
            using (var fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Certificate Usage");

                IRow row = excelSheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("Octopus Project");
                row.CreateCell(1).SetCellValue("Certificate Name");
                row.CreateCell(2).SetCellValue("Certificate Thumbprint");
                row.CreateCell(3).SetCellValue("Expiration Date");

                var rowCount = 1;


                foreach (var usage in certificateUsages)
                {
                    row = excelSheet.CreateRow(rowCount);
                    row.CreateCell(0).SetCellValue(usage.Project);
                    row.CreateCell(1).SetCellValue(usage.CertificateName);
                    row.CreateCell(2).SetCellValue(usage.Thumbprint);
                    row.CreateCell(3).SetCellValue(usage.ExpirationDate);


                    rowCount++;
                }

                workbook.Write(fs);

            }
        }
    }
}
