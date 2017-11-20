using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ausm.ThemeWithMenuAndIdentity.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using ObjectStore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Ausm.ThemeWithMenuAndIdentity
{
    public class AccountController : Controller
    {
        #region Fields
        IUserManagerProvider _userManagerProvider;
        #endregion

        #region Constructor
        public AccountController(IUserManagerProvider userManagerProvider)
        {
            _userManagerProvider = userManagerProvider;
        }
        #endregion

        #region Actions
        #region Index
        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            List<Role> roles = _userManagerProvider.GetRoles().ToList();
            List<UserViewModel> model = new List<UserViewModel>();

            foreach (User user in _userManagerProvider.GetUsers())
            {
                List<string> userRoles = (await _userManagerProvider.GetUserRolesAsync(user.Name)).ToList();

                model.Add(new UserViewModel()
                {
                    Username = user.Name,
                    UserRoles = roles.Select(x => new UserViewModel.Role() { Name = x.Name, IsSet = userRoles.Contains(x.Name) }).ToList()
                });
            }

            return View(model);
        }
        #endregion

        #region Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
                return View(model);

            Microsoft.AspNetCore.Identity.SignInResult result = await _userManagerProvider.SignInAsync(model.Username, model.Password);
            if (result.Succeeded)
                return Redirect(returnUrl ?? "/");
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
        }
        #endregion

        #region ChangePassword
        [HttpGet, Authorize]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            IdentityResult result = await _userManagerProvider.ChangePasswordAsync(User, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
                return Redirect("/");

            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return View(model);
        }
        #endregion

        #region ResetPassword
        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetPasswordByAdmin(string username) =>
            RedirectToAction(nameof(ResetPassword), new { u = username, t = await _userManagerProvider.GeneratePasswordResetTokenAsync(username) });

        [HttpGet]
        public IActionResult ResetPassword(string u, string t) =>
            View(new ResetPasswordViewModel() { Username = u, Token = t });

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            IdentityResult result = await _userManagerProvider.ResetPasswordAsync(model.Username, model.Token, model.NewPassword);
            if (result.Succeeded)
                return Redirect("/");

            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return View(model);
        }
        #endregion

        #region LogOff
        [Authorize]
        public async Task<IActionResult> LogOff()
        {
            await _userManagerProvider.SignOutAsync();
            return Redirect("/");
        }
        #endregion

        #region CreateUser
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model, [FromQuery] string returnurl)
        {
            if (!ModelState.IsValid)
                return ViewError(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));

            IdentityResult result = await _userManagerProvider.CreateUserAsync(model.Username, model.Password);
            if (result.Succeeded)
            {
                if (string.IsNullOrWhiteSpace(returnurl))
                    return RedirectToAction("Index");
                else
                    return Redirect(returnurl);
            }

            return ViewError(result.Errors.Select(x => $"{x.Code} {x.Description}"));
        }
        #endregion

        #region ToggleRole
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleRole([FromForm]string username, [FromForm]string rolename, [FromQuery]string returnurl)
        {
            IdentityResult result = await _userManagerProvider.ToggleUserRoleAsync(username, rolename);
            if (!result.Succeeded)
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description);

            if (string.IsNullOrWhiteSpace(returnurl))
                return RedirectToAction("Index");
            else
                return Redirect(returnurl);
        }
        #endregion
        #endregion

        #region Methods
        IActionResult ViewError(IEnumerable<string> errors)
        {
            ViewData["Error"] = errors;
            return View("Error");
        }
        #endregion
    }
}
