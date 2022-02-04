using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace JiraReport
{
    class Helper
    {

        public static void ExportExcel(List<BacklogReportRow> backlogReportRows)
        {
            var outputFile = Path.Combine(Directory.GetCurrentDirectory(), "JiraData.xlsx");
            using (var fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Variables");

                IRow row = excelSheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("Date");
                row.CreateCell(1).SetCellValue("Epic");
                row.CreateCell(2).SetCellValue("JIRA ID");
                row.CreateCell(3).SetCellValue("Description");
                row.CreateCell(4).SetCellValue("IssueType");
                row.CreateCell(5).SetCellValue("Severity");
                row.CreateCell(6).SetCellValue("Status");
                row.CreateCell(7).SetCellValue("Sprint");
                row.CreateCell(8).SetCellValue("Points");
                row.CreateCell(9).SetCellValue("Assigned To");

                var rowCount = 1;


                foreach (var backlogReportRow in backlogReportRows)
                {
                    row = excelSheet.CreateRow(rowCount);
                    row.CreateCell(0).SetCellValue(backlogReportRow.Date);
                    row.CreateCell(1).SetCellValue(backlogReportRow.Epic);
                    row.CreateCell(2).SetCellValue(backlogReportRow.JiraId);
                    row.CreateCell(3).SetCellValue(backlogReportRow.JiraDescription);
                    row.CreateCell(4).SetCellValue(backlogReportRow.IssueType);
                    row.CreateCell(5).SetCellValue(backlogReportRow.Severity);
                    row.CreateCell(6).SetCellValue(backlogReportRow.Status);
                    row.CreateCell(7).SetCellValue(backlogReportRow.Sprint);
                    row.CreateCell(8).SetCellValue(Convert.ToDouble(backlogReportRow.Points));
                    row.CreateCell(9).SetCellValue(backlogReportRow.AssignedTo);
                    rowCount++;
                }

                workbook.Write(fs);

            }
        }
    }
}
