using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReachingFam.Core.Data;
using ReachingFam.Core.Enums;
using ReachingFam.Core.Interfaces;
using ReachingFam.Core.Models;
using ReachingFam.Core.Models.ReachingFamViewModels;
using ReachingFam.Core.Services;

namespace ReachingFam.Controllers
{
    public class SetupController(
        ILogger<SetupController> logger,
        ApplicationDbContext context,
        CustomIDataProtection protector,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IApprovalService approvalService,
        IResolverService resolverService
            ) : Controller
    {
        private const int DATE_ADD = 1;

        private readonly DateTime startDate = DateTime.Now;
        private readonly DateTime endDate = DateTime.Today.AddMonths(DATE_ADD);

        private readonly ApplicationDbContext _context = context;
        private readonly CustomIDataProtection _protector = protector;
        private readonly ILogger<SetupController> _logger = logger;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IApprovalService _approvalService = approvalService;
        private readonly IResolverService _resolverService = resolverService;

        public async Task<IActionResult> ListDonors()
        {
            return View(await _context.Donors.OrderByDescending(x => x.DonorId).ToListAsync());
        }

        public IActionResult AddDonor()
        {
            return View(new DonorViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDonor([Bind("DonorId,Name,Address,ContactEmail,ContactPhone")] DonorViewModel donorView)
        {
            var user = await _userManager.GetUserAsync(User);

            Donor donor = new()
            {
                Name = donorView.Name,
                Address = donorView.Address,
                ContactEmail = donorView.ContactEmail,
                ContactPhone = donorView.ContactPhone,
                AddedBy = user.UserName,
                DateAdded = DateTime.Now
            };

            try
            {
                if (ModelState.IsValid)
                {
                    await _context.AddAsync(donor);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(ListDonors));
                }
                else
                {
                    ViewBag.Message = "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.";
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error,
                            $"An error has occurred when trying to write into {donor.GetType().Name} table",
                            ex);
            }

            return View(donorView);
        }

        public async Task<IActionResult> EditDonor(string id)
        {
            if (id == null)
            {
                //Not Found
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            int num = _resolverService.ResolveInterger(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var donor = await _context.Donors.FirstOrDefaultAsync(x => x.DonorId == num);
            if (donor == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            DonorViewModel donorView = new()
            {
                DonorId = donor.DonorId,
                Name = donor.Name,
                Address = donor.Address,
                ContactEmail = donor.ContactEmail,
                ContactPhone = donor.ContactPhone,
            };

            return View(donorView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDonor(string id, [Bind("DonorId,Name,Address,ContactEmail,ContactPhone")] DonorViewModel donorView)
        {
            int num = _resolverService.ResolveInterger(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if(num != donorView.DonorId)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            var user = await _userManager.GetUserAsync(User);

            Donor donor = new()
            {
                DonorId = donorView.DonorId,
                Name = donorView.Name,
                Address = donorView.Address,
                ContactEmail = donorView.ContactEmail,
                ContactPhone = donorView.ContactPhone,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            var oldValue = JsonConvert.SerializeObject(await _context.Donors.FirstOrDefaultAsync(x => x.DonorId == donor.DonorId));
            var newValue = JsonConvert.SerializeObject(donor);


            if (await _approvalService.UpdateApprovalQueue(donor.GetType().Name, "Donor", UpdateAction.Update, oldValue, newValue, user.UserName))
            {
                ViewBag.Approval = "Your changes have been forwarded for approval";
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(donorView);
        }

        public async Task<IActionResult> ListPartners()
        {
            return View(await _context.Partners.OrderByDescending(x => x.PartnerId).ToListAsync());
        }

        public IActionResult AddPartner()
        {
            return View(new PartnerViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPartner([Bind("Name,Catergory,Address,ContactEmail,ContactPhone")] PartnerViewModel partnerView)
        {
            var user = await _userManager.GetUserAsync(User);

            Partner partner  = new()
            {
                Name = partnerView.Name,
                Catergory = partnerView.Catergory,
                Address = partnerView.Address,
                ContactEmail = partnerView.ContactEmail,
                ContactPhone = partnerView.ContactPhone,
                AddedBy = user.UserName,
                DateAdded = DateTime.Now
            };

            try
            {
                if (ModelState.IsValid)
                {
                    await _context.AddAsync(partner);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(ListPartners));
                }
                else
                {
                    ViewBag.Message = "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.";
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error,
                            $"An error has occurred when trying to write into {partner.GetType().Name} table",
                            ex);
            }

            return View(partnerView);
        }

        public async Task<IActionResult> EditPartner(string id)
        {
            if (id == null)
            {
                //Not Found
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            int num = _resolverService.ResolveInterger(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var partner = await _context.Partners.FirstOrDefaultAsync(x => x.PartnerId == num);
            if (partner == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            PartnerViewModel partnerView = new()
            {
                PartnerId = partner.PartnerId,
                Name = partner.Name,
                Catergory = partner.Catergory,
                Address = partner.Address,
                ContactEmail = partner.ContactEmail,
                ContactPhone = partner.ContactPhone,
            };

            return View(partnerView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPartner(string id, [Bind("PartnerId,Name,Catergory,Address,ContactEmail,ContactPhone")] PartnerViewModel partnerView)
        {
            int num = _resolverService.ResolveInterger(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != partnerView.PartnerId)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            var user = await _userManager.GetUserAsync(User);

            Partner partner = new()
            {
                PartnerId = partnerView.PartnerId,
                Name = partnerView.Name,
                Catergory = partnerView.Catergory,
                Address = partnerView.Address,
                ContactEmail = partnerView.ContactEmail,
                ContactPhone = partnerView.ContactPhone,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            var oldValue = JsonConvert.SerializeObject(await _context.Partners.FirstOrDefaultAsync(x => x.PartnerId == partner.PartnerId));
            var newValue = JsonConvert.SerializeObject(partner);


            if (await _approvalService.UpdateApprovalQueue(partner.GetType().Name, "Partner", UpdateAction.Update, oldValue, newValue, user.UserName))
            {
                ViewBag.Approval = "Your changes have been forwarded for approval";
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(partnerView);
        }

        public async Task<IActionResult> ListFamilies()
        {
            return View(await _context.Families.OrderByDescending(x => x.FamilyId).ToListAsync());
        }

        public IActionResult AddFamily()
        {
            return View(new FamilyViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFamily([Bind("LastName,OtherNames,Address,Email,Phone,Code")] FamilyViewModel familyView)
        {
            var user = await _userManager.GetUserAsync(User);                        

            Family family = new()
            {
                LastName = familyView.LastName,
                OtherNames = familyView.OtherNames,
                Address = familyView.Address,
                Email = familyView.Email,
                Phone = familyView.Phone,                
                Code = familyView.Code,
                AddedBy = user.UserName,
                DateAdded = DateTime.Now
            };

            try
            {
                if (ModelState.IsValid)
                {
                    await _context.AddAsync(family);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(ListFamilies));
                }
                else
                {
                    ViewBag.Message = "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.";
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex , $"An error has occurred when trying to write into {family.GetType().Name} table");
            }

            return View(familyView);
        }

        public async Task<IActionResult> EditFamily(string id)
        {
            if (id == null)
            {
                //Not Found
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            int num = _resolverService.ResolveInterger(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var family = await _context.Families.FirstOrDefaultAsync(x => x.FamilyId == num);
            if (family == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            FamilyViewModel familyView = new()
            {
                FamilyId = family.FamilyId,
                LastName = family.LastName,
                OtherNames = family.OtherNames,
                Address = family.Address,
                Email = family.Email,
                Phone = family.Phone,
                Code = family.Code,
            };

            return View(familyView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFamily(string id, [Bind("FamilyId,LastName,OtherNames,Address,Email,Phone,Code")] FamilyViewModel familyView)
        {
            int num = _resolverService.ResolveInterger(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != familyView.FamilyId)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            var user = await _userManager.GetUserAsync(User);

            Family family = new()
            {
                FamilyId = familyView.FamilyId,
                LastName = familyView.LastName,
                OtherNames = familyView.OtherNames,
                Address = familyView.Address,
                Email = familyView.Email,
                Phone = familyView.Phone,                
                Code = familyView.Code,
            };

            var oldValue = JsonConvert.SerializeObject(await _context.Families.FirstOrDefaultAsync(x => x.FamilyId == family.FamilyId));
            var newValue = JsonConvert.SerializeObject(family);


            if (await _approvalService.UpdateApprovalQueue(family.GetType().Name, "Family", UpdateAction.Update, oldValue, newValue, user.UserName))
            {
                ViewBag.Approval = "Your changes have been forwarded for approval";
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(familyView);
        }


        private bool DonorExists(int id)
        {
            return _context.Donors.Any(x => x.DonorId == id);
        }

        private bool PartnerExists(int id)
        {
            return _context.Partners.Any(x => x.PartnerId == id);
        }

        private bool FamilyExists(int id)
        {
            return _context.Families.Any(x => x.FamilyId == id);
        }
        
    }
}
