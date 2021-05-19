using CsvHelper;
using CsvHelper.Configuration;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using lab.PDFExcelCSVExportApp.Helpers;
using lab.PDFExcelCSVExportApp.Models;
using lab.PDFExcelCSVExportApp.ViewModels;
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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/Home/GetDataAsync")]
        [ResponseCache(NoStore = true, Duration = 0)]
        public IActionResult GetDataAsync(IDataTablesRequest request)
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
    }
}
