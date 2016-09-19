using ObjectStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Entities
{
    [Table("Users")]
    public abstract class User : ObjectStore.Identity.User
    {
    }

    [Table("Roles")]
    public abstract class Role : ObjectStore.Identity.Role
    {
    }

    [Table("UsersInRole")]
    public abstract class UserInRole : ObjectStore.Identity.UserInRole<User, Role>
    {
    }
}
