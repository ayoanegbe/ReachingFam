using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReachingFam.Core.Data;
using ReachingFam.Core.Enums;
using ReachingFam.Core.Interfaces;
using ReachingFam.Core.Models;
using ReachingFam.Core.Models.DataViewModels;
using ReachingFam.Core.Models.ReachingFamViewModels;
using ReachingFam.Core.Services;
using System.Linq;
using System.Transactions;

namespace ReachingFam.Controllers
{
    [Authorize]
    public class ProcessManagementController(
        ILogger<SetupController> logger,
        ApplicationDbContext context,
        CustomIDataProtection protector,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IApprovalService approvalService,
        IFileService fileService,
        IResolverService resolverService,
        IStockLevelRepository stockLevelRepository,
        IUnitOfWork unitOfWork,
        IGenericRepository<Hamper> familyHamperRepository,
        IGenericRepository<HamperItem> familyHamperItemRepository
            ) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly CustomIDataProtection _protector = protector;
        private readonly ILogger<SetupController> _logger = logger;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IApprovalService _approvalService = approvalService;
        private readonly IFileService _fileService = fileService;
        private readonly IResolverService _resolverService = resolverService;
        private readonly IStockLevelRepository _stockLevelRepository = stockLevelRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IGenericRepository<Hamper> _familyHamperRepository = familyHamperRepository;
        private readonly IGenericRepository<HamperItem> _familyHamperItemRepository = familyHamperItemRepository;

        public async Task<IActionResult> InwardItemsList()
        {
            ViewData["ReturnUrl"] = HttpContext.Request.Path;

            return View(await _context.InwardItems.Include(x => x.Donor).OrderByDescending(x => x.InwardItemId).ToListAsync());
        }

        public IActionResult AddInwardItem()
        {
            ViewData["DonorId"] = new SelectList(_context.Donors, "DonorId", "Name");

            return View(new InwardItemViewModel() { CollectionDate = DateTime.Now} );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInwardItem([Bind("DonorId,CollectionDate,TotalWeight,NonPerishables,NonPerishablesWeight,Perishables,PerishablesWeight,Frozen,FrozenWeight,NonFood,NonFoodWeight,File")] InwardItemViewModel inwardItemView)
        {
            var user = await _userManager.GetUserAsync(User);

            string filePath = string.Empty;
            string message = string.Empty;
            string thumbnailPath = string.Empty;

            (filePath, thumbnailPath, message) = await _fileService.ProcessFile(inwardItemView.File, "images", "donors");

            if (message != "Success")
            {
                ViewBag.Message = message;
                return View(inwardItemView);
            }
           
            var totalWeight = inwardItemView.NonPerishablesWeight + inwardItemView.PerishablesWeight + inwardItemView.FrozenWeight + inwardItemView.NonFoodWeight;

            InwardItem inwardItem = new()
            {
                DonorId = inwardItemView.DonorId,
                CollectionDate = inwardItemView.CollectionDate,
                TotalWeight = (decimal)totalWeight,
                NonPerishables = inwardItemView.NonPerishables,
                NonPerishablesWeight = inwardItemView.NonPerishablesWeight,
                Perishables = inwardItemView.Perishables,
                PerishablesWeight = inwardItemView.PerishablesWeight,
                Frozen = inwardItemView.Frozen,
                FrozenWeight = inwardItemView.FrozenWeight,
                NonFood = inwardItemView.NonFood,
                NonFoodWeight = inwardItemView.NonFoodWeight,
                AddedBy = user.UserName,
                DateAdded = DateTime.Now,
                FilePath = filePath,
                ThumbnailPath = thumbnailPath,
            };

            try
            {
                if (ModelState.IsValid)
                {
                    await _context.AddAsync(inwardItem);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(InwardItemsList));
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
                _logger.Log(LogLevel.Error, ex, $"An error has occurred when trying to write into {inwardItem.GetType().Name} table");
            }

            ViewData["DonorId"] = new SelectList(_context.Donors, "DonorId", "Name");

            return View(inwardItemView);
        }

        public async Task<IActionResult> EditInwardItem(string id, string returnUrl = null)
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

            var inwardItem = await _context.InwardItems.Include(x => x.Donor).FirstOrDefaultAsync(x => x.InwardItemId == num);
            if (inwardItem == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            InwardItemViewModel inwardItemView = new()
            {
                InwardItemId = inwardItem.InwardItemId,
                DonorId = inwardItem.DonorId,
                CollectionDate = inwardItem.CollectionDate,
                TotalWeight = inwardItem.TotalWeight,
                NonPerishables = inwardItem.NonPerishables,
                NonPerishablesWeight = inwardItem.NonPerishablesWeight,
                Perishables = inwardItem.Perishables,
                PerishablesWeight = inwardItem.PerishablesWeight,
                Frozen = inwardItem.Frozen,
                FrozenWeight = inwardItem.FrozenWeight,
                NonFood = inwardItem.NonFood,
                NonFoodWeight = inwardItem.NonFoodWeight,
            };

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
            }

            ViewData["ReturnUrl"] = retUrl;

            ViewData["DonorId"] = new SelectList(_context.Donors, "DonorId", "Name", inwardItem.InwardItemId);

            return View(inwardItemView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInwardItem(string id, [Bind("InwardItemId,DonorId,CollectionDate,TotalWeight,NonPerishables,NonPerishablesWeight,Perishables,PerishablesWeight,Frozen,FrozenWeight,NonFood,NonFoodWeight")] InwardItemViewModel inwardItemView, string returnUrl = null)
        {
            int num = _resolverService.ResolveInterger(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != inwardItemView.InwardItemId)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            var user = await _userManager.GetUserAsync(User);

            var totalWeight = inwardItemView.NonPerishablesWeight + inwardItemView.PerishablesWeight + inwardItemView.FrozenWeight + inwardItemView.NonFoodWeight;

            InwardItem inwardItem = new()
            {
                InwardItemId = inwardItemView.InwardItemId,
                DonorId = inwardItemView.DonorId,
                CollectionDate = inwardItemView.CollectionDate,
                TotalWeight = (decimal)totalWeight,
                NonPerishables = inwardItemView.NonPerishables,
                NonPerishablesWeight = inwardItemView.NonPerishablesWeight,
                Perishables = inwardItemView.Perishables,
                PerishablesWeight = inwardItemView.PerishablesWeight,
                Frozen = inwardItemView.Frozen,
                FrozenWeight = inwardItemView.FrozenWeight,
                NonFood = inwardItemView.NonFood,
                NonFoodWeight = inwardItemView.NonFoodWeight,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            var oldValue = JsonConvert.SerializeObject(await _context.InwardItems.FirstOrDefaultAsync(x => x.InwardItemId == inwardItem.InwardItemId));
            var newValue = JsonConvert.SerializeObject(inwardItem);

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
            }

            if (await _approvalService.UpdateApprovalQueue(inwardItem.GetType().Name, "Inward Item", UpdateAction.Update, oldValue, newValue, user.UserName))
            {
                return RedirectToAction(nameof(Confirmation), new { url = retUrl });
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["DonorId"] = new SelectList(_context.Donors, "DonorId", "Name", inwardItem.InwardItemId);

            return View(inwardItemView);
        }

        public async Task<IActionResult> FamilyHamperList()
        {
            ViewData["ReturnUrl"] = HttpContext.Request.Path;

            return View(await _context.Hampers.Include(x => x.Family).OrderByDescending(x => x.HamperId).ToListAsync());
        }

        public IActionResult AddFamilyHamper()
        {
            ViewData["FamilyId"] = new SelectList(_context.Families, "FamilyId", "FullName");

            return View( new HamperViewModel() { CollectionDate = DateTime.Today, CollectionTime = DateTime.Now});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFamilyHamper([Bind("FamilyId,CollectionDate,CollectionTime,Weight,NonPerishables,NonPerishablesWeight,Perishables,PerishablesWeight,Frozen,FrozenWeight,NonFood,NonFoodWeight,FamilySize,Seniors,Adults,Children,Collected")] HamperViewModel hamperView)
        {
            var user = await _userManager.GetUserAsync(User);

            string filePath = string.Empty;
            string message = string.Empty;
            string thumbnailPath = string.Empty;

            (filePath, thumbnailPath, message) = await _fileService.ProcessFile(hamperView.File, "images", "donors");

            if (message != "Success")
            {
                ViewBag.Message = message;
                return View(hamperView);
            }

            try
            {
                int familySize = (int)(hamperView.Seniors + hamperView.Adults + hamperView.Children);
                if (familySize != hamperView.FamilySize)
                {
                    ViewBag.Message = "Inconsistencies in family size. Please verify";
                    return View(hamperView);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Inconsistencies in family size. Please verify";
                _logger.Log(LogLevel.Error, ex, "Inconsistencies in family size");
                return View(hamperView);
            }

            var totalWeight = hamperView.NonPerishablesWeight + hamperView.PerishablesWeight + hamperView.FrozenWeight + hamperView.NonFoodWeight;

            Hamper hamper = new()
            {
                FamilyId = hamperView.FamilyId,
                CollectionDate = hamperView.CollectionDate,
                CollectionTime = hamperView.CollectionTime,
                Weight = (decimal)totalWeight,
                NonPerishables = hamperView.NonPerishables,
                NonPerishablesWeight = hamperView.NonPerishablesWeight,
                Perishables = hamperView.Perishables,
                PerishablesWeight = hamperView.PerishablesWeight,
                Frozen = hamperView.Frozen,
                FrozenWeight = hamperView.FrozenWeight,
                NonFood = hamperView.NonFood,
                NonFoodWeight = hamperView.NonFoodWeight,
                FamilySize = hamperView.FamilySize,
                Seniors = hamperView.Seniors,
                Adults = hamperView.Adults,
                Children = hamperView.Children,
                Collected = hamperView.Collected,
                AddedBy = user.UserName,
                DateAdded = DateTime.Now,
                FilePath = filePath,
                ThumbnailPath = thumbnailPath,
            };

            try
            {
                if (ModelState.IsValid)
                {
                    await _context.AddAsync(hamper);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(AddFamilyHamperItem), new { id = _protector.Encode(hamper.HamperId.ToString()) });
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
                _logger.Log(LogLevel.Error, ex, $"An error has occurred when trying to write into {hamper.GetType().Name} table");
            }

            ViewData["FamilyId"] = new SelectList(_context.Families, "FamilyId", "FullName");

            return View(hamperView);
        }

        public async Task<IActionResult> AddFamilyHamperItem(string id)
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

            var hamper = await _context.Hampers.Include(x => x.Family).FirstOrDefaultAsync(x => x.HamperId == num);
            if (hamper == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            HamperViewModel hamperView = new()
            {
                HamperId = hamper.HamperId,
                FamilyId = hamper.FamilyId,
                CollectionDate = hamper.CollectionDate,
                CollectionTime = hamper.CollectionTime,
                Weight = hamper.Weight,
                NonPerishables = hamper.NonPerishables,
                NonPerishablesWeight = hamper.NonPerishablesWeight,
                Perishables = hamper.Perishables,
                PerishablesWeight = hamper.PerishablesWeight,
                Frozen = hamper.Frozen,
                FrozenWeight = hamper.FrozenWeight,
                NonFood = hamper.NonFood,
                NonFoodWeight = hamper.NonFoodWeight,
                FamilySize = hamper.FamilySize,
                Seniors = hamper.Seniors,
                Adults = hamper.Adults,
                Children = hamper.Children,
                Collected = hamper.Collected,
                HamperItems = []
            };

            ViewData["FamilyId"] = new SelectList(_context.Families, "FamilyId", "FullName", hamperView.HamperId);

            return View(hamperView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFamilyHamperItem(string id, [Bind("HamperId,HamperItems")] HamperViewModel hamperView)
        {
            int num = _resolverService.ResolveInterger(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != hamperView.HamperId)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            var user = await _userManager.GetUserAsync(User);

            //using (var scope = new TransactionScope(TransactionScopeOption.Required,
            //    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            //{

            //}

            try
            {
                foreach (var item in hamperView.HamperItems)
                {
                    HamperItem hamperItem = new()
                    {
                        HamperId = hamperView.HamperId,
                        FoodItemId = item.FoodItemId,
                        Quantity = item.Quantity,
                        Note = item.Note,
                        UpdatedBy = user.UserName,
                        DateUpdated = DateTime.Now
                    };

                    await _familyHamperItemRepository.AddAsync(hamperItem);
                }

                await _unitOfWork.SaveChangesAsync();

                return RedirectToAction(nameof(FamilyHamperList));

            }
            catch (Exception ex)
            {

                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";

                _logger.Log(LogLevel.Error, ex, $"Unable to add family hamper items: {JsonConvert.SerializeObject(hamperView)}");

            }

            ViewData["FamilyId"] = new SelectList(_context.Families, "FamilyId", "FullName", hamperView.HamperId);

            return View(hamperView);
        }

        public async Task<IActionResult> EditFamilyHamper(string id, string returnUrl = null)
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

            var hamper = await _context.Hampers.Include(x => x.Family).FirstOrDefaultAsync(x => x.HamperId == num);
            if (hamper == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            HamperViewModel hamperView = new()
            {
                HamperId = hamper.HamperId,
                FamilyId = hamper.FamilyId,
                CollectionDate = hamper.CollectionDate,
                CollectionTime = hamper.CollectionTime,
                Weight = hamper.Weight,
                NonPerishables = hamper.NonPerishables,
                NonPerishablesWeight = hamper.NonPerishablesWeight,
                Perishables = hamper.Perishables,
                PerishablesWeight = hamper.PerishablesWeight,
                Frozen = hamper.Frozen,
                FrozenWeight = hamper.FrozenWeight,
                NonFood = hamper.NonFood,
                NonFoodWeight = hamper.NonFoodWeight,
                FamilySize = hamper.FamilySize,
                Seniors = hamper.Seniors,
                Adults = hamper.Adults,
                Children = hamper.Children,
                Collected = hamper.Collected
            };

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
            }

            ViewData["ReturnUrl"] = retUrl;

            ViewData["FamilyId"] = new SelectList(_context.Families, "FamilyId", "FullName", hamper.HamperId);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFamilyHamper(string id, [Bind("HamperId,FamilyId,CollectionDate,CollectionTime,Weight,NonPerishables,NonPerishablesWeight,Perishables,PerishablesWeight,Frozen,FrozenWeight,NonFood,NonFoodWeight,FamilySize,Seniors,Adults,Children,Collected")] HamperViewModel hamperView, string returnUrl = null)
        {
            int num = _resolverService.ResolveInterger(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != hamperView.HamperId)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            try
            {
                int familySize = (int)(hamperView.Seniors + hamperView.Adults + hamperView.Children);
                if (familySize != hamperView.FamilySize)
                {
                    ViewBag.Message = "Inconsistencies in family size. Please verify";
                    return View(hamperView);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Inconsistencies in family size. Please verify";
                _logger.Log(LogLevel.Error, ex, "Inconsistencies in family size");
                return View(hamperView);
            }

            var user = await _userManager.GetUserAsync(User);

            var totalWeight = hamperView.NonPerishablesWeight + hamperView.PerishablesWeight + hamperView.FrozenWeight + hamperView.NonFoodWeight;

            Hamper hamper = new()
            {
                HamperId = hamperView.HamperId,
                FamilyId = hamperView.FamilyId,
                CollectionDate = hamperView.CollectionDate,
                CollectionTime = hamperView.CollectionTime,
                Weight = (decimal)totalWeight,
                NonPerishables = hamperView.NonPerishables,
                NonPerishablesWeight = hamperView.NonPerishablesWeight,
                Perishables = hamperView.Perishables,
                PerishablesWeight = hamperView.PerishablesWeight,
                Frozen = hamperView.Frozen,
                FrozenWeight = hamperView.FrozenWeight,
                NonFood = hamperView.NonFood,
                NonFoodWeight = hamperView.NonFoodWeight,
                FamilySize = hamperView.FamilySize,
                Seniors = hamperView.Seniors,
                Adults = hamperView.Adults,
                Children = hamperView.Children,
                Collected = hamperView.Collected,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            var oldValue = JsonConvert.SerializeObject(await _context.Hampers.FirstOrDefaultAsync(x => x.HamperId == hamper.HamperId));
            var newValue = JsonConvert.SerializeObject(hamper);

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
            }

            if (await _approvalService.UpdateApprovalQueue(hamper.GetType().Name, "Family Hamper", UpdateAction.Update, oldValue, newValue, user.UserName))
            {
                return RedirectToAction(nameof(Confirmation), new { url = retUrl });
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["FamilyId"] = new SelectList(_context.Families, "FamilyId", "FullName", hamper.HamperId);

            return View(hamperView);
        }

        public async Task<IActionResult> PartnerGiveOutList()
        {
            ViewData["ReturnUrl"] = HttpContext.Request.Path;

            return View(await _context.PartnerGiveOuts.Include(x => x.Partner).OrderByDescending(x => x.PartnerGiveOutId).ToListAsync());
        }

        public IActionResult AddPartnerGiveOut()
        {
            ViewData["PartnerId"] = new SelectList(_context.Partners, "PartnerId", "Name");

            return View( new PartnerGiveOutViewModel() { CollectionDate = DateTime.Today, CollectionTime = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPartnerGiveOut([Bind("PartnerId,CollectionDate,CollectionTime,Weight,NonPerishables,NonPerishablesWeight,Perishables,PerishablesWeight,Frozen,FrozenWeight,NonFood,NonFoodWeight,Collected,File")] PartnerGiveOutViewModel partnerGiveOutView)
        {
            var user = await _userManager.GetUserAsync(User);

            string filePath = string.Empty;
            string message = string.Empty;
            string thumbnailPath = string.Empty;

            (filePath, thumbnailPath, message) = await _fileService.ProcessFile(partnerGiveOutView.File, "images", "donors");

            if (message != "Success")
            {
                ViewBag.Message = message;
                return View(partnerGiveOutView);
            }

            var totalWeight = partnerGiveOutView.NonPerishablesWeight + partnerGiveOutView.PerishablesWeight + partnerGiveOutView.FrozenWeight + partnerGiveOutView.NonFoodWeight;

            PartnerGiveOut partnerGiveOut = new()
            {
                PartnerId = partnerGiveOutView.PartnerId,
                CollectionDate = partnerGiveOutView.CollectionDate,
                CollectionTime = partnerGiveOutView.CollectionTime,
                Weight = (decimal)totalWeight,
                NonPerishables = partnerGiveOutView.NonPerishables,
                NonPerishablesWeight = partnerGiveOutView.NonPerishablesWeight,
                Perishables = partnerGiveOutView.Perishables,
                PerishablesWeight = partnerGiveOutView.PerishablesWeight,
                Frozen = partnerGiveOutView.Frozen,
                FrozenWeight = partnerGiveOutView.FrozenWeight,
                NonFood = partnerGiveOutView.NonFood,
                NonFoodWeight = partnerGiveOutView.NonFoodWeight,
                Collected = partnerGiveOutView.Collected,
                AddedBy = user.UserName,
                DateAdded = DateTime.Now,
                FilePath = filePath,
                ThumbnailPath = thumbnailPath,
            };

            try
            {
                if (ModelState.IsValid)
                {
                    await _context.AddAsync(partnerGiveOut);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(PartnerGiveOutList));
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
                _logger.Log(LogLevel.Error, ex, $"An error has occurred when trying to write into {partnerGiveOut.GetType().Name} table");
            }

            ViewData["PartnerId"] = new SelectList(_context.Partners, "PartnerId", "Name");

            return View(partnerGiveOutView);
        }

        public async Task<IActionResult> EditPartnerGiveOut(string id, string returnUrl = null)
        {
            if(id == null)
            {
                //Not Found
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            int num = _resolverService.ResolveInterger(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var partnerGiveOut = await _context.PartnerGiveOuts.Include(x => x.Partner).FirstOrDefaultAsync(x => x.PartnerGiveOutId == num);
            if (partnerGiveOut == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            PartnerGiveOutViewModel partnerGiveOutView = new()
            {
                PartnerGiveOutId = partnerGiveOut.PartnerGiveOutId,
                PartnerId = partnerGiveOut.PartnerId,
                CollectionDate = partnerGiveOut.CollectionDate,
                CollectionTime = partnerGiveOut.CollectionTime,
                Weight = partnerGiveOut.Weight,
                NonPerishables = partnerGiveOut.NonPerishables,
                NonPerishablesWeight = partnerGiveOut.NonPerishablesWeight,
                Perishables = partnerGiveOut.Perishables,
                PerishablesWeight = partnerGiveOut.PerishablesWeight,
                Frozen = partnerGiveOut.Frozen,
                FrozenWeight = partnerGiveOut.FrozenWeight,
                NonFood = partnerGiveOut.NonFood,
                NonFoodWeight = partnerGiveOut.NonFoodWeight,
                Collected = partnerGiveOut.Collected
            };

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
            }

            ViewData["ReturnUrl"] = retUrl;

            ViewData["PartnerId"] = new SelectList(_context.Partners, "PartnerId", "Name", partnerGiveOutView.PartnerGiveOutId);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPartnerGiveOut(string id, [Bind("PartnerGiveOutId,PartnerId,CollectionDate,CollectionTime,Weight,NonPerishables,NonPerishablesWeight,Perishables,PerishablesWeight,Frozen,FrozenWeight,NonFood,NonFoodWeight,Collected")] PartnerGiveOutViewModel partnerGiveOutView, string returnUrl = null)
        {
            int num = _resolverService.ResolveInterger(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != partnerGiveOutView.PartnerGiveOutId)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            var user = await _userManager.GetUserAsync(User);

            var totalWeight = partnerGiveOutView.NonPerishablesWeight + partnerGiveOutView.PerishablesWeight + partnerGiveOutView.FrozenWeight + partnerGiveOutView.NonFoodWeight;

            PartnerGiveOut partnerGiveOut = new()
            {
                PartnerGiveOutId = partnerGiveOutView.PartnerGiveOutId,
                PartnerId = partnerGiveOutView.PartnerId,
                CollectionDate = partnerGiveOutView.CollectionDate,
                CollectionTime = partnerGiveOutView.CollectionTime,
                Weight = (decimal)totalWeight,
                NonPerishables = partnerGiveOutView.NonPerishables,
                NonPerishablesWeight = partnerGiveOutView.NonPerishablesWeight,
                Perishables = partnerGiveOutView.Perishables,
                PerishablesWeight = partnerGiveOutView.PerishablesWeight,
                Frozen = partnerGiveOutView.Frozen,
                FrozenWeight = partnerGiveOutView.FrozenWeight,
                NonFood = partnerGiveOutView.NonFood,
                NonFoodWeight = partnerGiveOutView.NonFoodWeight,
                Collected = partnerGiveOutView.Collected,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            var oldValue = JsonConvert.SerializeObject(await _context.PartnerGiveOuts.FirstOrDefaultAsync(x => x.PartnerGiveOutId == partnerGiveOut.PartnerGiveOutId));
            var newValue = JsonConvert.SerializeObject(partnerGiveOut);

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
            }

            if (await _approvalService.UpdateApprovalQueue(partnerGiveOut.GetType().Name, "Partner Hamper", UpdateAction.Update, oldValue, newValue, user.UserName))
            {
                return RedirectToAction(nameof(Confirmation), new { url = retUrl });
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["PartnerId"] = new SelectList(_context.Partners, "PartnerId", "Name", partnerGiveOut.PartnerId);

            return View(partnerGiveOutView);
        }

        public async Task<IActionResult> VolunteerGiveOutList()
        {
            ViewData["ReturnUrl"] = HttpContext.Request.Path;

            return View(await _context.VolunteerGiveOuts.Include(x => x.User).OrderByDescending(x => x.VolunteerGiveOutId).ToListAsync());
        }

        public IActionResult AddVolunteerGiveOut()
        {
            ViewData["Email"] = new SelectList(_userManager.Users, "Email", "FullName");

            return View( new VolunteerGiveOutViewModel() { CollectionDate = DateTime.Now});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVolunteerGiveOut([Bind("Email,CollectionDate,Weight,NonPerishables,NonPerishablesWeight,Perishables,PerishablesWeight,Frozen,FrozenWeight,NonFood,NonFoodWeight")] VolunteerGiveOutViewModel volunteerGiveOutView)
        {
            var user = await _userManager.GetUserAsync(User);

            var totalWeight = volunteerGiveOutView.NonPerishablesWeight + volunteerGiveOutView.PerishablesWeight + volunteerGiveOutView.FrozenWeight + volunteerGiveOutView.NonFoodWeight;

            VolunteerGiveOut volunteerGiveOut = new()
            {
                Email = volunteerGiveOutView.Email,
                CollectionDate = volunteerGiveOutView.CollectionDate,
                Weight = (decimal)totalWeight,
                NonPerishables = volunteerGiveOutView.NonPerishables,
                NonPerishablesWeight = volunteerGiveOutView.NonPerishablesWeight,
                Perishables = volunteerGiveOutView.Perishables,
                PerishablesWeight = volunteerGiveOutView.PerishablesWeight,
                Frozen = volunteerGiveOutView.Frozen,
                FrozenWeight = volunteerGiveOutView.FrozenWeight,
                NonFood = volunteerGiveOutView.NonFood,
                NonFoodWeight = volunteerGiveOutView.NonFoodWeight,
                AddedBy = user.UserName,
                DateAdded = DateTime.Now
            };

            try
            {
                if (ModelState.IsValid)
                {
                    await _context.AddAsync(volunteerGiveOut);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(VolunteerGiveOutList));
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
                _logger.Log(LogLevel.Error, ex, $"An error has occurred when trying to write into {volunteerGiveOut.GetType().Name} table");
            }

            ViewData["Email"] = new SelectList(_userManager.Users, "Email", "FullName");

            return View(volunteerGiveOutView);
        }

        public async Task<IActionResult> EditVolunteerGiveOut(string id, string returnUrl = null)
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

            var volunteerGiveOut = await _context.VolunteerGiveOuts.Include(x => x.User).FirstOrDefaultAsync(x => x.VolunteerGiveOutId == num);
            if (volunteerGiveOut == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            VolunteerGiveOutViewModel volunteerGiveOutView = new()
            {
                VolunteerGiveOutId = volunteerGiveOut.VolunteerGiveOutId,
                Email = volunteerGiveOut.Email,
                CollectionDate = volunteerGiveOut.CollectionDate,
                Weight = volunteerGiveOut.Weight,
                NonPerishables = volunteerGiveOut.NonPerishables,
                NonPerishablesWeight = volunteerGiveOut.NonPerishablesWeight,
                Perishables = volunteerGiveOut.Perishables,
                PerishablesWeight = volunteerGiveOut.PerishablesWeight,
                Frozen = volunteerGiveOut.Frozen,
                FrozenWeight = volunteerGiveOut.FrozenWeight,
                NonFood = volunteerGiveOut.NonFood,
                NonFoodWeight = volunteerGiveOut.NonFoodWeight
            };

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
            }

            ViewData["ReturnUrl"] = retUrl;

            ViewData["Email"] = new SelectList(_userManager.Users, "Email", "FullName", volunteerGiveOut.Email);

            return View(volunteerGiveOutView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVolunteerGiveOut(string id, [Bind("VolunteerGiveOutId,Email,CollectionDate,Weight,NonPerishables,NonPerishablesWeight,Perishables,PerishablesWeight,Frozen,FrozenWeight,NonFood,NonFoodWeight")] VolunteerGiveOutViewModel volunteerGiveOutView, string returnUrl = null)
        {
            int num = _resolverService.ResolveInterger(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != volunteerGiveOutView.VolunteerGiveOutId)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            var user = await _userManager.GetUserAsync(User);

            var totalWeight = volunteerGiveOutView.NonPerishablesWeight + volunteerGiveOutView.PerishablesWeight + volunteerGiveOutView.FrozenWeight + volunteerGiveOutView.NonFoodWeight;

            VolunteerGiveOut volunteerGiveOut = new()
            {
                VolunteerGiveOutId = volunteerGiveOutView.VolunteerGiveOutId,
                Email = volunteerGiveOutView.Email,
                CollectionDate = volunteerGiveOutView.CollectionDate,
                Weight = (decimal)totalWeight,
                NonPerishables = volunteerGiveOutView.NonPerishables,
                NonPerishablesWeight = volunteerGiveOutView.NonPerishablesWeight,
                Perishables = volunteerGiveOutView.Perishables,
                PerishablesWeight = volunteerGiveOutView.PerishablesWeight,
                Frozen = volunteerGiveOutView.Frozen,
                FrozenWeight = volunteerGiveOutView.FrozenWeight,
                NonFood = volunteerGiveOutView.NonFood,
                NonFoodWeight = volunteerGiveOutView.NonFoodWeight,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            var oldValue = JsonConvert.SerializeObject(await _context.VolunteerGiveOuts.FirstOrDefaultAsync(x => x.VolunteerGiveOutId == volunteerGiveOut.VolunteerGiveOutId));
            var newValue = JsonConvert.SerializeObject(volunteerGiveOut);

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
            }

            if (await _approvalService.UpdateApprovalQueue(volunteerGiveOut.GetType().Name, "Volunteer Hamper", UpdateAction.Update, oldValue, newValue, user.UserName))
            {
                return RedirectToAction(nameof(Confirmation), new { url = retUrl });
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["Email"] = new SelectList(_userManager.Users, "Email", "FullName", volunteerGiveOut.Email);

            return View(volunteerGiveOutView);
        }

        public async Task<IActionResult> WastesList()
        {
            ViewData["ReturnUrl"] = HttpContext.Request.Path;

            return View(await _context.Wastes.ToListAsync());
        }

        public IActionResult AddWaste()
        {
            return View( new WasteViewModel() { Date = DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddWaste([Bind("Date,Weight,NonPerishables,NonPerishablesWeight,Perishables,PerishablesWeight,Frozen,FrozenWeight,NonFood,NonFoodWeight,Note")] WasteViewModel wasteView)
        {
            var user = await _userManager.GetUserAsync(User);

            var totalWeight = wasteView.NonPerishablesWeight + wasteView.PerishablesWeight + wasteView.FrozenWeight + wasteView.NonFoodWeight;

            Waste waste = new()
            {
                Date = wasteView.Date,
                Weight = (decimal)totalWeight,
                NonPerishables = wasteView.NonPerishables,
                NonPerishablesWeight = wasteView.NonPerishablesWeight,
                Perishables = wasteView.Perishables,
                PerishablesWeight = wasteView.PerishablesWeight,
                Frozen = wasteView.Frozen,
                FrozenWeight = wasteView.FrozenWeight,
                NonFood = wasteView.NonFood,
                NonFoodWeight = wasteView.NonFoodWeight,
                Note = wasteView.Note,                
                AddedBy = user.UserName,
                DateAdded = DateTime.Now
            };

            try
            {
                if (ModelState.IsValid)
                {
                    await _context.AddAsync(wasteView);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(WastesList));
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
                _logger.Log(LogLevel.Error, ex, $"An error has occurred when trying to write into {waste.GetType().Name} table");
            }

            return View(wasteView);
        }

        public async Task<IActionResult> EditWaste(string id, string returnUrl = null)
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

            var waste = await _context.Wastes.FirstOrDefaultAsync(x => x.WasteId == num);
            if (waste == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            WasteViewModel wasteView = new()
            {
                WasteId = waste.WasteId,
                Date = waste.Date,
                Weight = waste.Weight,
                NonPerishables = waste.NonPerishables,
                NonPerishablesWeight = waste.NonPerishablesWeight,
                Perishables = waste.Perishables,
                PerishablesWeight = waste.PerishablesWeight,
                Frozen = waste.Frozen,
                FrozenWeight = waste.FrozenWeight,
                NonFood = waste.NonFood,
                NonFoodWeight = waste.NonFoodWeight,
                Note = waste.Note,
            };

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
            }

            ViewData["ReturnUrl"] = retUrl;

            return View(wasteView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditWaste(string id, [Bind("WasteId,Date,Weight,NonPerishables,NonPerishablesWeight,Perishables,PerishablesWeight,Frozen,FrozenWeight,NonFood,NonFoodWeight,Note")] WasteViewModel wasteView, string returnUrl = null)
        {
            int num = _resolverService.ResolveInterger(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != wasteView.WasteId)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            var user = await _userManager.GetUserAsync(User);

            var totalWeight = wasteView.NonPerishablesWeight + wasteView.PerishablesWeight + wasteView.FrozenWeight + wasteView.NonFoodWeight;

            Waste waste = new()
            {
                WasteId = wasteView.WasteId,
                Date = wasteView.Date,
                Weight = (decimal)totalWeight,
                NonPerishables = wasteView.NonPerishables,
                NonPerishablesWeight = wasteView.NonPerishablesWeight,
                Perishables = wasteView.Perishables,
                PerishablesWeight = wasteView.PerishablesWeight,
                Frozen = wasteView.Frozen,
                FrozenWeight = wasteView.FrozenWeight,
                NonFood = wasteView.NonFood,
                NonFoodWeight = wasteView.NonFoodWeight,
                Note = wasteView.Note,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            var oldValue = JsonConvert.SerializeObject(await _context.Wastes.FirstOrDefaultAsync(x => x.WasteId == waste.WasteId));
            var newValue = JsonConvert.SerializeObject(waste);

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
            }

            if (await _approvalService.UpdateApprovalQueue(waste.GetType().Name, "Waste Item", UpdateAction.Update, oldValue, newValue, user.UserName))
            {
                return RedirectToAction(nameof(Confirmation), new { url = retUrl });
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(wasteView);
        }

        public async Task<IActionResult> ItemsForCollection()
        {
            var families = await _context.Hampers.Include(x => x.Family).Where(x => x.CollectionDate == DateTime.Today && !x.Collected).OrderBy(x => x.CollectionTime).ToListAsync();
            var partners = await _context.PartnerGiveOuts.Include(x => x.Partner).Where(x => x.CollectionDate == DateTime.Today && !x.Collected).OrderBy(x => x.CollectionTime).ToListAsync();

            List<DailyCollection> dailyCollections = [];

            foreach (var f in families)
            {
                DailyCollection collection = new() { Id = f.FamilyId, Name = f.Family.FullName, CollectionDate = f.CollectionDate, CollectionTime = f.CollectionTime, Weight = f.Weight, Collected = f.Collected, Source = "Family" };

                dailyCollections.Add(collection);
            }

            foreach (var p in partners)
            {
                DailyCollection collection = new() { Id = p.PartnerId, Name = p.Partner.Name, CollectionDate = p.CollectionDate, CollectionTime = p.CollectionTime, Weight = p.Weight, Collected = p.Collected, Source = "Family" };

                dailyCollections.Add(collection);
            }

            return View(dailyCollections.OrderBy(x => x.CollectionTime));
        }

        public async Task<IActionResult> FamilyCollected(string id)
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

            var hamper = await _context.Hampers.FirstOrDefaultAsync(x => x.HamperId == num);
            if (hamper == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            if (!hamper.Collected)
            {
                hamper.Collected = true;
                hamper.DateCollected = DateTime.Now;
            }

            try
            {
                _context.Update(hamper);
                await _context.SaveChangesAsync();               
            }
            catch (Exception ex)
            {          
                _logger.Log(LogLevel.Error, ex, "An error has occurred fetching item");
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });         
            }

            return RedirectToAction(nameof(ItemsForCollection));
        }

        public async Task<IActionResult> PartnerCollected(string id)
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

            var partnerGiveOut = await _context.PartnerGiveOuts.FirstOrDefaultAsync(x => x.PartnerGiveOutId == num);
            if (partnerGiveOut == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            if (!partnerGiveOut.Collected)
            {
                partnerGiveOut.Collected = true;
                partnerGiveOut.DateCollected = DateTime.Now;
            }

            try
            {
                _context.Update(partnerGiveOut);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                _logger.Log(LogLevel.Error, ex, "An error has occurred fetching item");
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            return RedirectToAction(nameof(ItemsForCollection));
        }

        public IActionResult Confirmation(string url)
        {
            ViewData["Approval"] = "Your changes have been forwarded for approval";
            ViewData["ReturnUrl"] = url;

            return View();
        }

        public async Task<IActionResult> PendingApprovals()
        {
            ViewData["ReturnUrl"] = HttpContext.Request.Path;

            return View(await _context.Approvals.Where(x => x.Status == ApprovalStatus.Pending).OrderByDescending(x => x.ApprovalQueueId).ToListAsync());
        }

        public async Task<IActionResult> ItemsForApproval()
        {
            ViewData["ReturnUrl"] = HttpContext.Request.Path;

            return View(await _context.Approvals.OrderByDescending(x => x.ApprovalQueueId).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> ItemsForApproval(DateRangeViewModel dateRangeView)
        {
            ViewData["ReturnUrl"] = HttpContext.Request.Path;

            return View(await _context.Approvals.Where(x => x.ChangeDate >= dateRangeView.StartDate && x.ChangeDate >= dateRangeView.EndDate).OrderByDescending(x => x.ApprovalQueueId).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveChange(string id, string returnUrl = null)
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

            var approvalQueue = await _context.Approvals.FirstOrDefaultAsync(x => x.ApprovalQueueId == num);
            if (approvalQueue == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            var user = await _userManager.GetUserAsync(User);

            approvalQueue.Status = ApprovalStatus.Approved;
            approvalQueue.ApprovedBy = user.UserName;
            approvalQueue.ApprovedDate = DateTime.Now;

            _context.Update(approvalQueue);
            await _context.SaveChangesAsync();

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
            }

            return RedirectToLocal(retUrl);
        }

        //TODO: fix rejection reason encryption
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectChange(string id, string rejectionReason, string returnUrl = null)
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

            var approvalQueue = await _context.Approvals.FirstOrDefaultAsync(x => x.ApprovalQueueId == num);
            if (approvalQueue == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            var user = await _userManager.GetUserAsync(User);

            approvalQueue.Status = ApprovalStatus.Declined;
            approvalQueue.RejectedBy = user.UserName;
            approvalQueue.RejectionReason = rejectionReason;
            approvalQueue.RejectionDate = DateTime.Now;

            _context.Update(approvalQueue);
            await _context.SaveChangesAsync();

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
            }

            return RedirectToLocal(retUrl);
        }

        [HttpGet]
        public void RejectChange(string approvalId)
        {
            return;
        }

        public async Task<IActionResult> ApprovalDetails(string id, string returnUrl = null) 
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

            var approvalQueue = await _context.Approvals.FirstOrDefaultAsync(x => x.ApprovalQueueId == num);
            if (approvalQueue == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
            }

            ApprovalObject approvalObject = new() { OldValue = approvalQueue.OldValue, NewValue = approvalQueue.NewValue, ReturnUrl = retUrl };
            string serializedObj = JsonConvert.SerializeObject(approvalObject);
            var encrptedObj = _protector.Encode(serializedObj);

            switch (approvalQueue.TableName)
            {
                case "Donor":
                    return RedirectToAction(nameof(DonorDetails), new { obj = encrptedObj });
                case "Family":
                    return RedirectToAction(nameof(FamilyDetails), new { obj = encrptedObj });
                case "Hamper":
                    return RedirectToAction(nameof(HamperDetails), new { obj = encrptedObj });
                case "InwardItem":
                    return RedirectToAction(nameof(InwardItemDetails), new { obj = encrptedObj });
                case "Partner":
                    return RedirectToAction(nameof(PartnerDetails), new { obj = encrptedObj });
                case "PartnerGiveOut":
                    return RedirectToAction(nameof(PartnerGiveOutDetails), new { obj = encrptedObj });
                case "VolunteerGiveOut":
                    return RedirectToAction(nameof(VolunteerGiveOutDetails), new { obj = encrptedObj });
                case "Waste":
                    return RedirectToAction(nameof(WasteDetails), new { obj = encrptedObj });
                default:
                    ViewBag.Message = "Unable to get changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
                    break;
            }

            return View();
        }

        public IActionResult DonorDetails(string obj)
        {
            if (obj == null)
            {
                //Not Found
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            ApprovalObject approvalObject = _resolverService.ResolveObject<ApprovalObject>(obj);
            if (approvalObject == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            Donor oldValue = JsonConvert.DeserializeObject<Donor>(approvalObject.OldValue);
            Donor newValue = JsonConvert.DeserializeObject<Donor>(approvalObject.NewValue);

            ViewData["oldValue"] = oldValue;
            ViewData["newValue"] = newValue;
            ViewData["ReturnUrl"] = approvalObject.ReturnUrl;

            return View();
        }

        public IActionResult FamilyDetails(string obj)
        {
            if (obj == null)
            {
                //Not Found
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            ApprovalObject approvalObject = _resolverService.ResolveObject<ApprovalObject>(obj);
            if (approvalObject == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            Family oldValue = JsonConvert.DeserializeObject<Family>(approvalObject.OldValue);
            Family newValue = JsonConvert.DeserializeObject<Family>(approvalObject.NewValue);

            ViewData["oldValue"] = oldValue;
            ViewData["newValue"] = newValue;
            ViewData["ReturnUrl"] = approvalObject.ReturnUrl;

            return View();
        }

        public async Task<IActionResult> HamperDetails(string obj)
        {
            if (obj == null)
            {
                //Not Found
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            ApprovalObject approvalObject = _resolverService.ResolveObject<ApprovalObject>(obj);
            if (approvalObject == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            Hamper oldValue = JsonConvert.DeserializeObject<Hamper>(approvalObject.OldValue);
            Hamper newValue = JsonConvert.DeserializeObject<Hamper>(approvalObject.NewValue);

            Family familyOld = await _context.Families.FirstOrDefaultAsync(x => x.FamilyId == oldValue.FamilyId);
            Family familyNew = await _context.Families.FirstOrDefaultAsync(x => x.FamilyId == newValue.FamilyId);

            oldValue.Family = familyOld;
            newValue.Family = familyNew;

            ViewData["oldValue"] = oldValue;
            ViewData["newValue"] = newValue;
            ViewData["ReturnUrl"] = approvalObject.ReturnUrl;

            return View();
        }

        public async Task<IActionResult> InwardItemDetails(string obj)
        {
            if (obj == null)
            {
                //Not Found
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            ApprovalObject approvalObject = _resolverService.ResolveObject<ApprovalObject>(obj);
            if (approvalObject == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            InwardItem oldValue = JsonConvert.DeserializeObject<InwardItem>(approvalObject.OldValue);
            InwardItem newValue = JsonConvert.DeserializeObject<InwardItem>(approvalObject.NewValue);

            Donor donorOld = await _context.Donors.FirstOrDefaultAsync(x => x.DonorId == oldValue.DonorId);
            Donor donorNew = await _context.Donors.FirstOrDefaultAsync(x => x.DonorId == newValue.DonorId);

            oldValue.Donor = donorOld;
            newValue.Donor = donorNew;

            ViewData["oldValue"] = oldValue;
            ViewData["newValue"] = newValue;
            ViewData["ReturnUrl"] = approvalObject.ReturnUrl;

            return View();
        }

        public IActionResult PartnerDetails(string obj)
        {
            if (obj == null)
            {
                //Not Found
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            ApprovalObject approvalObject = _resolverService.ResolveObject<ApprovalObject>(obj);
            if (approvalObject == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            Partner oldValue = JsonConvert.DeserializeObject<Partner>(approvalObject.OldValue);
            Partner newValue = JsonConvert.DeserializeObject<Partner>(approvalObject.NewValue);

            ViewData["oldValue"] = oldValue;
            ViewData["newValue"] = newValue;
            ViewData["ReturnUrl"] = approvalObject.ReturnUrl;

            return View();
        }

        public async Task<IActionResult> PartnerGiveOutDetails(string obj)
        {
            if (obj == null)
            {
                //Not Found
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            ApprovalObject approvalObject = _resolverService.ResolveObject<ApprovalObject>(obj);
            if (approvalObject == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            PartnerGiveOut oldValue = JsonConvert.DeserializeObject<PartnerGiveOut>(approvalObject.OldValue);
            PartnerGiveOut newValue = JsonConvert.DeserializeObject<PartnerGiveOut>(approvalObject.NewValue);

            Partner partnerOld = await _context.Partners.FirstOrDefaultAsync(x => x.PartnerId == oldValue.PartnerId);
            Partner partnerNew = await _context.Partners.FirstOrDefaultAsync(x => x.PartnerId == newValue.PartnerId);

            oldValue.Partner = partnerOld;
            newValue.Partner = partnerNew;

            ViewData["oldValue"] = oldValue;
            ViewData["newValue"] = newValue;
            ViewData["ReturnUrl"] = approvalObject.ReturnUrl;

            return View();
        }

        public async Task<IActionResult> VolunteerGiveOutDetails(string obj)
        {
            if (obj == null)
            {
                //Not Found
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            ApprovalObject approvalObject = _resolverService.ResolveObject<ApprovalObject>(obj);
            if (approvalObject == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            VolunteerGiveOut oldValue = JsonConvert.DeserializeObject<VolunteerGiveOut>(approvalObject.OldValue);
            VolunteerGiveOut newValue = JsonConvert.DeserializeObject<VolunteerGiveOut>(approvalObject.NewValue);

            ApplicationUser userOld = await _userManager.FindByEmailAsync(oldValue.Email);
            ApplicationUser userNew = await _userManager.FindByEmailAsync(newValue.Email);

            oldValue.User = userOld;
            newValue.User = userNew;

            ViewData["oldValue"] = oldValue;
            ViewData["newValue"] = newValue;
            ViewData["ReturnUrl"] = approvalObject.ReturnUrl;

            return View();
        }

        public IActionResult WasteDetails(string obj)
        {
            if (obj == null)
            {
                //Not Found
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            ApprovalObject approvalObject = _resolverService.ResolveObject<ApprovalObject>(obj);
            if (approvalObject == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            Waste oldValue = JsonConvert.DeserializeObject<Waste>(approvalObject.OldValue);
            Waste newValue = JsonConvert.DeserializeObject<Waste>(approvalObject.NewValue);

            ViewData["oldValue"] = oldValue;
            ViewData["newValue"] = newValue;
            ViewData["ReturnUrl"] = approvalObject.ReturnUrl;

            return View();
        }

        public async Task<IActionResult> ListFoodItems()
        {
            ViewData["ReturnUrl"] = HttpContext.Request.Path;

            return View(await _context.FoodItems.OrderByDescending(x => x.FoodItemId).ToListAsync());
        }

        public IActionResult AddFoodItem()
        {
            ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "ItemCategoryId", "Name");
            ViewData["UnitOfMeasureId"] = new SelectList(_context.UnitOfMeasures, "UnitOfMeasureId", "Name");

            return View(new FoodItemViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFoodItem([Bind("Name,ItemType,ItemCategoryId,InStock,HasOption,ReorderLevel,UnitOfMeasureId")] FoodItemViewModel foodItemView)
        {
            var user = await _userManager.GetUserAsync(User);

            FoodItem foodItem = new()
            {
                Name = foodItemView.Name,
                ItemType = foodItemView.ItemType,
                ItemCategoryId = foodItemView.ItemCategoryId,
                InStock = foodItemView.InStock,
                HasOption = foodItemView.HasOption,
                ReorderLevel = foodItemView.ReorderLevel,
                UnitOfMeasureId = foodItemView.UnitOfMeasureId,
                AddedBy = user.UserName,
                DateAdded = DateTime.Now
            };

            try
            {
                if (ModelState.IsValid)
                {
                    await _context.AddAsync(foodItem);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(ListFoodItems));
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
                _logger.Log(LogLevel.Error, ex, $"An error has occurred when trying to write into {foodItem.GetType().Name} table");
            }

            ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "ItemCategoryId", "Name");
            ViewData["UnitOfMeasureId"] = new SelectList(_context.UnitOfMeasures, "UnitOfMeasureId", "Name");

            return View(foodItemView);
        }

        public async Task<IActionResult> EditFoodItem(string id, string returnUrl = null)
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

            var foodItem = await _context.FoodItems.FirstOrDefaultAsync(x => x.FoodItemId == num);
            if (foodItem == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
            }

            FoodItemViewModel foodItemView = new()
            {
                FoodItemId = foodItem.FoodItemId,
                Name = foodItem.Name,
                ItemType = foodItem.ItemType,
                ItemCategoryId = foodItem.ItemCategoryId,
                InStock = foodItem.InStock,
                HasOption = foodItem.HasOption,
                ReorderLevel = foodItem.ReorderLevel,
                UnitOfMeasureId = foodItem.UnitOfMeasureId,
            };

            ViewData["ReturnUrl"] = retUrl;
            ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "ItemCategoryId", "Name", foodItemView.FoodItemId);
            ViewData["UnitOfMeasureId"] = new SelectList(_context.UnitOfMeasures, "UnitOfMeasureId", "Name", foodItemView.FoodItemId);

            return View(foodItemView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFoodItem(string id, [Bind("FoodItemId,Name,ItemType,ItemCategoryId,InStock,HasOption,ReorderLevel,UnitOfMeasureId")] FoodItemViewModel foodItemView, string returnUrl = null)
        {
            int num = _resolverService.ResolveInterger(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != foodItemView.FoodItemId)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            var user = await _userManager.GetUserAsync(User);

            FoodItem foodItem = new()
            {
                FoodItemId = foodItemView.FoodItemId,
                Name = foodItemView.Name,
                ItemType = foodItemView.ItemType,
                ItemCategoryId = foodItemView.ItemCategoryId,
                InStock = foodItemView.InStock,
                HasOption = foodItemView.HasOption,
                ReorderLevel = foodItemView.ReorderLevel,
                UnitOfMeasureId = foodItemView.UnitOfMeasureId,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            var oldValue = JsonConvert.SerializeObject(await _context.FoodItems.FirstOrDefaultAsync(x => x.FoodItemId == foodItem.FoodItemId));
            var newValue = JsonConvert.SerializeObject(foodItem);

            if (await _approvalService.UpdateApprovalQueue(foodItem.GetType().Name, "FoodItem", UpdateAction.Update, oldValue, newValue, user.UserName))
            {
                return RedirectToAction(nameof(Confirmation), new { url = returnUrl });
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "ItemCategoryId", "Name", foodItemView.FoodItemId);
            ViewData["UnitOfMeasureId"] = new SelectList(_context.UnitOfMeasures, "UnitOfMeasureId", "Name", foodItemView.FoodItemId);

            return View(foodItemView);
        }

        public async Task<IActionResult> ListFoodItemOptions()
        {
            ViewData["ReturnUrl"] = HttpContext.Request.Path;

            return View(await _context.FoodItemOptions.OrderByDescending(x => x.FoodItemOptionId).ToListAsync());
        }

        public IActionResult AddFoodOption()
        {
            return View(new FoodItemOptionViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFoodItemOption([Bind("FoodItemId,Option")] FoodItemOptionViewModel foodItemOptionView)
        {
            var user = await _userManager.GetUserAsync(User);

            FoodItemOption foodItemOption = new()
            {
                FoodItemId = foodItemOptionView.FoodItemId,
                Option = foodItemOptionView.Option,
                AddedBy = user.UserName,
                DateAdded = DateTime.Now
            };

            try
            {
                if (ModelState.IsValid)
                {
                    await _context.AddAsync(foodItemOption);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(ListFoodItemOptions));
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
                _logger.Log(LogLevel.Error, ex, $"An error has occurred when trying to write into {foodItemOption.GetType().Name} table");
            }

            return View(foodItemOptionView);
        }

        public async Task<IActionResult> EditFoodItemOption(string id, string returnUrl = null)
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

            var foodItemOption = await _context.FoodItemOptions.FirstOrDefaultAsync(x => x.FoodItemOptionId == num);
            if (foodItemOption == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            string retUrl = string.Empty;
            if (returnUrl != null)
            {
                retUrl = _resolverService.ResolveString(returnUrl);
            }

            FoodItemOptionViewModel foodItemOptionView = new()
            {
                FoodItemId = foodItemOption.FoodItemId,
                Option = foodItemOption.Option,
            };

            ViewData["ReturnUrl"] = retUrl;

            return View(foodItemOptionView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFoodItemOption(string id, [Bind("FoodItemOptionId,FoodItemId,Option")] FoodItemOptionViewModel foodItemOptionView, string returnUrl = null)
        {
            int num = _resolverService.ResolveInterger(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != foodItemOptionView.FoodItemOptionId)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            var user = await _userManager.GetUserAsync(User);

            FoodItemOption foodItemOption = new()
            {
                FoodItemOptionId = foodItemOptionView.FoodItemOptionId,
                FoodItemId = foodItemOptionView.FoodItemId,
                Option = foodItemOptionView.Option,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            var oldValue = JsonConvert.SerializeObject(await _context.FoodItems.FirstOrDefaultAsync(x => x.FoodItemId == foodItemOption.FoodItemOptionId));
            var newValue = JsonConvert.SerializeObject(foodItemOption);

            if (await _approvalService.UpdateApprovalQueue(foodItemOption.GetType().Name, "FoodItemOption", UpdateAction.Update, oldValue, newValue, user.UserName))
            {
                return RedirectToAction(nameof(Confirmation), new { url = returnUrl });
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(foodItemOptionView);
        }

        public async Task<IActionResult> ListStocks()
        {
            ViewData["ReturnUrl"] = HttpContext.Request.Path;

            return View(await _context.Stocks.OrderByDescending(x => x.StockId).ToListAsync());
        }

        public IActionResult AddStock()
        {
            return View(new StockViewModel() { Date = DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStock([Bind("FoodItemId,Quantity,Date,TransactionType")] StockViewModel stockView)
        {
            var user = await _userManager.GetUserAsync(User);

            Stock stock = new()
            {
                FoodItemId = stockView.FoodItemId,
                Quantity = stockView.Quantity,
                AddedBy = user.UserName,
                DateAdded = DateTime.Now
            };

            try
            {
                if (ModelState.IsValid)
                {
                    using (TransactionScope scope = new ())
                    {
                        if (await _stockLevelRepository.UpdateStocHistory(stock))
                        {
                            await _context.AddAsync(stock);
                            await _context.SaveChangesAsync();

                            scope.Complete();
                        }
                        else
                        {
                            ViewBag.Message = "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.";

                            return View(stockView);
                        }
                    }
                        

                    return RedirectToAction(nameof(ListStocks));
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
                _logger.Log(LogLevel.Error, ex, $"An error has occurred when trying to write into {stock.GetType().Name} table");
            }

            return View(stockView);
        }      

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
