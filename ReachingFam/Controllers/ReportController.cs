using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReachingFam.Core.Data;
using ReachingFam.Core.Enums;
using ReachingFam.Core.Interfaces;
using ReachingFam.Core.Models.ReachingFamViewModels;
using ReachingFam.Core.Services;

namespace ReachingFam.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly ILogger<ReportController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly CustomIDataProtection _protector;
        private readonly IReportData _reportData;

        public ReportController(ILogger<ReportController> logger,
            ApplicationDbContext context,
            CustomIDataProtection protector,
            IReportData reportData
            )
        {
            _logger = logger;
            _context = context;
            _protector = protector;
            _reportData = reportData;
        }

        public async Task<IActionResult> Report()
        {
            var donorSummary = await _context.InwardItems.SumAsync(x => x.TotalWeight);
            return View();
        }

        public async Task<IActionResult> HampersReport()
        {
            return View(await _reportData.HampersReport());
        }

        public async Task<IActionResult> FullHamperReport()
        {
            return View(await _reportData.FullHamperReport());
        }

        public async Task<IActionResult> ThisWeekReport()
        {
            return View(_reportData.SummarizeReportData(await _reportData.SummaryReport(AnalyticPeriod.ThisWeek)));
        }

        public async Task<IActionResult> ThisMonthReport()
        {
            return View(_reportData.SummarizeReportData(await _reportData.SummaryReport(AnalyticPeriod.ThisMonth)));
        }

        public async Task<IActionResult> LastMonthReport()
        {
            return View(_reportData.SummarizeReportData(await _reportData.SummaryReport(AnalyticPeriod.LastMonth)));
        }

        public async Task<IActionResult> ThisYearReport()
        {
            return View(_reportData.SummarizeReportData(await _reportData.SummaryReport(AnalyticPeriod.ThisYear)));
        }

        [HttpGet]
        public async Task<IActionResult> SummaryReport(AnalyticPeriod period)
        {
            var summary = _reportData.SummarizeReportData(await _reportData.SummaryReport(period));
            summary.Period = period;

            return View(summary);
        }

        public async Task<IActionResult> GeneralReport(AnalyticPeriod period)
        {
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
                Volunteers = [1, 3, 1, 4, 4, 9, 5, 8, 4, 9, 5, 8],
                Donors = [20, 29, 37, 36, 44, 45, 50, 58, 58, 60, 62, 65],
                FamilyHampersWeight = [520.22, 629.34, 937.34, 4336.66, 944.73, 845.22, 950.34, 458.23, 3258.52, 2260.52, 2262.43, 3465.52],
                PartnerHamperWeight = [20.22, 29.34, 37.34, 36.66, 44.73, 45.22, 50.34, 58.23, 58.52, 60.52, 62.43, 65.52],
                VolunteerHamperWeight = [20.22, 29.34, 37.34, 36.66, 44.73, 45.22, 50.34, 58.23, 58.52, 60.52, 62.43, 65.52],
                WeightIn = [20.22, 29.34, 37.34, 36.66, 44.73, 45.22, 50.34, 58.23, 58.52, 60.52, 62.43, 65.52],
                WeightOut = [20.22, 29.34, 37.34, 36.66, 44.73, 45.22, 50.34, 58.23, 58.52, 60.52, 62.43, 65.52],
                Hours = [20.22, 29.34, 37.34, 36.66, 44.73, 45.22, 50.34, 58.23, 58.52, 60.52, 62.43, 65.52],
                Wastes = [20.22, 29.34, 37.34, 36.66, 44.73, 45.22, 50.34, 58.23, 58.52, 60.52, 62.43, 65.52],
                Categories = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                Period = period
            };

            return View(graphView);
        }
    }
}
