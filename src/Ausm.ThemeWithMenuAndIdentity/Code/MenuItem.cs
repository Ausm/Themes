using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ausm.ThemeWithMenuAndIdentity
{
    public interface IMenuItem
    {
        string Name { get; }
        string Url { get; }
        Type Controller { get; }
        string Action { get; }
        IEnumerable<IMenuItem> SubItems { get; }

        bool IsEnabled { get; }
        bool IsVisible { get; }
        bool IsSeparator { get; }
    }

    public class MenuItem : IMenuItem
    {
        #region Fields
        Type _controller;
        string _action;
        string _name;
        string _url;
        IMenuItem[] _subItems;
        Func<IEnumerable<IMenuItem>> _subItemsFunc;

        bool _isEnabled;
        bool _isVisible;
        #endregion

        #region Constructors
        private MenuItem(string name, string url, Type controller, string action, Func<IEnumerable<IMenuItem>> subItemsFunc, IMenuItem[] subItems)
        {
            _name = name;
            _url = url;
            _controller = controller;
            _action = action;
            _subItemsFunc = subItemsFunc;
            _subItems = subItems;
            _isEnabled = 
            _isVisible = true;
        }

        public MenuItem(string name, Type controller, string action, Func<IEnumerable<IMenuItem>> subItemsFunc = null) :
            this(name, null, controller, action, subItemsFunc, null)
        {
        }

        public MenuItem(string name, string url, Func<IEnumerable<IMenuItem>> subItemsFunc = null) :
            this(name, url, null, null, subItemsFunc, null)
        {
        }

        public MenuItem(string name, Type controller, string action, params IMenuItem[] subItems) :
            this(name, null, controller, action, null, subItems)
        {
        }

        public MenuItem(string name, string url, IMenuItem[] subItems) :
            this(name, url, null, null, null, subItems)
        {
        }
        #endregion

        #region Properties
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
            }
        }

        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
            }
        }
        #endregion

        #region IMenuItem Member
        string IMenuItem.Action => _action;

        Type IMenuItem.Controller => _controller;

        bool IMenuItem.IsEnabled => _isEnabled;

        bool IMenuItem.IsSeparator => false;

        bool IMenuItem.IsVisible => _isVisible;

        string IMenuItem.Name => _name;

        string IMenuItem.Url => _url;

        IEnumerable<IMenuItem> IMenuItem.SubItems
        {
            get
            {
                if (_subItems != null)
                    return _subItems;
                else if (_subItemsFunc != null)
                    return _subItemsFunc();
                else
                    return Enumerable.Empty<IMenuItem>();
            }
        }
        #endregion
    }

    public class SeparatorItem : IMenuItem
    {
        string IMenuItem.Action => null;
        Type IMenuItem.Controller => null;
        bool IMenuItem.IsEnabled => false;
        bool IMenuItem.IsSeparator => true;
        bool IMenuItem.IsVisible => true;
        string IMenuItem.Name => null;
        IEnumerable<IMenuItem> IMenuItem.SubItems => Enumerable.Empty<IMenuItem>();
        string IMenuItem.Url => null;
    }
}
