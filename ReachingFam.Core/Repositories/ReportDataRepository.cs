
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReachingFam.Core.Data;
using ReachingFam.Core.Enums;
using ReachingFam.Core.Interfaces;
using ReachingFam.Core.Models;
using ReachingFam.Core.Models.ReachingFamViewModels;
using ReachingFam.Core.Services;

namespace ReachingFam.Core.Repositories
{
    public class ReportDataRepository(ILogger<ReportDataRepository> logger, ApplicationDbContext context) : IReportData
    {
        private readonly ILogger<ReportDataRepository> _logger = logger;
        private readonly ApplicationDbContext _context = context;

        public async Task<FullHamperReportViewModel> FullHamperReport()
        {
            var families = await _context.Hampers.Include(x => x.Family).Where(x => x.CollectionDate == DateTime.Today && x.Collected).OrderBy(x => x.CollectionTime).ToListAsync();
            var partners = await _context.PartnerGiveOuts.Include(x => x.Partner).Where(x => x.CollectionDate == DateTime.Today && x.Collected).OrderBy(x => x.CollectionTime).ToListAsync();
            var volunteers = await _context.VolunteerGiveOuts.Include(x => x.User).Where(x => x.CollectionDate == DateTime.Today).OrderBy(x => x.VolunteerGiveOutId).ToListAsync();

            List<DailyCollection> dailyCollections = [];

            foreach (var f in families)
            {
                DailyCollection collection = new()
                {
                    Name = f.Family.FullName,
                    CollectionDate = f.CollectionDate,
                    CollectionTime = f.CollectionTime,
                    Weight = f.Weight,
                    Collected = f.Collected,
                    Source = "Family"
                };

                dailyCollections.Add(collection);
            }

            foreach (var p in partners)
            {
                DailyCollection collection = new()
                {
                    Name = p.Partner.Name,
                    CollectionDate = p.CollectionDate,
                    CollectionTime = p.CollectionTime,
                    Weight = p.Weight,
                    Collected = p.Collected,
                    Source = "Family"
                };

                dailyCollections.Add(collection);
            }

            foreach (var v in volunteers)
            {
                DailyCollection collection = new()
                {
                    Name = v.User.FullName,
                    CollectionDate = v.CollectionDate,
                    Weight = v.Weight,
                    Collected = true,
                    Source = "Volunteer"
                };

                dailyCollections.Add(collection);
            }

            var summary = await SummaryReport(DateTime.Today);

            return new()
            {
                WeightIn = summary.WeightIn,
                WeightOut = summary.WeightOut,
                TotalHamperWeight = summary.FamilyWeight + summary.PartnerWeight,
                DailyCollections = (List<DailyCollection>)dailyCollections.OrderBy(x => x.CollectionTime),
            };

        }

        public async Task<HamperReportViewModel> HampersReport()
        {
            SummaryReportViewModel summary = await SummaryReport(DateTime.Today);

            if (summary != null)
            {
                return new()
                {
                    TotalHamperWeight = summary.FamilyWeight,
                    TotalFamilySize = summary.Families,
                    TotalAdults = summary.Adults,
                    TotalChildren = summary.Children,
                    TotalSeniors = summary.Seniors,
                    HampersCollection = await _context.Hampers.Include(x => x.Family).Where(x => x.CollectionDate == DateTime.Today && x.Collected).ToListAsync(),
                };
            }

            return new();

        }

        public async Task<SummaryReportViewModel> SummaryReport(DateTime date)
        {
            decimal donorWeight = await _context.InwardItems.Where(x => x.CollectionDate == date).SumAsync(x => x.TotalWeight);
            int donorCount = await _context.InwardItems.Where(x => x.CollectionDate == date).CountAsync();

            decimal familyWeight = await _context.Hampers.Where(x => x.CollectionDate == date && x.Collected).SumAsync(x => x.Weight);
            int familyCount = await _context.Hampers.Where(x => x.CollectionDate == date && x.Collected).CountAsync();

            int familySize = await _context.Hampers.Where(x => x.CollectionDate == date && x.Collected).SumAsync(x => x.FamilySize);
            int seniors = (int)await _context.Hampers.Where(x => x.CollectionDate == date && x.Collected).SumAsync(x => x.Seniors);
            int adults = (int)await _context.Hampers.Where(x => x.CollectionDate == date && x.Collected).SumAsync(x => x.Adults);
            int children = (int)await _context.Hampers.Where(x => x.CollectionDate == date && x.Collected).SumAsync(x => x.Children);

            decimal partnerWeight = await _context.PartnerGiveOuts.Where(x => x.CollectionDate == date && x.Collected).SumAsync(x => x.Weight);
            int partnerCount = await _context.PartnerGiveOuts.Where(x => x.CollectionDate == date && x.Collected).CountAsync();

            decimal volunteerWeight = await _context.VolunteerGiveOuts.Where(x => x.CollectionDate == date).SumAsync(x => x.Weight);
            int volunteerCount = await _context.VolunteerGiveOuts.Where(x => x.CollectionDate == date).CountAsync();
            decimal hours = await _context.SignIns.Where(x => x.Date == date).SumAsync(x => x.HoursWorked);

            decimal waste = await _context.Wastes.Where(x => x.Date == date).SumAsync(x => x.Weight);

            if (donorWeight == 0 && familyWeight == 0 && partnerWeight == 0 && volunteerWeight == 0)
                return null;

            return new()
            {
                WeightIn = donorWeight,
                Donors = donorCount,
                FamilyWeight = familyWeight,
                Families = familySize,
                Seniors = seniors,
                Adults = adults,
                Children = children,
                PartnerWeight = partnerWeight,
                Partners = partnerCount,
                VolunteerWeight = volunteerWeight,
                Volunteers = volunteerCount,
                Hours = hours,
                Waste = waste,
                WeightOut = familyWeight + partnerWeight + volunteerWeight + waste,
                Date = date.ToString("dd-MMM"),
            };
        }

        public async Task<List<SummaryReportViewModel>> SummaryReport(AnalyticPeriod period)
        {
            var date = DateTime.Today;

            _ = DateTime.Today;
            _ = DateTime.Today;
            DateTime endDate;
            DateTime startDate;

            switch (period)
            {
                case AnalyticPeriod.ThisMonth:
                    startDate = new DateTime(date.Year, date.Month, 1);
                    endDate = date;                                        
                    return await MonthlyReportData(startDate, endDate);
                case AnalyticPeriod.ThisYear:
                    return await YearlyReportData(date.Year);
                case AnalyticPeriod.ThisWeek:
                    startDate = Utils.GetDateofFOW();
                    endDate = date;
                    return await WeeklyReportData(startDate, endDate);
                case AnalyticPeriod.LastMonth:
                    startDate = new DateTime(date.Year, date.Month - 1, 1);
                    endDate = startDate.AddMonths(1).AddSeconds(-1);
                    return await MonthlyReportData(startDate, endDate);
            }

            return [];
        }

        public async Task<List<SummaryReportViewModel>> MonthlyReportData(DateTime startDate, DateTime endDate)
        {
            List<SummaryReportViewModel> monthlyReport = [];

            int year = startDate.Year;
            int month = endDate.Month;

            int lastDay = endDate.Day;

            for (int day = 1; day <= lastDay; day++)
            {
                DateTime date = new(year, month, day);

                SummaryReportViewModel dailyReport = await SummaryReport(date);

                if (dailyReport != null)
                    monthlyReport.Add(dailyReport);
            }

            return monthlyReport;
        }

        public async Task<List<SummaryReportViewModel>> WeeklyReportData(DateTime startDate, DateTime endDate)
        {
            List<SummaryReportViewModel> weeklyReport = [];

            for (var day = startDate.Date; day.Date <= endDate.Date; day = day.AddDays(1))
            {
                SummaryReportViewModel dailyReport = await SummaryReport(day);
                if (dailyReport != null)
                {
                    dailyReport.Date = day.ToString("ddd");
                    weeklyReport.Add(dailyReport);
                }
            }

            return weeklyReport;
        }

        public async Task<List<SummaryReportViewModel>> YearlyReportData(int year)
        {
            List<SummaryReportViewModel> yearlyReport = [];

            DateTime date = DateTime.Today;

            for (int month = 1; month <= date.Month; month++)
            {
                DateTime startDate = new(year, month, 1);
                DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);

                List<SummaryReportViewModel> monthlyReport = await MonthlyReportData(startDate, endDate);

                if (monthlyReport != null)
                {
                    SummaryReportViewModel montlySummary = new()
                    {
                        WeightIn = monthlyReport.Sum(x => x.WeightIn),
                        WeightOut = monthlyReport.Sum(x => x.WeightOut),
                        FamilyWeight = monthlyReport.Sum(x => x.FamilyWeight),
                        Families = monthlyReport.Sum(y => y.Families),
                        Seniors = monthlyReport.Sum(x => x.Seniors),
                        Adults = monthlyReport.Sum(x => x.Adults),
                        Children = monthlyReport.Sum(x => x.Children),
                        Partners = monthlyReport.Sum(x => x.Partners),
                        PartnerWeight = monthlyReport.Sum(x => x.PartnerWeight),
                        Volunteers = monthlyReport.Sum(x => x.Volunteers),
                        VolunteerWeight = monthlyReport.Sum(x => x.VolunteerWeight),
                        Hours = monthlyReport.Sum(x => x.Hours),
                        Waste = monthlyReport.Sum(x => x.Waste),
                        Date = startDate.ToString("MMM")
                    };

                    yearlyReport.Add(montlySummary);
                }
                
            }

            return yearlyReport;
        }

        public FinalReportViewModel SummarizeReportData(List<SummaryReportViewModel> summaryReports)
        {
            return new()
            {
                WeightIn = summaryReports.Sum(x => x.WeightIn),
                WeightOut = summaryReports.Sum(x => x.WeightOut),
                FamilyWeight = summaryReports.Sum(x => x.FamilyWeight),
                Families = summaryReports.Sum(y => y.Families),
                Seniors = summaryReports.Sum(x => x.Seniors),
                Adults = summaryReports.Sum(x => x.Adults),
                Children = summaryReports.Sum(x => x.Children),
                Partners = summaryReports.Sum(x => x.Partners),
                PartnerWeight = summaryReports.Sum(x => x.PartnerWeight),
                Volunteers = summaryReports.Sum(x => x.Volunteers),
                VolunteerWeight = summaryReports.Sum(x => x.VolunteerWeight),
                Hours = summaryReports.Sum(x => x.Hours),
                Waste = summaryReports.Sum(x => x.Waste),
                FinalReports = summaryReports,
            };
        }

        public async Task<DashboardViewModel> DashboardFilter(DateTime startDate, DateTime endDate)
        {
            return new()
            {
                FamilyCount = await _context.Hampers.Where(x => x.DateAdded >= startDate || x.DateAdded <= endDate).SumAsync(x => x.FamilySize),
                SeniorCount = (int)await _context.Hampers.Where(x => x.DateAdded >= startDate || x.DateAdded <= endDate).SumAsync(x => x.Seniors),
                AdultCount = (int)await _context.Hampers.Where(x => x.DateAdded >= startDate || x.DateAdded <= endDate).SumAsync(x => x.Adults),
                ChildrenCount = (int)await _context.Hampers.Where(x => x.DateAdded >= startDate || x.DateAdded <= endDate).SumAsync(x => x.Children),
                TotalHoursWorked = await _context.SignIns.Where(x => x.Date >= startDate || x.Date <= endDate).SumAsync(x => x.HoursWorked),
                TotalWeightIn = await _context.InwardItems.Where(x => x.CollectionDate >= startDate || x.CollectionDate <= endDate).SumAsync(x => x.TotalWeight),
                FamilyWeight = await _context.Hampers.Where(x => x.CollectionDate >= startDate || x.CollectionDate <= endDate).SumAsync(x => x.Weight),
                PartnerWeight = await _context.PartnerGiveOuts.Where(x => x.CollectionDate >= startDate || x.CollectionDate <= endDate).SumAsync(x => x.Weight),
                VolunteerWeight = await _context.VolunteerGiveOuts.Where(x => x.CollectionDate >= startDate || x.CollectionDate <= endDate).SumAsync(x => x.Weight),
            };
        }

        public CurrentPreviousDashboardViewModel ProcessDashboardFilter(CurrentPreviousDashboardViewModel dashboardViewModel)
        {
            var processedDashboard = new CurrentPreviousDashboardViewModel()
            {
                Current = dashboardViewModel.Current,
                Previous = dashboardViewModel.Previous,
                Period = dashboardViewModel.Period,
                FooditemsBelowReorderLevel = dashboardViewModel.FooditemsBelowReorderLevel,
                FamilyHampersForCollection = dashboardViewModel.FamilyHampersForCollection,
                FamilyHampersCollected = dashboardViewModel.FamilyHampersCollected,
                FamilyHampersNotCollected = dashboardViewModel.FamilyHampersNotCollected,
                PartnerHampersForCollection = dashboardViewModel.PartnerHampersForCollection,
                PartnerHampersCollected = dashboardViewModel.PartnerHampersCollected,
                PartnerHampersNotCollected = dashboardViewModel.PartnerHampersNotCollected,
                FamilyCountPercentage = dashboardViewModel.Previous.FamilyCount > 0 ? (dashboardViewModel.Previous.FamilyCount - dashboardViewModel.Current.FamilyCount) / dashboardViewModel.Previous.FamilyCount * 100 : 0,
                FamilyWeightPercentage = dashboardViewModel.Previous.FamilyWeight > 0 ? (dashboardViewModel.Previous.FamilyWeight - dashboardViewModel.Current.FamilyWeight) / dashboardViewModel.Previous.FamilyWeight * 100 : 0,
                SeniorCountPercentage = dashboardViewModel.Previous.SeniorCount > 0 ? (dashboardViewModel.Previous.SeniorCount - dashboardViewModel.Current.SeniorCount) / dashboardViewModel.Previous.SeniorCount * 100 : 0,
                AdultCountPercentage = dashboardViewModel.Previous.AdultCount > 0 ? (dashboardViewModel.Previous.AdultCount - dashboardViewModel.Current.AdultCount) / dashboardViewModel.Previous.AdultCount * 100 : 0,
                ChildrenCountPercentage = dashboardViewModel.Previous.ChildrenCount > 0 ? (dashboardViewModel.Previous.ChildrenCount - dashboardViewModel.Current.ChildrenCount) / dashboardViewModel.Previous.ChildrenCount * 100 : 0,
                TotalHoursWorkedPercentage = dashboardViewModel.Previous.TotalHoursWorked > 0 ? (dashboardViewModel.Previous.TotalHoursWorked - dashboardViewModel.Current.TotalHoursWorked) / dashboardViewModel.Previous.TotalHoursWorked * 100 : 0,
                TotalWeightInPercentage = dashboardViewModel.Previous.TotalWeightIn > 0 ? (dashboardViewModel.Previous.TotalWeightIn - dashboardViewModel.Current.TotalWeightIn) / dashboardViewModel.Previous.TotalWeightIn * 100 : 0,
                TotalWeigthOutPercentage = dashboardViewModel.Previous.TotalWeightOut > 0 ? (dashboardViewModel.Previous.TotalWeightOut - dashboardViewModel.Current.TotalWeightOut) / dashboardViewModel.Previous.TotalWeightOut * 100 : 0,
                PartnerWeightPercentage = dashboardViewModel.Previous.PartnerWeight > 0 ? (dashboardViewModel.Previous.PartnerWeight - dashboardViewModel.Current.PartnerWeight) / dashboardViewModel.Previous.PartnerWeight * 100 : 0,
                VolunteerWeightPercentage = dashboardViewModel.Previous.VolunteerWeight > 0 ? (dashboardViewModel.Previous.VolunteerWeight - dashboardViewModel.Current.VolunteerWeight) / dashboardViewModel.Previous.VolunteerWeight * 100 : 0,
                TotalWeightDistributed = dashboardViewModel.Current.TotalWeightIn + dashboardViewModel.Current.TotalWeightIn,
                TotalEmergencyHampers = dashboardViewModel.Current.FamilyWeight + dashboardViewModel.Current.VolunteerWeight,
            };

            return processedDashboard;
        }

        public async Task<long> FamilyHampersForCollection()
        {
            return await _context.Hampers.Include(x => x.Family).Where(x => x.CollectionDate == DateTime.Today).CountAsync();
        }

        public async Task<long> FamilyHampersCollected()
        {
            return await _context.Hampers.Include(x => x.Family).Where(x => x.CollectionDate == DateTime.Today && x.Collected).CountAsync();
        }

        public async Task<long> FamilyHampersNotCollected()
        {
            return await _context.Hampers.Include(x => x.Family).Where(x => x.CollectionDate == DateTime.Today && !x.Collected).CountAsync();
        }

        public async Task<long> PartnerHampersForCollection()
        {
            return await _context.PartnerGiveOuts.Include(x => x.Partner).Where(x => x.CollectionDate == DateTime.Today).CountAsync();
        }

        public async Task<long> PartnerHampersCollected()
        {
            return await _context.PartnerGiveOuts.Include(x => x.Partner).Where(x => x.CollectionDate == DateTime.Today && x.Collected).CountAsync();
        }

        public async Task<long> PartnerHampersNotCollected()
        {
            return await _context.PartnerGiveOuts.Include(x => x.Partner).Where(x => x.CollectionDate == DateTime.Today && !x.Collected).CountAsync();
        }
    }
}
