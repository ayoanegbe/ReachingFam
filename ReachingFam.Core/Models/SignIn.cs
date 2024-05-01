using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class SignIn
    {
        [Key]
        public int SignInId { get; set; }
        public string UserId { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;
        [Display(Name = "Clock In")]
        public DateTime ClockIn { get; set; }
        [Display(Name = "Clock Out")]
        public DateTime? ClockOut { get; set; }
        [Display(Name = "Hours Worked")]
        public double HoursWorked { get; set; }
    }
}
