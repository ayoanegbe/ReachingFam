using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReachingFam.Core.Data;
using ReachingFam.Core.Enums;
using ReachingFam.Core.Interfaces;
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
        public async Task<IActionResult> SummaryReport(AnalyticPeriod Period)
        {
            return View(_reportData.SummarizeReportData(await _reportData.SummaryReport(Period)));
        }
    }
}
