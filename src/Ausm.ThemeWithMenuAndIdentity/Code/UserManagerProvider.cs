using Microsoft.AspNetCore.Identity;
using ObjectStore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ausm.ThemeWithMenuAndIdentity
{
    public interface IUserManagerProvider
    {
        Task<SignInResult> SignInAsync(string username, string password);
        Task SignOutAsync();
        Task<IdentityResult> ChangePasswordAsync(ClaimsPrincipal principal, string currentPassword, string newPassword);
        Task<IdentityResult> CreateUserAsync(string username, string password);
    }

    class UserManagerProvider<TUser> : IUserManagerProvider
        where TUser : User
    {
        #region Fields
        SignInManager<TUser> _signInManager;
        UserManager<TUser> _userManager;
        #endregion

        #region Constructors
        public UserManagerProvider(SignInManager<TUser> signInManager, UserManager<TUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        #endregion

        #region Methods
        public Task<SignInResult> SignInAsync(string username, string password)
        {
            return _signInManager.PasswordSignInAsync(username, password, false, false);
        }

        public Task SignOutAsync()
        {
            return _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ChangePasswordAsync(ClaimsPrincipal principal, string currentPassword, string newPassword)
        {
            TUser user = await _userManager.GetUserAsync(principal);
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<IdentityResult> CreateUserAsync(string username, string password)
        {
            ClassMock<TUser> mock = new ClassMock<TUser>();
            mock.Value.Name = username;

            return await _userManager.CreateAsync(mock.Value, password);
        }
        #endregion
    }
}
