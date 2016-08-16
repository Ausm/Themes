using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ausm.ThemeWithMenuAndIdentity.Code
{
    public class ThemeOptions
    {
        public IEnumerable<IMenuItem> PublicRootMenuItems { get; set; }
        public Func<IEnumerable<IMenuItem>> UserDependendMenuItems { get; set; }
    }
}
