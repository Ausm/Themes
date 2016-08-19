using System;
using System.Collections.Generic;

namespace Ausm.ThemeWithMenuAndIdentity
{
    public class ThemeOptions
    {
        public string LogoUrl { get; set; }
        public IEnumerable<IMenuItem> StaticMenuItems { get; set; }
        public Func<IEnumerable<IMenuItem>> DynamicMenuItems { get; set; }
    }
}
