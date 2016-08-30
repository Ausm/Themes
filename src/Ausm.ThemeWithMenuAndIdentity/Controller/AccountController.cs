using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ausm.ThemeWithMenuAndIdentity.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using ObjectStore.Identity;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Ausm.ThemeWithMenuAndIdentity
{
    public class AccountController : Controller
    {
        #region Subclasses
        class UserMock : User
        {
            public override int Id => 0;

            public override string Name { get; set; }

            public override string NormalizedUsername { get; set; }

            public override string Password { get; set; }
        }
        #endregion

        SignInManager<User> _signInManager;
        UserManager<User> _userManager;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

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

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            if (result.Succeeded)
                return Redirect(returnUrl ?? "/");
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
        }

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

            User user = await _userManager.GetUserAsync(User);
            IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
                return Redirect("/");

            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            IdentityResult result = await _userManager.CreateAsync(new UserMock() { Name = model.Username }, model.Password);
            if (result.Succeeded)
                return Redirect("/");

            foreach(IdentityError error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return View(model);
        }
    }
}
