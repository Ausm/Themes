using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

namespace Ausm.ThemeWithMenuAndIdentity.HtmlHelpers
{
    public static class MenuHelper
    {
        public static IHtmlContent GetMenu(this Microsoft.AspNetCore.Mvc.Razor.RazorPage razorPage, Delegate menuItemFunction)
        {
            if (menuItemFunction == null)
                return null;

            IServiceProvider serviceProvider = razorPage.Context.RequestServices;
            object[] arguments = menuItemFunction.GetMethodInfo().GetParameters().Select(x => serviceProvider.GetService(x.ParameterType)).ToArray();

            IEnumerable<IMenuItem> menuItems = menuItemFunction.DynamicInvoke(arguments) as IEnumerable<IMenuItem>;

            if (menuItems == null)
                return null;

            return GetMenu(razorPage, menuItems);
        }

        public static IHtmlContent GetMenu(this Microsoft.AspNetCore.Mvc.Razor.RazorPage razorPage, IEnumerable<IMenuItem> menuItems)
        {
            if (menuItems == null)
                return null;

            IUrlHelper urlHelper = razorPage.GetType().GetTypeInfo().GetProperty("Url").GetValue(razorPage) as IUrlHelper;
            if (urlHelper == null)
                return null;

            HtmlContentBuilder builder = new HtmlContentBuilder();
            foreach (IMenuItem menuItem in menuItems.Where(x => x != null))
                builder.AppendHtml(WriteMenuItemToBuilder(razorPage, urlHelper, menuItem));

            return builder;
        }

        static IHtmlContent WriteMenuItemToBuilder(this Microsoft.AspNetCore.Mvc.Razor.RazorPage razorPage, IUrlHelper helper, IMenuItem menuItem)
        {
            TagBuilder liTag = new TagBuilder("li");

            if (menuItem.IsSeparator)
            {
                liTag.AddCssClass("divider");
                liTag.Attributes.Add("role", "separator");

                return liTag;
            }

            List<IMenuItem> subItems = menuItem.SubItems.ToList();
            TagBuilder aTag = new TagBuilder("a");

            if (!menuItem.IsVisible)
                liTag.AddCssClass("hidden");
            else if (!menuItem.IsEnabled)
                liTag.AddCssClass("disabled");
            else if (!string.IsNullOrWhiteSpace(menuItem.Url))
                aTag.Attributes.Add("href", menuItem.Url);
            else if (!string.IsNullOrEmpty(menuItem.Action) && menuItem.Controller != null)
            {
                ControllerActionDescriptor actionDescriptor = razorPage.ViewContext.ActionDescriptor as ControllerActionDescriptor;
                if (actionDescriptor?.ActionName == menuItem.Action && actionDescriptor?.ControllerName == menuItem.Controller)
                    liTag.AddCssClass("active");

                aTag.Attributes.Add("href", helper.Action(menuItem.Action, menuItem.Controller));
            }
            else
                aTag.Attributes.Add("href", "#");

            aTag.InnerHtml.Append(menuItem.Name);
            liTag.InnerHtml.AppendHtml(aTag);

            if (subItems.Count != 0)
            {
                aTag.AddCssClass("dropdown-toggle");
                aTag.Attributes.Add("data-toggle", "dropdown");
                aTag.Attributes.Add("role", "button");
                aTag.Attributes.Add("aria-haspopup", "true");
                aTag.Attributes.Add("aria-expanded", "false");

                TagBuilder spanCaret = new TagBuilder("span");
                spanCaret.AddCssClass("caret");

                aTag.InnerHtml.AppendHtml(spanCaret);

                TagBuilder ulTag = new TagBuilder("ul");
                ulTag.AddCssClass("dropdown-menu");
                foreach (IMenuItem subItem in subItems)
                    ulTag.InnerHtml.AppendHtml(WriteMenuItemToBuilder(razorPage, helper, subItem));

                liTag.InnerHtml.AppendHtml(ulTag);
            }

            return liTag;
        }
    }
}
