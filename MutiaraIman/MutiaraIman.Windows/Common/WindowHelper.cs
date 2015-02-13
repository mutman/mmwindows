using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MutiaraIman.Common
{
	public class WindowHelper
	{
        private Page _page;
		private double _width;
		private double _height;
		private bool _menuOpened = false;
		public bool MenuOpened
		{
			get { return _menuOpened; }
			set
			{
				_menuOpened = value;
				DetermineState(_width, _height);
			}
		}

        public WindowHelper(Page page)
        {
            _page = page;
            _page.Loaded += page_Loaded;
            _page.Unloaded += page_Unloaded;
        }

        public IEnumerable<WindowState> States { get; set; }

        private void page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Window.Current.SizeChanged += Window_SizeChanged;
            DetermineState(Window.Current.Bounds.Width, Window.Current.Bounds.Height);
        }

        private void page_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Window_SizeChanged;
        }

		public void SetSize(Size windowSize)
		{
			_width = windowSize.Width;
			_height = windowSize.Height;
			DetermineState(_width, _height, true);
		}

        private void Window_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
			_width = e.Size.Width;
			_height = e.Size.Height;
            DetermineState(_width, _height, true);
        }

        private void DetermineState(double width, double height, bool transitions = false)
        {
            var state = States.First(x => x.MatchCriterium(width, height, _menuOpened));
            VisualStateManager.GoToState(_page, state.State, transitions);
        }
    }

    public class WindowState
    {
        public string State { get; set; }

        public Func<double, double, bool, bool> MatchCriterium { get; set; }
    }
}
