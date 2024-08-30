using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using ReachingFam.Core.Data;
using ReachingFam.Core.Models;
using ReachingFam.Core.Models.AccountViewModels;
using Microsoft.EntityFrameworkCore;
using ReachingFam.Core.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ReachingFam.Controllers
{
    [Authorize]
    public class AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        //IEmailSender emailSender,
        //ISmsSender smsSender,
        ILoggerFactory loggerFactory,
        ApplicationDbContext context,
        IResolverService resolverService) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        //private readonly IEmailSender _emailSender;
        //private readonly ISmsSender _smsSender;
        private readonly ILogger _logger = loggerFactory.CreateLogger<AccountController>();
        private readonly ApplicationDbContext _context = context;
        private readonly IResolverService _resolverService = resolverService;

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            ViewData["LayoutType"] = "login-page";
            ViewData["BoxType"] = "login-box";
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["LayoutType"] = "login-page";
            ViewData["BoxType"] = "login-box";

            
            if (ModelState.IsValid)
            {
                //var claims = new List<Claim>
                //{
                //    new("UserName", model.Email)
                //};

                //var identity = new ClaimsIdentity(
                //    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                //var claimsPrincipal = new ClaimsPrincipal(identity);

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Email);

                    if (!model.Email.Equals("ayo.anegbe@gmail.com")) //default admin
                    {
                        //var employee = _context.Employees.FirstOrDefault(e => e.Email.Equals(model.Email));
                        //if (employee != null && !employee.Status)
                        //{
                        //    ViewBag.Message = "Your account has been disabled!";
                        //    return View(model);
                        //}

                        //if (!user.IsStaff && !(await _userManager.IsEmailConfirmedAsync(user)))
                        //{
                        //    ViewBag.Message = "Please check you email to confirm your account!";
                        //    return View(model);
                        //}
                    }

                    if (user.ChangePassword) // password change required
                    {
                        return RedirectToAction(nameof(ManageController.ChangePassword), "Manage");
                    }

                    //if (user.IsStaff)
                    //{
                    //    return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
                    //}

                    _logger.LogInformation("User logged in.");
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null, string source = null)
        {
            if (!string.IsNullOrEmpty(source))
            {
                ViewData["source"] = source;
            }

            ViewData["ReturnUrl"] = returnUrl;
            ViewData["LayoutType"] = "register-page";
            ViewData["BoxType"] = "register-box";

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null, string source = null)
        {
            if (!string.IsNullOrEmpty(source))
            {
                ViewData["source"] = source;

                if (source.Equals("admin"))
                {
                    var user = await _userManager.GetUserAsync(User);
                }
            }

            ViewData["ReturnUrl"] = returnUrl;
            ViewData["LayoutType"] = "register-page";
            ViewData["BoxType"] = "register-box";
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Phone = model.Phone,
                    ChangePassword = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");

                    //const string password = "Password01!";
                    //var ir = await _userManager.AddPasswordAsync(user, password);
                    //if (ir.Succeeded)
                    //{
                    //    _logger.LogInformation(4, $"Set password '{password}' for default user '{model.Email}' successfully");
                    //}

                    _logger.Log(LogLevel.Information, $"Add default user '{model.Email}' to role '{model.Role}'");
                    var ir = await _userManager.AddToRoleAsync(user, model.Role.ToString());
                    if (ir.Succeeded)
                    {
                        _logger.Log(LogLevel.Information, $"Added the role '{model.Role}' to default user `{model.Email}` successfully");
                    }
                    else
                    {
                        var exception = new Exception($"The role '{model.Role}' could not be set for the user `{model.Email}`");
                        _logger.Log(LogLevel.Debug, $"An error has occurred fetching item {exception}");
                        throw exception;
                    }

                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(AccountController.Login));
        }

        public async Task<IActionResult> UserList()
        {
            var users = await _userManager.Users.ToListAsync();

            foreach (var usr in users)
            {
                var rol = await _userManager.GetRolesAsync(usr);
                if (rol.Equals(Constants.SuperAdmin))
                    users.Remove(usr);
            }

            return View(users);
        }

        public async Task<IActionResult> GetAllUsers()
        {
            return View(await _userManager.Users.ToListAsync());
        }

        public async Task<IActionResult> DisableUser(string userName)
        {            

            if (userName == null)
            {
                //Not Found
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 404 });
            }

            string resolvedUser = _resolverService.ResolveString(userName);

            if (resolvedUser == null)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var user = await _userManager.FindByNameAsync(resolvedUser);            

            if (await _userManager.IsInRoleAsync(user, Constants.SuperAdmin))
            {
                return RedirectToAction(nameof(AccountController.GetAllUsers));
            }

            if (user.IsActive)
            {
                user.IsActive = false;
            }
            else
            {
                user.IsActive = true;
            }

            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(AccountController.UserList));
        }

        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            ViewData["LayoutType"] = "login-page";

            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            ViewData["LayoutType"] = "login-page";

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                //var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var callbackUrl = Url.Action(nameof(ResetPassword), "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                //await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                //   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                //return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            ViewData["LayoutType"] = "login-page";

            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            ViewData["LayoutType"] = "login-page";

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Lockout()
        {

            var user = await _userManager.GetUserAsync(User);

            var loginView = new LoginViewModel
            {
                Email = user.Email
            };

            HttpContext.Session.Clear();

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");

            ViewData["LoginView"] = loginView;

            return RedirectToAction(nameof(Login), "Account");
        }

        //
        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
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

        #endregion
    }
}
