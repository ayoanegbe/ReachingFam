using ReachingFam.Core.Enums;
using ReachingFam.Core.Models.ReachingFamViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Interfaces
{
    public interface IReportData
    {
        Task<SummaryReportViewModel> SummaryReport(DateTime date);
        Task<List<SummaryReportViewModel>> SummaryReport(AnalyticPeriod period);
        Task<List<SummaryReportViewModel>> MonthlyReportData(DateTime startDate, DateTime endDate);
        Task<List<SummaryReportViewModel>> YearlyReportData(int year);
        Task<List<SummaryReportViewModel>> WeeklyReportData(DateTime startDate, DateTime endDate);
        Task<FullHamperReportViewModel> FullHamperReport();
        Task<HamperReportViewModel> HampersReport();
        FinalReportViewModel SummarizeReportData(List<SummaryReportViewModel> summaryReports);
        Task<DashboardViewModel> DashboardFilter(DateTime startDate, DateTime endDate);
    }
}
