﻿using IntegrationConnectors.JIRA.Model;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace JiraReporting.Report
{
    public class ExcelHelper
    {

        public static void Export(List<JiraBacklogItem> backlogItems, List<JiraBacklogItem> newBacklogItems, string outputFile, string jiraUrl)
        {
            using (var fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Backlog");

                ICellStyle hlink_style = workbook.CreateCellStyle();
                IFont hlink_font = workbook.CreateFont();
                hlink_font.Underline = FontUnderlineType.Single;
                hlink_font.Color = HSSFColor.Blue.Index;
                hlink_style.SetFont(hlink_font);

                IRow row = excelSheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("Date");
                row.CreateCell(1).SetCellValue("Sprint");
                row.CreateCell(2).SetCellValue("EpicId");
                row.CreateCell(3).SetCellValue("EpicTitle");
                row.CreateCell(4).SetCellValue("IssueId");
                row.CreateCell(5).SetCellValue("IssueTitle");
                row.CreateCell(6).SetCellValue("IssueType");
                row.CreateCell(7).SetCellValue("Severity");
                row.CreateCell(8).SetCellValue("Status");
                row.CreateCell(9).SetCellValue("Points");
                row.CreateCell(10).SetCellValue("Assigned To");

                var rowCount = 1;


                foreach (var backlogItem in backlogItems)
                {
                    row = excelSheet.CreateRow(rowCount);
                    row.CreateCell(0).SetCellValue(backlogItem.Date);
                    row.CreateCell(1).SetCellValue(backlogItem.Sprint);

                    var link = new XSSFHyperlink(HyperlinkType.Url)
                    {
                        Address = $"{jiraUrl}/browse/{backlogItem.EpicId}"
                    };
                    var epicIdCell = row.CreateCell(2);
                    epicIdCell.SetCellValue(backlogItem.EpicId);
                    epicIdCell.Hyperlink = link;
                    epicIdCell.CellStyle = hlink_style;

                    row.CreateCell(3).SetCellValue(backlogItem.EpicTitle);

                    link = new XSSFHyperlink(HyperlinkType.Url)
                    {
                        Address = $"{jiraUrl}/browse/{backlogItem.IssueId}"
                    };
                    var issueIdCell = row.CreateCell(4);
                    issueIdCell.SetCellValue(backlogItem.IssueId);
                    issueIdCell.Hyperlink = link;
                    issueIdCell.CellStyle = hlink_style;

                    row.CreateCell(5).SetCellValue(backlogItem.IssueTitle);
                    row.CreateCell(6).SetCellValue(backlogItem.IssueType);
                    row.CreateCell(7).SetCellValue(backlogItem.Priority);
                    row.CreateCell(8).SetCellValue(backlogItem.Status);
                    row.CreateCell(9).SetCellValue(Convert.ToDouble(backlogItem.Points));
                    row.CreateCell(10).SetCellValue(backlogItem.AssignedTo);
                    rowCount++;
                }

                //Column width auto-size
                int numberOfColumns = excelSheet.GetRow(0).PhysicalNumberOfCells;
                for (int i = 1; i <= numberOfColumns; i++)
                {
                    excelSheet.AutoSizeColumn(i);
                    GC.Collect(); // Add this line
                }

                //Create filters
                excelSheet.SetAutoFilter(new CellRangeAddress(0, 0, 0, numberOfColumns));
                excelSheet.CreateFreezePane(0, 1);

                workbook.Write(fs);

            }
        }
    }
}
