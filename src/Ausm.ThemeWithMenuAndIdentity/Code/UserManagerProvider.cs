using Microsoft.AspNetCore.Identity;
using ObjectStore.Identity;
using System.Collections.Generic;
using System.Linq;
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
        IEnumerable<User> GetUsers();
        IEnumerable<Role> GetRoles();
        Task<IEnumerable<string>> GetUserRolesAsync(string username);
        Task<IdentityResult> ToggleUserRoleAsync(string username, string rolename);
    }

    class UserManagerProvider<TUser, TRole> : IUserManagerProvider
        where TUser : User
        where TRole : Role
    {
        #region Fields
        SignInManager<TUser> _signInManager;
        UserManager<TUser> _userManager;
        RoleManager<TRole> _roleManager;
        #endregion

        #region Constructors
        public UserManagerProvider(SignInManager<TUser> signInManager, UserManager<TUser> userManager, RoleManager<TRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
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

        public IEnumerable<User> GetUsers()
        {
            return _userManager.Users.ToList().Cast<User>();
        }

        public IEnumerable<Role> GetRoles()
        {
            return _roleManager.Roles.ToList().Cast<Role>();
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string username)
        {
            TUser user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return null;

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> ToggleUserRoleAsync(string username, string rolename)
        {
            TUser user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return IdentityResult.Failed(new IdentityError() { Description = "Username not found" });

            if(await _userManager.IsInRoleAsync(user, rolename))
                return await _userManager.RemoveFromRoleAsync(user, rolename);
            else
                return await _userManager.AddToRoleAsync(user, rolename);
        }
        #endregion
    }
}
