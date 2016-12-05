using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ausm.ThemeWithMenuAndIdentity.ViewModels.Account
{
    public class UserViewModel
    {
        public class Role
        {
            [Display(Name = "Rolename")]
            [ReadOnly(true)]
            public string Name { get; set; }

            [Display(Name = "Set")]
            [ReadOnly(true)]
            public bool IsSet { get; set; }
        }

        [ReadOnly(true)]
        public string Id { get; set; }

        [Display(Name = "Username")]
        [ReadOnly(true)]
        public string Username { get; set; }

        [Display(Name = "Locked")]
        [ReadOnly(true)]
        public bool IsLocked { get; set; }

        [Display(Name = "Roles")]
        [ReadOnly(true)]
        public List<Role> UserRoles { get; set; }
    }
}
