using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace JiraReporting.Report
{
    class Helper
    {

        public static void ExportExcel(List<BacklogItem> backlogItems, List<BacklogItem> newBacklogItems, string outputFile)
        {
            using (var fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Backlog");

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


                foreach (var backlogItem in backlogItems)
                {
                    row = excelSheet.CreateRow(rowCount);
                    row.CreateCell(0).SetCellValue(backlogItem.Date);
                    row.CreateCell(1).SetCellValue(backlogItem.Epic);
                    row.CreateCell(2).SetCellValue(backlogItem.JiraId);
                    row.CreateCell(3).SetCellValue(backlogItem.JiraDescription);
                    row.CreateCell(4).SetCellValue(backlogItem.IssueType);
                    row.CreateCell(5).SetCellValue(backlogItem.Severity);
                    row.CreateCell(6).SetCellValue(backlogItem.Status);
                    row.CreateCell(7).SetCellValue(backlogItem.Sprint);
                    row.CreateCell(8).SetCellValue(Convert.ToDouble(backlogItem.Points));
                    row.CreateCell(9).SetCellValue(backlogItem.AssignedTo);
                    rowCount++;
                }

                workbook.Write(fs);

            }
        }
    }
}
