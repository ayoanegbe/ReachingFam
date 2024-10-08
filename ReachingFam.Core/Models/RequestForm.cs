using ReachingFam.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ReachingFam.Core.Models
{
    public class RequestForm
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        public int FormId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public List<string> VoiceTextMessages { get; set; }
        /// <summary>
        /// 1 - Male
        /// 2 - Female
        /// 3 - Non Binary
        /// 4 - No Answer
        /// </summary>
        [Required]
        public GenderType Gender { get; set; }
        [Required]
        public List<string> Disabilities { get; set; }
        [Required]
        public string HearAboutUs { get; set; }
        [Required]
        public int Seniors { get; set; }
        [Required]
        public int Adults { get; set; }
        [Required]
        public int Children { get; set; }
        [Required]
        public bool PeanutButter { get; set; }
        [Required]
        public OatmealType Oatmeal { get; set; }
        [Required]
        public bool AllCannedBeans { get; set; }
        [Required]
        public bool DryBeans { get; set; }
        [Required]
        public string CannedMeat { get; set; }
        [Required]
        public bool CannedTomatoes { get; set; }
        [Required]
        public bool CannedSoup { get; set; }
        [Required]
        public bool CannedVegetables { get; set; }
        [Required]
        public bool PastaSource { get; set; }
        [Required]
        public bool Pasta { get; set; }
        [Required]
        public List<string> Coffees { get; set; }
        [Required]
        public string Tea { get; set; }
        [Required]
        public Flavour CoffeeCream { get; set; }
        [Required]
        public Flavour RequirePeanutFreeSnacks { get; set; }
        [Required]
        public string Condiments { get; set; }
        [Required]
        public string NonDairy { get; set; }
        [Required]
        public bool FreshFruits { get; set; }
        [Required]
        public bool FreshVegetables { get; set; }
        [Required]
        public List<string> CookingItemsAvailable { get; set; }
        [Required]
        public string NonFoodItemsRequest { get; set; }
        [Required]
        public bool OtherFoodResources { get; set; }
        public string Token { get; set; }
    }
}
