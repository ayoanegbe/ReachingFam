using ReachingFam.Core.Enums;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class GraphViewModel
    {
        public List<int> Families { get; set; }
        public List<int> Partners { get; set; }
        public List<int> Donors { get; set; }
        public List<int> Volunteers { get; set; }
        public List<double> FamilyHampersWeight { get; set; }
        public List<double> PartnerHamperWeight { get; set; }
        public List<double> VolunteerHamperWeight { get; set; }
        public List<string> Categories { get; set; }
        public List<double> WeightIn { get; set; }
        public List<double> WeightOut { get; set; }
        public List<double> Hours { get; set; }
        public List<double> Wastes { get; set; }
        public AnalyticPeriod Period { get; set; }
    }
}
