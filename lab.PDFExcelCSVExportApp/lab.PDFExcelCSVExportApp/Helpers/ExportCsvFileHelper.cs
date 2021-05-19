using CsvHelper;
using CsvHelper.Configuration;
using lab.PDFExcelCSVExportApp.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Text;

namespace lab.PDFExcelCSVExportApp.Helpers
{
    public static class ExportCsvFileHelper
    {
        public static ExportFileViewModel ExportToCSVFileByCsvHelper<T>(this List<T> objectList, string fileName)
        {
            ExportFileViewModel exportFileViewModel = new ExportFileViewModel();

            var csvConfiguration = new CsvConfiguration(new System.Globalization.CultureInfo("en-US"));
            var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(stream: memoryStream, encoding: new UTF8Encoding(true)))
            {
                using (var csvWriter = new CsvWriter(streamWriter, csvConfiguration))
                {
                    csvWriter.WriteRecords(objectList);
                }

            }
            string fileNameWithExtension = $"Company-{fileName}-{DateTime.UtcNow.ToFormatedDateTimeForFileString()}.csv".ToLower();

            exportFileViewModel.FileStream = memoryStream;
            exportFileViewModel.ContentType = "text/csv";
            exportFileViewModel.FileName = fileNameWithExtension;

            return exportFileViewModel;
        }

        public static ExportFileViewModel ExportToCSVFile<T>(this List<T> objectList, string fileName)
        {
            ExportFileViewModel exportFileViewModel = new ExportFileViewModel();
            string excelFormat = GetToCSVFormatValue(objectList);
            exportFileViewModel = ExportToCSVFileByFileStream(excelFormat, fileName);
            return exportFileViewModel;
        }

        private static ExportFileViewModel ExportToCSVFileByFileStream(string excelFormat, string fileName)
        {
            ExportFileViewModel exportFileViewModel = new ExportFileViewModel();

            var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(stream: memoryStream, encoding: new UTF8Encoding(true)))
            {
                streamWriter.Write(excelFormat);

            }
            string fileNameWithExtension = $"Company-{fileName}-{DateTime.UtcNow.ToFormatedDateTimeForFileString()}.csv".ToLower();

            exportFileViewModel.FileStream = memoryStream;
            exportFileViewModel.ContentType = "text/csv";
            exportFileViewModel.FileName = fileNameWithExtension;
            
            return exportFileViewModel;
        }

        private static string GetToCSVFormatValue<T>(this List<T> objectList)
        {
            StringBuilder sb = new StringBuilder();

            //Get the properties for type T for the headers
            PropertyInfo[] propInfos = typeof(T).GetProperties();
            for (int i = 0; i <= propInfos.Length - 1; i++)
            {
                var displayAttribute = propInfos[i].GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                if (displayAttribute != null)
                {
                    string displayName = displayAttribute.Name;
                    sb.Append(displayName);
                }
                else
                {
                    string propName = propInfos[i].Name;
                    sb.Append(propName);
                }

                if (i < propInfos.Length - 1)
                {
                    sb.Append(",");
                }
            }

            sb.AppendLine();

            //Loop through the collection, then the properties and add the values
            for (int i = 0; i <= objectList.Count - 1; i++)
            {
                T item = objectList[i];
                for (int j = 0; j <= propInfos.Length - 1; j++)
                {
                    object o = item.GetType().GetProperty(propInfos[j].Name).GetValue(item, null);
                    if (o != null)
                    {
                        string value = o.ToString();

                        //Check if the value contans a comma and place it in quotes if so
                        if (value.Contains(","))
                        {
                            value = string.Concat("\"", value, "\"");
                        }

                        //Replace any \r or \n special characters from a new line with a space
                        if (value.Contains("\r"))
                        {
                            //value = value.Replace("\r", " ");
                            value = string.Concat("\"", value, "\"");
                        }
                        if (value.Contains("\n"))
                        {
                            //value = value.Replace("\n", " ");
                            value = string.Concat("\"", value, "\"");
                        }
                        if (value.Contains("\t"))
                        {
                            //value = value.Replace("\t", " ");
                            value = string.Concat("\"", value, "\"");
                        }

                        sb.Append(value);
                    }

                    if (j < propInfos.Length - 1)
                    {
                        sb.Append(",");
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
