using System;
using System.Collections.Generic;

namespace Ausm.ThemeWithMenuAndIdentity
{
    public class ThemeOptions
    {
        public string BrandName { get; set; } = "Home";
        public string LogoUrl { get; set; }
        public IEnumerable<IMenuItem> StaticMenuItems { get; set; }
        public Delegate DynamicMenuItems { get; private set; }

        public void SetDynamicMenuItemExpression(Func<IEnumerable<IMenuItem>> expression)
        {
            DynamicMenuItems = expression;
        }

        public void SetDynamicMenuItemExpression<T1>(Func<T1, IEnumerable<IMenuItem>> expression)
        {
            DynamicMenuItems = expression;
        }

        public void SetDynamicMenuItemExpression<T1, T2>(Func<T1, T2, IEnumerable<IMenuItem>> expression)
        {
            DynamicMenuItems = expression;
        }

        public void SetDynamicMenuItemExpression<T1, T2, T3>(Func<T1, T2, T3, IEnumerable<IMenuItem>> expression)
        {
            DynamicMenuItems = expression;
        }

        public void SetDynamicMenuItemExpression<T1, T2, T3, T4>(Func<T1, T2, T3, T4, IEnumerable<IMenuItem>> expression)
        {
            DynamicMenuItems = expression;
        }

    }
}
