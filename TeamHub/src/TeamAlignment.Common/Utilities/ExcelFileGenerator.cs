using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using ClosedXML.Excel;

namespace ScrumTools.Common.Utilities
{
    public class ExcelFileGenerator
    {
        public static void GenerateExcelFile(string fileName, Dictionary<string, object> excelSheets)
        {
            //Generate Output
            var wb = new XLWorkbook();

            foreach (var sheet in excelSheets)
            {
                var outputTable = new DataTable();

                Type type;
                try
                {
                    type = sheet.Value.GetType().GetProperty("Item").PropertyType;
                }
                catch (Exception) //TODO: Temporary workaround
                {
                    type = sheet.GetType();
                }

                //Define Headers
                foreach (PropertyInfo propertyInfo in type.GetProperties())
                {
                    outputTable.Columns.Add(propertyInfo.Name);
                }

                foreach (var item in (IEnumerable)sheet.Value)
                {
                    DataRow newRow = outputTable.NewRow();

                    //Populate row
                    foreach (PropertyInfo propertyInfo in type.GetProperties())
                    {
                        newRow[propertyInfo.Name] = type.GetProperty(propertyInfo.Name).GetValue(item);
                    }

                    outputTable.Rows.Add(newRow);
                }

                var ws = wb.Worksheets.Add(outputTable, sheet.Key);
                ws.Columns().AdjustToContents();
            }

            //Save Excel File
            wb.SaveAs(fileName);
        }
    }
}
