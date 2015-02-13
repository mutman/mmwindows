using MutiaraIman.Common;
using MutiaraIman.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace MutiaraIman
{
	/// <summary>
	/// A basic page that provides characteristics common to most applications.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		private static Dictionary<string, MenuItem> menuitems = new Dictionary<string, MenuItem>
		{
			{ "prayers", new MenuItem
				{
					Category = "prayers", Title = "Doa",
					ImageSource = new BitmapImage(new Uri("ms-appx:/Assets/prayer.png", UriKind.Absolute)),
					AltImageSource = new BitmapImage(new Uri("ms-appx:/Assets/prayer-inverted.png", UriKind.Absolute))
				}
			},
			{ "reflections", new MenuItem
				{
					Category = "reflections", Title = "Renungan",
					ImageSource = new BitmapImage(new Uri("ms-appx:/Assets/reflections.png", UriKind.Absolute)),
					AltImageSource = new BitmapImage(new Uri("ms-appx:/Assets/reflections-inverted.png", UriKind.Absolute))
				}
			},
			{ "worship", new MenuItem
				{
					Category = "worship", Title = "Lagu Rohani",
					ImageSource = new BitmapImage(new Uri("ms-appx:/Assets/worship.png", UriKind.Absolute)),
					AltImageSource = new BitmapImage(new Uri("ms-appx:/Assets/worship-inverted.png", UriKind.Absolute))
				}
			},
			{ "stories", new MenuItem
				{
					Category = "stories", Title = "Kisah Santo dan Santa",
					ImageSource = new BitmapImage(new Uri("ms-appx:/Assets/stories.png", UriKind.Absolute)),
					AltImageSource = new BitmapImage(new Uri("ms-appx:/Assets/stories-inverted.png", UriKind.Absolute))
				}
			},
			{ "quote", new MenuItem
				{
					Category = "quote", Title = "Catholic Quote",
					ImageSource = new BitmapImage(new Uri("ms-appx:/Assets/quote.png", UriKind.Absolute)),
					AltImageSource = new BitmapImage(new Uri("ms-appx:/Assets/quote-inverted.png", UriKind.Absolute))
				}
			},
		};

		private static Dictionary<string, entityPage> entitycache = new Dictionary<string, entityPage>();
		public static entityPage CurrentPage = null;
		private static string CurrentCategory = null;
		private static DateTime CurrentSyncSession = DateTime.Now;
		private ScrollViewer GridViewScroll = null;

		private WindowHelper windowHelper;
		private NavigationHelper navigationHelper;
		private ObservableDictionary defaultViewModel = new ObservableDictionary();

		public static readonly DependencyProperty MenuOpenedProperty = DependencyProperty.Register("MenuOpened", typeof(bool), typeof(MainPage), new PropertyMetadata(false, MenuOpenedChanged));
		public bool MenuOpened
		{
			get { return (bool)GetValue(MenuOpenedProperty); }
			set { SetValue(MenuOpenedProperty, value); }
		}
		public static void MenuOpenedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MainPage page = d as MainPage;
			if (page != null)
			{
				page.MenuOpenedChanged((bool)e.NewValue);
			}
		}
		public void MenuOpenedChanged(bool newValue)
		{
			windowHelper.MenuOpened = newValue;
		}

		/// <summary>
		/// This can be changed to a strongly typed view model.
		/// </summary>
		public ObservableDictionary DefaultViewModel
		{
			get { return this.defaultViewModel; }
		}

		/// <summary>
		/// NavigationHelper is used on each page to aid in navigation and 
		/// process lifetime management
		/// </summary>
		public NavigationHelper NavigationHelper
		{
			get { return this.navigationHelper; }
		}


		public MainPage()
		{
			this.InitializeComponent();
			this.navigationHelper = new NavigationHelper(this);
			this.navigationHelper.LoadState += navigationHelper_LoadState;
			this.navigationHelper.SaveState += navigationHelper_SaveState;

			windowHelper = new WindowHelper(this)
			{
				States = new List<WindowState>()
				{
					new WindowState { State = "WideOpened", MatchCriterium = (w, h, o) => w > 900 && o },
					new WindowState { State = "WideClosed", MatchCriterium = (w, h, o) => w > 900 && !o },
					new WindowState { State = "NarrowOpened", MatchCriterium = (w, h, o) => w <= 900 && o },
					new WindowState { State = "NarrowClosed", MatchCriterium = (w, h, o) => w <= 900 && !o }
				}
			};
		}

		/// <summary>
		/// Populates the page with content passed during navigation. Any saved state is also
		/// provided when recreating a page from a prior session.
		/// </summary>
		/// <param name="sender">
		/// The source of the event; typically <see cref="NavigationHelper"/>
		/// </param>
		/// <param name="e">Event data that provides both the navigation parameter passed to
		/// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
		/// a dictionary of state preserved by this page during an earlier
		/// session. The state will be null the first time a page is visited.</param>
		private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
		{
		}

		/// <summary>
		/// Preserves state associated with this page in case the application is suspended or the
		/// page is discarded from the navigation cache.  Values must conform to the serialization
		/// requirements of <see cref="SuspensionManager.SessionState"/>.
		/// </summary>
		/// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
		/// <param name="e">Event data that provides an empty dictionary to be populated with
		/// serializable state.</param>
		private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
		{
		}

		#region NavigationHelper registration

		/// The methods provided in this section are simply used to allow
		/// NavigationHelper to respond to the page's navigation methods.
		/// 
		/// Page specific logic should be placed in event handlers for the  
		/// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
		/// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
		/// The navigation parameter is available in the LoadState method 
		/// in addition to page state preserved during an earlier session.

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			navigationHelper.OnNavigatedTo(e);
			//await LoadEntitiesAsync("reflections");
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			navigationHelper.OnNavigatedFrom(e);
		}

		#endregion

		private void CategoryList_ItemClick(object sender, ItemClickEventArgs e)
		{

		}

		private void pageRoot_Loaded(object sender, RoutedEventArgs e)
		{
			CategoryList.ItemsSource = menuitems;
			CategoryList.SelectedIndex = 1;
			GridViewScroll = ContentList.GetFirstDescendantOfType<ScrollViewer>();
			if (GridViewScroll != null)
			{
				GridViewScroll.ViewChanged += gridViewScroll_ViewChanged;
			}
			DataTransferManager.GetForCurrentView().DataRequested += MainPage_DataRequested;
			windowHelper.SetSize(this.RenderSize);
		}

		public void MainPage_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
		{
			entity item = ContentList.SelectedItem as entity;
			if (item != null)
			{
				args.Request.Data.Properties.Title = item.title;
				args.Request.Data.Properties.Description = item.content;
				args.Request.Data.SetWebLink(new Uri(string.Format("http://www.mutiara-iman.org/module/{0}#{1}", CurrentCategory, item.id)));
			}
			else
			{
				args.Request.FailWithDisplayText("Nothing to share");
			}
		}

		private async void gridViewScroll_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{
			try
			{
				if (!e.IsIntermediate && GridViewScroll.VerticalOffset >= GridViewScroll.ScrollableHeight * 0.9)
				{
					await LoadNextEntitiesAsync();
				}
			}
			catch
			{ }
		}

		private async Task LoadEntitiesAsync(string category)
		{
			MenuItem menuitem = menuitems[category];
			if (menuitem != null)
			{
				CategoryIcon.Source = menuitem.ImageSource;
				CategoryName.Text = menuitem.Title;
			}
			CurrentSyncSession = DateTime.Now;
			DateTime syncSession = CurrentSyncSession;
			try
			{
				//ErrorMessage.Visibility = Visibility.Collapsed;
				ProgressBar.IsIndeterminate = true;
				ProgressBar.Visibility = Visibility.Visible;
				if (entitycache.ContainsKey(category))
				{
					CurrentPage = entitycache[category];
					ContentList.ItemsSource = CurrentPage.EntityList;
					ContentList.SelectedIndex = 0;
				}
				else
				{
					ContentList.ItemsSource = null;
					Title.Text = "Loading...";
					Content.Text = string.Empty;
					ShareButton.Visibility = Visibility.Collapsed;
					TitleSeparator.Visibility = Visibility.Collapsed;
					YamaNewsApi api = new YamaNewsApi();
					entityPage page = await api.GetEntitiesAsync(category, 0, 40);
					if (syncSession == CurrentSyncSession && page != null)
					{
						CurrentPage = page;
						CurrentCategory = category;
						ContentList.ItemsSource = page.EntityList;
						ContentList.SelectedIndex = 0;
						entitycache.Add(category, page);
					}
				}
				await Dispatcher.RunAsync(CoreDispatcherPriority.Low, async () =>
				{
					if (GridViewScroll.VerticalOffset >= GridViewScroll.ScrollableHeight * 0.9)
					{
						await LoadNextEntitiesAsync();
					}
				});
			}
			catch
			{
				Title.Text = "Failed to load content.";
				Content.Text = string.Empty;
				ShareButton.Visibility = Visibility.Collapsed;
				TitleSeparator.Visibility = Visibility.Collapsed;
			}
			finally
			{
				if (syncSession == CurrentSyncSession)
				{
					ProgressBar.Visibility = Visibility.Collapsed;
					ProgressBar.IsIndeterminate = false;
				}
			}
		}

		private async Task LoadNextEntitiesAsync()
		{
			if (!string.IsNullOrEmpty(CurrentCategory) && CurrentPage != null && CurrentPage.currentPage < CurrentPage.totalPage - 1)
			{
				CurrentSyncSession = DateTime.Now;
				DateTime syncSession = CurrentSyncSession;
				entityPage syncedPage = CurrentPage;
				try
				{
					ProgressBar.IsIndeterminate = true;
					ProgressBar.Visibility = Visibility.Visible;
					YamaNewsApi api = new YamaNewsApi();
					entityPage page = await api.GetEntitiesAsync(CurrentCategory, CurrentPage.currentPage + 1, 40);
					if (page != null && syncedPage != null)
					{
						foreach (entity entity in page.entityList)
						{
							if (!string.IsNullOrEmpty(entity.title) && syncedPage.entityList.SingleOrDefault(e => e.id == entity.id) == null)
							{
								syncedPage.entityList.Add(entity);
							}
						}
						syncedPage.currentPage = page.currentPage;
					}
					await Dispatcher.RunAsync(CoreDispatcherPriority.Low, async () =>
					{
						if (GridViewScroll.HorizontalOffset >= GridViewScroll.ScrollableWidth * 0.99)
						{
							await LoadNextEntitiesAsync();
						}
					});
				}
				catch
				{
				}
				finally
				{
					if (syncSession == CurrentSyncSession)
					{
						ProgressBar.Visibility = Visibility.Collapsed;
						ProgressBar.IsIndeterminate = false;
					}
				}
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			MenuOpened = !MenuOpened;
		}

		private void ContentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			entity item = e.AddedItems.FirstOrDefault() as entity;
			if (item != null)
			{
				Title.Text = item.Title;
				Content.Text = item.Content;
				ShareButton.Visibility = Visibility.Visible;
				TitleSeparator.Visibility = Visibility.Visible;
				ContentScroll.ChangeView(0.0, null, null);
			}
		}

		private async void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				KeyValuePair<string, MenuItem> selectedMenu = (KeyValuePair<string, MenuItem>)e.AddedItems.FirstOrDefault();
				await LoadEntitiesAsync(selectedMenu.Key);
			}
		}

		private void ShareButton_Click(object sender, RoutedEventArgs e)
		{
			DataTransferManager.ShowShareUI();
		}

		private void ContentList_Tapped(object sender, TappedRoutedEventArgs e)
		{
			MenuOpened = false;
		}

		private void ContentWrapper_Tapped(object sender, TappedRoutedEventArgs e)
		{
			MenuOpened = false;
		}
	}
}
