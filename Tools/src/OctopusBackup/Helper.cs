using IntegrationConnectors.Octopus.Model;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctopusBackup
{
    class Helper
    {

        public static void ExportExcel(List<OctopusVariable> variables)
        {
            var outputFile = Path.Combine(Directory.GetCurrentDirectory(), "OctopusData.xlsx");
            using (var fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Variables");

                IRow row = excelSheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("Name");
                row.CreateCell(1).SetCellValue("Value");
                row.CreateCell(2).SetCellValue("Environments");
                row.CreateCell(2).SetCellValue("Machines");

                var rowCount = 1;


                foreach (var variable in variables)
                {
                    row = excelSheet.CreateRow(rowCount);
                    row.CreateCell(0).SetCellValue(variable.Name);
                    row.CreateCell(1).SetCellValue(variable.Value);
                    if (variable.Scope.Environments!=null)
                    {
                        row.CreateCell(2).SetCellValue(string.Join(',', variable.Scope.Environments));
                    }
                    if (variable.Scope.Machines != null)
                    {
                        row.CreateCell(2).SetCellValue(string.Join(',', variable.Scope.Machines));
                    }


                    rowCount++;
                }

                workbook.Write(fs);

            }
        }
    }
}
