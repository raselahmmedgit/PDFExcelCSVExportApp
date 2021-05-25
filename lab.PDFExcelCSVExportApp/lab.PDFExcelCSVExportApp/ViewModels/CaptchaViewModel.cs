using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace lab.PDFExcelCSVExportApp.ViewModels
{
    public class CaptchaViewModel
    {
        public string CaptchaId { get; set; }
        public string CaptchaCode { get; set; }
        public string CaptchaImage { get; set; }
        [Required(ErrorMessage = "Captcha Token is required.")]
        public string CaptchaToken { get; set; }
        [Required(ErrorMessage = "Captcha Text is required.")]
        public string CaptchaText { get; set; }
    }

    public interface ICaptchaViewModel
    {
        string CaptchaId { get; set; }
        string CaptchaCode { get; set; }
        string CaptchaImage { get; set; }
        string CaptchaToken { get; set; }
        string CaptchaText { get; set; }
    }
}
