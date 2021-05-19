using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace lab.PDFExcelCSVExportApp.ViewModel
{
    public class ExportFileViewModel
    {
        public MemoryStream FileStream { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
