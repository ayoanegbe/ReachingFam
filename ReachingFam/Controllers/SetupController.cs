using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    [Authorize]
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
            ViewData["ReturnUrl"] = HttpContext.Request.Path;
            //string timeZoneId = HttpContext.Request.Cookies["userTimeZoneId"];

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

        public async Task<IActionResult> EditDonor(string id, string returnUrl = null)
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

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
            }

            ViewData["ReturnUrl"] = retUrl;

            return View(donorView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDonor(string id, [Bind("DonorId,Name,Address,ContactEmail,ContactPhone")] DonorViewModel donorView, string returnUrl = null)
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

            //string retUrl = string.Empty;
            //if (returnUrl != null)
            //{
            //    retUrl = _resolverService.ResolveString(returnUrl);
            //}

            if (await _approvalService.UpdateApprovalQueue(donor.GetType().Name, "Donor", UpdateAction.Update, oldValue, newValue, user.UserName))
            {
                return RedirectToAction(nameof(Confirmation), new { url = returnUrl });
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
            ViewData["ReturnUrl"] = HttpContext.Request.Path;

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

        public async Task<IActionResult> EditPartner(string id, string returnUrl = null)
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

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
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

            ViewData["ReturnUrl"] = retUrl;

            return View(partnerView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPartner(string id, [Bind("PartnerId,Name,Catergory,Address,ContactEmail,ContactPhone")] PartnerViewModel partnerView, string returnUrl = null)
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

            //string retUrl = string.Empty;
            //if (returnUrl != null)
            //{
            //    retUrl = _resolverService.ResolveString(returnUrl);
            //}

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
                return RedirectToAction(nameof(Confirmation), new { url = returnUrl });
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(partnerView);
        }

        public IActionResult Confirmation(string url)
        {
            string retUrl = string.Empty;
            if (url != null)
            {
                retUrl = _resolverService.ResolveString(url);
            }

            ViewData["Approval"] = "Your changes have been forwarded for approval";
            ViewData["ReturnUrl"] = retUrl;

            return View();
        }

        public async Task<IActionResult> ListFamilies()
        {
            ViewData["ReturnUrl"] = HttpContext.Request.Path;

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

        public async Task<IActionResult> EditFamily(string id, string returnUrl = null)
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

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
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

            ViewData["ReturnUrl"] = retUrl;

            return View(familyView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFamily(string id, [Bind("FamilyId,LastName,OtherNames,Address,Email,Phone,Code")] FamilyViewModel familyView, string returnUrl = null)
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

            //string retUrl = string.Empty;
            //if (returnUrl != null)
            //{
            //    retUrl = _resolverService.ResolveString(returnUrl);
            //}

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
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            var oldValue = JsonConvert.SerializeObject(await _context.Families.FirstOrDefaultAsync(x => x.FamilyId == family.FamilyId));
            var newValue = JsonConvert.SerializeObject(family);

            if (await _approvalService.UpdateApprovalQueue(family.GetType().Name, "Family", UpdateAction.Update, oldValue, newValue, user.UserName))
            {
                return RedirectToAction(nameof(Confirmation), new { url = returnUrl });
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(familyView);
        }        

        public async Task<IActionResult> ListUOMs()
        {
            ViewData["ReturnUrl"] = HttpContext.Request.Path;

            return View(await _context.UnitOfMeasures.ToListAsync());
        }

        public IActionResult AddUOM()
        {
            return View(new UnitOfMeasureViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUOM([Bind("Name,Symbol,Description")] UnitOfMeasureViewModel unitOfMeasureView)
        {
            var user = await _userManager.GetUserAsync(User);

            UnitOfMeasure unitOfMeasure = new()
            {
                Name = unitOfMeasureView.Name,
                Symbol = unitOfMeasureView.Symbol,
                Description = unitOfMeasureView.Description,
                AddedBy = user.UserName,
                DateAdded = DateTime.Now
            };

            try
            {
                if (ModelState.IsValid)
                {
                    await _context.AddAsync(unitOfMeasure);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(ListUOMs));
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
                _logger.Log(LogLevel.Error, ex, $"An error has occurred when trying to write into {unitOfMeasure.GetType().Name} table");
            }

            return View(unitOfMeasureView);
        }

        public async Task<IActionResult> EditUOM(string id, string returnUrl = null)
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

            var unitOfMeasure = await _context.UnitOfMeasures.FirstOrDefaultAsync(x => x.UnitOfMeasureId == num);
            if (unitOfMeasure == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
            }

            UnitOfMeasureViewModel unitOfMeasureView = new()
            {
                UnitOfMeasureId = num,
                Name = unitOfMeasure.Name,
                Symbol = unitOfMeasure.Symbol,
                Description = unitOfMeasure.Description,    
            };

            ViewData["ReturnUrl"] = retUrl;

            return View(unitOfMeasureView);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUOM(string id, [Bind("UnitOfMeasureId,Name,Symbol,Description")] UnitOfMeasureViewModel unitOfMeasureView, string returnUrl = null)
        {
            int num = _resolverService.ResolveInterger(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != unitOfMeasureView.UnitOfMeasureId)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            var user = await _userManager.GetUserAsync(User);

            UnitOfMeasure unitOfMeasure = new()
            {
                UnitOfMeasureId = unitOfMeasureView.UnitOfMeasureId,
                Name = unitOfMeasureView.Name,
                Symbol = unitOfMeasureView.Symbol,
                Description = unitOfMeasureView.Description,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            var oldValue = JsonConvert.SerializeObject(await _context.UnitOfMeasures.FirstOrDefaultAsync(x => x.UnitOfMeasureId == unitOfMeasure.UnitOfMeasureId));
            var newValue = JsonConvert.SerializeObject(unitOfMeasure);

            if (await _approvalService.UpdateApprovalQueue(unitOfMeasure.GetType().Name, "UnitOfMeasure", UpdateAction.Update, oldValue, newValue, user.UserName))
            {
                return RedirectToAction(nameof(Confirmation), new { url = returnUrl });
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(unitOfMeasureView);
        }

        public async Task<IActionResult> ListOptionTypes()
        {
            ViewData["ReturnUrl"] = HttpContext.Request.Path;
            //string timeZoneId = HttpContext.Request.Cookies["userTimeZoneId"];

            return View(await _context.OptionTypes.OrderByDescending(x => x.OptionTypeId).ToListAsync());
        }

        public IActionResult AddOptionType() {  return View(new OptionTypeViewModel()); }

        public async Task<IActionResult> AddOptionType([Bind("Name")] OptionTypeViewModel optionTypeView)
        {
            var user = await _userManager.GetUserAsync(User);

            OptionType optionType = new()
            {
                Name = optionTypeView.Name,
                AddedBy = user.UserName,
                DateAdded = DateTime.Now
            };

            try
            {
                if (ModelState.IsValid)
                {
                    await _context.AddAsync(optionType);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(ListOptionTypes));
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
                            $"An error has occurred when trying to write into {optionType.GetType().Name} table",
                            ex);
            }

            return View(optionTypeView);
        }

        public async Task<IActionResult> EditOptionType(string id, string returnUrl = null)
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

            var optionType = await _context.OptionTypes.FirstOrDefaultAsync(x => x.OptionTypeId == num);
            if (optionType == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            OptionTypeViewModel optionTypeView = new()
            {
                Name = optionType.Name,
            };

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
            }

            ViewData["ReturnUrl"] = retUrl;

            return View(optionTypeView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOptionType(string id, [Bind("OptionTypeId,Name")] OptionTypeViewModel optionTypeView, string returnUrl = null)
        {
            int num = _resolverService.ResolveInterger(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != optionTypeView.OptionTypeId)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            var user = await _userManager.GetUserAsync(User);

            OptionType optionType = new()
            {
                OptionTypeId = optionTypeView.OptionTypeId,
                Name = optionTypeView.Name,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            var oldValue = JsonConvert.SerializeObject(await _context.OptionTypes.FirstOrDefaultAsync(x => x.OptionTypeId == optionType.OptionTypeId));
            var newValue = JsonConvert.SerializeObject(optionType);

            if (await _approvalService.UpdateApprovalQueue(optionType.GetType().Name, "OptionType", UpdateAction.Update, oldValue, newValue, user.UserName))
            {
                return RedirectToAction(nameof(Confirmation), new { url = returnUrl });
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(optionTypeView);
        }

        public async Task<IActionResult> ListOptionValues()
        {
            ViewData["ReturnUrl"] = HttpContext.Request.Path;

            return View(await _context.OptionValues.Include(x => x.OptionType).OrderByDescending(x => x.OptionValueId).ToListAsync());
        }

        public IActionResult AddOptionValue() 
        {
            ViewData["OptionTypeId"] = new SelectList(_context.OptionTypes, "OptionTypeId", "Name");
            return View(new OptionTypeViewModel()); 
        }

        public async Task<IActionResult> AddOptionValue([Bind("OptionTypeId,Name")] OptionValueViewModel optionValueView)
        {
            var user = await _userManager.GetUserAsync(User);

            OptionValue optionValue = new()
            {
                OptionTypeId = optionValueView.OptionTypeId,
                Name = optionValueView.Name,
                AddedBy = user.UserName,
                DateAdded = DateTime.Now
            };

            try
            {
                if (ModelState.IsValid)
                {
                    await _context.AddAsync(optionValue);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(ListOptionValues));
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
                            $"An error has occurred when trying to write into {optionValue.GetType().Name} table",
                            ex);
            }

            ViewData["OptionTypeId"] = new SelectList(_context.OptionTypes, "OptionTypeId", "Name", optionValueView.OptionValueId);

            return View(optionValueView);
        }

        public async Task<IActionResult> EditOptionValue(string id, string returnUrl = null)
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

            var optionValue = await _context.OptionValues.FirstOrDefaultAsync(x => x.OptionValueId == num);
            if (optionValue == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            OptionValueViewModel optionValueView = new()
            {
                OptionValueId = optionValue.OptionValueId,
                OptionTypeId = optionValue.OptionTypeId,
                Name = optionValue.Name,
            };

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
            }

            ViewData["ReturnUrl"] = retUrl;

            ViewData["OptionTypeId"] = new SelectList(_context.OptionTypes, "OptionTypeId", "Name", optionValueView.OptionValueId);

            return View(optionValueView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOptionValue(string id, [Bind("OptionValueID,OptionTypeID,Name")] OptionValueViewModel optionValueView, string returnUrl = null)
        {
            int num = _resolverService.ResolveInterger(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != optionValueView.OptionValueId)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            var user = await _userManager.GetUserAsync(User);

            OptionValue optionValue = new()
            {
                OptionValueId = optionValueView.OptionTypeId,
                OptionTypeId = optionValueView.OptionTypeId,
                Name = optionValueView.Name,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            var oldValue = JsonConvert.SerializeObject(await _context.OptionValues.FirstOrDefaultAsync(x => x.OptionValueId == optionValue.OptionValueId));
            var newValue = JsonConvert.SerializeObject(optionValue);

            if (await _approvalService.UpdateApprovalQueue(optionValue.GetType().Name, "OptionValue", UpdateAction.Update, oldValue, newValue, user.UserName))
            {
                return RedirectToAction(nameof(Confirmation), new { url = returnUrl });
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["OptionTypeId"] = new SelectList(_context.OptionTypes, "OptionTypeId", "Name", optionValueView.OptionValueId);

            return View(optionValueView);
        }
    }

}
