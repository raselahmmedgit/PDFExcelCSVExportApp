using CsvHelper;
using CsvHelper.Configuration;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using DNTCaptcha.Core;
using lab.PDFExcelCSVExportApp.Helpers;
using lab.PDFExcelCSVExportApp.Models;
using lab.PDFExcelCSVExportApp.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab.PDFExcelCSVExportApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDNTCaptchaValidatorService _validatorService;
        private readonly IDNTCaptchaApiProvider _apiProvider;
        private readonly IWebHostEnvironment _iWebHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IDNTCaptchaValidatorService validatorService, IDNTCaptchaApiProvider apiProvider, IWebHostEnvironment iWebHostEnvironment)
        {
            _logger = logger;
            _validatorService = validatorService;
            _apiProvider = apiProvider;
            _iWebHostEnvironment = iWebHostEnvironment;
        }

        public IActionResult Index()
        {
            var notificationSendDate = ConvertNotificationSendDate8To18();

            return View();
        }

        private DateTime ConvertNotificationSendDate8To18()
        {
            try
            {
                DateTime dateTime = DateTime.UtcNow;

                var dateTimeUtc = dateTime.AddDays(Convert.ToInt32(5));
                var dateTimeUtcToCst = ConvertToUSACSTDateTime(dateTimeUtc);
                int startValue = 8;
                int endValue = 18;
                TimeSpan startTimeSpan = new TimeSpan(startValue, 0, 0); //8 o'clock
                TimeSpan endTimeSpan = new TimeSpan(endValue, 0, 0); //18 o'clock
                TimeSpan dateTimeUtcToCstTimeSpan = dateTimeUtcToCst.TimeOfDay;

                if ((dateTimeUtcToCstTimeSpan > startTimeSpan) && (dateTimeUtcToCstTimeSpan < endTimeSpan))
                {
                    //match found
                    dateTime = ConvertToUtcDateTime(dateTimeUtc);
                }
                else
                {
                    //do not match found
                    Random random = new Random();
                    int randomValue = random.Next(startValue, endValue);
                    TimeSpan randomTimeSpan = new TimeSpan(randomValue, 0, 0);

                    var dateTimeUtcToCstRandom = dateTimeUtcToCst.Date + randomTimeSpan;
                    dateTime = ConvertToUtcDateTime(dateTimeUtcToCstRandom);
                }

                return dateTime;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DateTime ConvertToUtcDateTime(DateTime dateTime)
        {
            if (dateTime != default(DateTime))
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
                DateTime time = TimeZoneInfo.ConvertTimeToUtc(dateTime);
                return time;
            }
            else
            {
                return DateTime.UtcNow;
            }
        }

        private DateTime ConvertToUSACSTDateTime(DateTime dateTime)
        {
            if (dateTime != default(DateTime))
            {
                var CSTDateTime = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dateTime), TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
                return CSTDateTime;
            }
            else
            {
                return DateTime.UtcNow;
            }
        }

        [HttpGet]
        [Route("/Home/GetDataTableAjax")]
        [ResponseCache(NoStore = true, Duration = 0)]
        public IActionResult GetDataTableAjax(IDataTablesRequest request)
        {
            try
            {
                DataTablesResponse response = GetDataTablesResponseAsync(request);
                return new DataTablesJsonResult(response, true);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [ActionName("ExportCsv")]
        public IActionResult ExportCsv()
        {
            try
            {
                var productViewModeleList = new List<ProductViewModel>() {
                    new ProductViewModel{ Id = 1, Name = "A" },
                    new ProductViewModel{ Id = 2, Name = "B" },
                    new ProductViewModel{ Id = 3, Name = "C" }
                };

                string fileName = "Product List";
                var exportFileViewModel = ExportCsvFileHelper.ExportToCSVFile(productViewModeleList, fileName);

                return File(exportFileViewModel.FileStream.ToArray(), exportFileViewModel.ContentType, exportFileViewModel.FileName);

            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public IActionResult DNTCaptcha()
        {
            var productViewModel = new ProductViewModel();
            productViewModel.Id = 1;
            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(
            ErrorMessage = "Please enter the security code as a number.",
            CaptchaGeneratorLanguage = Language.English,
            CaptchaGeneratorDisplayMode = DisplayMode.SumOfTwoNumbersToWords)]
        public IActionResult DNTCaptcha(ProductViewModel model)
        {
            try
            {
                //if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.SumOfTwoNumbersToWords))
                //{
                //    model.Message = "Please enter the security code as a number.";
                //    return View(model);
                //}

                model.Message = "Successfully.";
                return View(model);
            }
            catch (Exception)
            {
                model.Message = "Failed.";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult DNTCaptchaPartial()
        {
            try
            {
                return PartialView("_DNTCaptchaPartial");
            }
            catch (Exception)
            {
                return null;
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private DataTablesResponse GetDataTablesResponseAsync(IDataTablesRequest request)
        {
            try
            {
                var viewModelList = new List<ProductViewModel>() {
                    new ProductViewModel{ Id = 1, Name = "A" },
                    new ProductViewModel{ Id = 2, Name = "B" },
                    new ProductViewModel{ Id = 3, Name = "C" }
                };

                // Global filtering.
                // Filter is being manually applied due to in-memmory (IEnumerable) data.
                // If you want something rather easier, check IEnumerableExtensions Sample.

                int dataCount = viewModelList.Count();
                int filteredDataCount = 0;
                IEnumerable<ProductViewModel> dataPage;
                if (viewModelList.Count() > 0 && request != null)
                {
                    var filteredData = viewModelList;

                    dataCount = filteredData.Count();

                    // Paging filtered data.
                    // Paging is rather manual due to in-memmory (IEnumerable) data.
                    dataPage = filteredData.Skip(request.Start).Take(request.Length);

                    filteredDataCount = filteredData.Count();
                }
                else
                {
                    var filteredData = viewModelList;

                    dataCount = filteredData.Count();

                    dataPage = filteredData;

                    filteredDataCount = filteredData.Count();
                }

                // Response creation. To create your response you need to reference your request, to avoid
                // request/response tampering and to ensure response will be correctly created.
                var response = DataTablesResponse.Create(request, dataCount, filteredDataCount, dataPage);

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("/Home/DNTCaptchaData")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
        public ActionResult<DNTCaptchaApiResponse> DNTCaptchaData()
        {
            try
            {
                // Note: For security reasons, a JavaScript client shouldn't be able to provide these attributes directly.
                // Otherwise an attacker will be able to change them and make them easier!
                return _apiProvider.CreateDNTCaptcha(new DNTCaptchaTagHelperHtmlAttributes
                {
                    Max = 30,
                    Min = 1,
                    Language = Language.English,
                    DisplayMode = DisplayMode.SumOfTwoNumbersToWords,
                    ValidationErrorMessage = "Please enter the security code as a number.",
                    FontName = "Tahoma",
                    FontSize = 24,
                    ForeColor = "#333333",
                    BackColor = "#CCCCCC",
                    UseNoise = true,
                    UseRelativeUrls = true
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult AppCaptcha()
        {
            var productViewModel = new ProductViewModel();
            productViewModel.Id = 1;
            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AppCaptcha(ProductViewModel model)
        {
            try
            {
                if (!CaptchaHelper.IsValidCaptcha(model.CaptchaToken, model.CaptchaId, model.CaptchaText))
                {
                    model.Message = "Please enter the security code as a number.";
                    return View(model);
                }

                model.Message = "Successfully.";
                return View(model);
            }
            catch (Exception)
            {
                model.Message = "Failed.";
                return View(model);
            }
        }

        [HttpGet]
        [Route("/Home/AppCaptchaData")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
        public IActionResult AppCaptchaData()
        {
            try
            {
                var captchaData = CaptchaHelper.GenerateCaptcha();
                return new JsonResult(captchaData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult QrBarCodeFromPdf()
        {
            var result = new Result();
            try
            {
                //IFormFile file = Request.Form.Files[0];
                string folderName = "upload";
                string webRootPath = _iWebHostEnvironment.WebRootPath;
                string uploadFolderPath = Path.Combine(webRootPath, folderName);

                StringBuilder sb = new StringBuilder();
                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                string fileName = "Bloom Richard_5925 Seaside Drive_C020_2889_170001_1_20211130_130730.pdf";
                string fileFullPath = Path.Combine(uploadFolderPath, fileName);

                string fileExtension = Path.GetExtension(fileFullPath).ToLower();

                if (fileExtension == ".pdf")
                {
                    result = QrBarCodeHelper.QrBarCodeFromPdf(uploadFolderPath, fileFullPath);
                }

            }
            catch (Exception ex)
            {
                result = Result.Fail();
            }

            var json = new { success = result.Success, message = result.Message, messagetype = result.MessageType, data = result.Data };
            return new JsonResult(json);
        }
    }
}
