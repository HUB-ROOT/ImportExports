using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;

public class ExcelExportService
{
    public byte[] GenerateExcel<T>(IEnumerable<T> data, Dictionary<string, List<string>> schema, string sheetName)
    {
        using (var package = new ExcelPackage())
        {

            // Add data sheet
            var dataSheet = package.Workbook.Worksheets.Add(sheetName + "_Data");

            // Add headers based on the schema
            int headerRow = 1;
            int column = 1;
            foreach (var propertyName in schema.Keys)
            {
                dataSheet.Cells[headerRow, column++].Value = propertyName;
            }

            // Add data
            int dataRow = headerRow + 1;
            foreach (var item in data)
            {
                column = 1;
                foreach (var propertyName in schema.Keys)
                {
                    var propertyValue = typeof(T).GetProperty(propertyName)?.GetValue(item);
                    dataSheet.Cells[dataRow, column++].Value = propertyValue?.ToString();
                }
                dataRow++;
            }
            // Add schema sheet
            var schemaSheet = package.Workbook.Worksheets.Add(sheetName + "_Schema");

            // Add headers for schema
            int schemaHeaderRow = 1;
            int schemaColumn = 1;

            // Add schema headers
            schemaSheet.Cells[schemaHeaderRow, schemaColumn++].Value = "Column Name";
            schemaSheet.Cells[schemaHeaderRow, schemaColumn++].Value = "Data Type";
            schemaSheet.Cells[schemaHeaderRow, schemaColumn++].Value = "Allow Null";

            // Add schema data
            foreach (var propertyName in schema.Keys)
            {
                schemaSheet.Cells[++schemaHeaderRow, 1].Value = propertyName; // Column Name

                var dataTypeList = schema[propertyName];
                schemaSheet.Cells[schemaHeaderRow, 2].Value = dataTypeList[0]; // Data Type
                schemaSheet.Cells[schemaHeaderRow, 3].Value = dataTypeList.Contains("Nullable") ? "Yes" : "No"; // Allow Null
            }

           

            return package.GetAsByteArray();
        }
    }

}
