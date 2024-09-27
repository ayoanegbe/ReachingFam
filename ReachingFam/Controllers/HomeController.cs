using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReachingFam.Core.Data;
using ReachingFam.Core.Enums;
using ReachingFam.Core.Interfaces;
using ReachingFam.Core.Models;
using ReachingFam.Core.Models.ReachingFamViewModels;
using ReachingFam.Core.Services;
using System.Collections.Generic;
using System.Diagnostics;

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
        private readonly IStockService _stockService;

        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext context,
            IWebHostEnvironment environment,
            CustomIDataProtection protector,
            IReportData reportData,
            IStockService stockService
            )
        {
            _logger = logger;
            _context = context;
            _environment = environment;
            _protector = protector;
            _reportData = reportData;
            _stockService = stockService;
        }

        public async Task<IActionResult> Index()
        {
            var date = DateTime.Today;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);

            DateTime previousStartDate = new(date.Year, date.Month - 1, 1);
            DateTime previousEndDate = previousStartDate.AddMonths(1).AddSeconds(-1);

            var fullDashboardData = new CurrentPreviousDashboardViewModel()
            {
                Current = await _reportData.DashboardFilter(firstDayOfMonth, date),
                Previous = await _reportData.DashboardFilter(previousStartDate, previousEndDate),
                Period = AnalyticPeriod.ThisMonth,
                FooditemsBelowReorderLevel = await _stockService.GetFooditemsBelowReorderLevel(),
                FamilyHampersForCollection = await _reportData.FamilyHampersForCollection(),
                FamilyHampersCollected = await _reportData.FamilyHampersCollected(),
                FamilyHampersNotCollected = await _reportData.FamilyHampersNotCollected(),
                PartnerHampersForCollection = await _reportData.PartnerHampersForCollection(),
                PartnerHampersCollected = await _reportData.PartnerHampersCollected(),
                PartnerHampersNotCollected = await _reportData.PartnerHampersNotCollected()
            };

            var filter = _reportData.ProcessDashboardFilter(fullDashboardData);

            List<SummaryReportViewModel> reportedData = await _reportData.SummaryReport(AnalyticPeriod.ThisMonth);

            //GraphViewModel graphView = new()
            //{
            //    FamilyHampers = reportedData.Select(x => x.Families).ToList(),
            //    PartnerHampers = reportedData.Select(x => x.Partners).ToList(),
            //    Donors = reportedData.Select(y => y.Donors).ToList(),
            //    Categories = reportedData.Select(k => k.Date).ToList()
            //};

            GraphViewModel graphView = new()
            {
                Families = [4, 2, 5, 5, 5, 8, 8, 6, 5, 8, 8, 6],
                Partners = [1, 3, 1, 4, 4, 9, 5, 8, 4, 9, 5, 8],
                Donors = [20, 29, 37, 36, 44, 45, 50, 58, 58, 60, 62, 65],
                Categories = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]
            };

            filter.GraphView = graphView;

            return View(filter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AnalyticPeriod period)
        {
            var date = DateTime.Today;
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;
            DateTime previousStartDate = DateTime.Today;
            DateTime previousEndDate = DateTime.Today;

            switch (period)
            {
                case AnalyticPeriod.ThisMonth:
                    startDate = new DateTime(date.Year, date.Month, 1);
                    endDate = date;

                    previousStartDate = new DateTime(date.Year, date.Month - 1, 1);
                    previousEndDate = previousStartDate.AddMonths(1).AddSeconds(-1);

                    break;
                case AnalyticPeriod.ThisYear:
                    startDate = new DateTime(date.Year, 1, 1);
                    endDate = date;

                    previousStartDate = new DateTime(date.Year -1, 1, 1);
                    previousEndDate = startDate.AddSeconds(-1);

                    break;
                case AnalyticPeriod.ThisWeek:
                    startDate = Utils.GetDateofFOW();
                    endDate = date;

                    (previousStartDate, previousEndDate) = Utils.PreviousWeek();

                    break;
                case AnalyticPeriod.LastMonth:
                    startDate = new DateTime(date.Year, date.Month - 1, 1);
                    endDate = startDate.AddMonths(1).AddSeconds(-1);

                    previousStartDate = new DateTime(date.Year, date.Month - 2, 1);
                    previousEndDate = previousStartDate.AddMonths(1).AddSeconds(-1);

                    break;
            }

            var fullDashboardData = new CurrentPreviousDashboardViewModel()
            {
                Current = await _reportData.DashboardFilter(startDate, endDate),
                Previous = await _reportData.DashboardFilter(previousStartDate, previousEndDate),
                Period = period,
                FooditemsBelowReorderLevel = await _stockService.GetFooditemsBelowReorderLevel()
            };

            var filter = _reportData.ProcessDashboardFilter(fullDashboardData);

            List<SummaryReportViewModel> reportedData = await _reportData.SummaryReport(period);

            //GraphViewModel graphView = new()
            //{
            //    FamilyHampers = reportedData.Select(x => x.Families).ToList(),
            //    PartnerHampers = reportedData.Select(x => x.Partners).ToList(),
            //    Donors = reportedData.Select(y => y.Donors).ToList(),
            //    Categories = reportedData.Select(k => k.Date).ToList()
            //};

            GraphViewModel graphView = new()
            {
                Families = [4, 2, 5, 5, 5, 8, 8, 6, 5, 8, 8, 6],
                Partners = [1, 3, 1, 4, 4, 9, 5, 8, 4, 9, 5, 8],
                Donors = [20, 29, 37, 36, 44, 45, 50, 58, 58, 60, 62, 65],
                Categories = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]
            };

            filter.GraphView = graphView;

            return View(filter);
        }

        //public async Task<ActionResult> GetGraphData(int period)
        //{

        //    List<SummaryReportViewModel> reportedData = await _reportData.SummaryReport((AnalyticPeriod)period);

        //    //GraphViewModel graphView = new()
        //    //{
        //    //    FamilyHampers = (List<long>)reportedData.Select(x => x.Families),
        //    //    PartnerHampers = (List<long>)reportedData.Select(x => x.Partners),
        //    //    Donors = (List<long>)reportedData.Select(y => y.Donors),
        //    //    Categories = (List<string>)reportedData.Select(k => k.Date)
        //    //};

        //    GraphViewModel graphView = new()
        //    {
        //        FamilyHampers = ["4", "2", "5", "5", "5", "8", "8", "6", "5", "8", "8", "6"],
        //        PartnerHampers = ["1", "3", "1", "4", "4", "9", "5", "8", "4", "9", "5", "8"],
        //        Donors = ["20", "29", "37", "36", "44", "45", "50", "58", "58", "60", "62", "65"],
        //        Categories = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]
        //    };

        //    return Ok(graphView);
        //}

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
