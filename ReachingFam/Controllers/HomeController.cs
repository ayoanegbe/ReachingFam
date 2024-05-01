using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using ReachingFam.Core.Data;
using ReachingFam.Core.Enums;
using ReachingFam.Core.Interfaces;
using ReachingFam.Core.Models;
using ReachingFam.Core.Models.ReachingFamViewModels;
using ReachingFam.Core.Services;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReachingFam.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private const int DATE_DEDUCT = -29;

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly CustomIDataProtection _protector;
        private readonly IReportData _reportData;

        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext context,
            IWebHostEnvironment environment,
            CustomIDataProtection protector,
            IReportData reportData
            )
        {
            _logger = logger;
            _context = context;
            _environment = environment;
            _protector = protector;
            _reportData = reportData;
        }

        public async Task<IActionResult> Index()
        {
            var date = DateTime.Today;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);

            return View(await _reportData.DashboardFilter(firstDayOfMonth, date));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AnalyticPeriod period)
        {
            var date = DateTime.Today;
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            switch (period)
            {
                case AnalyticPeriod.ThisMonth:
                    startDate = new DateTime(date.Year, date.Month, 1);
                    endDate = date;
                    break;
                case AnalyticPeriod.ThisYear:
                    startDate = new DateTime(date.Year, 1, 1);
                    endDate = date;
                    break;
                case AnalyticPeriod.ThisWeek:
                    startDate = Utils.GetDateofFOW();
                    endDate = date;
                    break;
                case AnalyticPeriod.LastMonth:
                    startDate = new DateTime(date.Year, date.Month - 1, 1);
                    endDate = startDate.AddMonths(1).AddSeconds(-1);
                    break;
            }

            return View(await _reportData.DashboardFilter(startDate, endDate));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
