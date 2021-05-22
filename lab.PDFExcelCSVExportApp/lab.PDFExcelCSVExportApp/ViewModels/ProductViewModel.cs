using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace lab.PDFExcelCSVExportApp.ViewModels
{
    public class ProductViewModel
    {
        [Display(Name = "Product Id")]
        public int Id { get; set; }
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}
