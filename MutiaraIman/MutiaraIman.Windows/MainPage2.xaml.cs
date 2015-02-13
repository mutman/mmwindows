using MutiaraIman.Data;
using MutiaraIman.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MutiaraIman
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage2 : Page
    {
		private static List<MenuItem> menuitems = new List<MenuItem>
		{
			new MenuItem { Title = "Doa", ImageSource = new BitmapImage(new Uri("ms-appx:/Assets/menu-doa.png", UriKind.Absolute)) },
			new MenuItem { Title = "Renungan", ImageSource = new BitmapImage(new Uri("ms-appx:/Assets/menu-renungan.png", UriKind.Absolute)) },
			new MenuItem { Title = "Lagu Rohani", ImageSource = new BitmapImage(new Uri("ms-appx:/Assets/menu-lagu.png", UriKind.Absolute)) },
			new MenuItem { Title = "Kisah Santo dan Santa", ImageSource = new BitmapImage(new Uri("ms-appx:/Assets/menu-santo.png", UriKind.Absolute)) },
			new MenuItem { Title = "Catholic Quote", ImageSource = new BitmapImage(new Uri("ms-appx:/Assets/menu-quote.png", UriKind.Absolute)) },
		};

		private static Dictionary<string, entityPage> entitycache = new Dictionary<string, entityPage>();
		public static entityPage CurrentPage = null;
		private static string CurrentCategory = null;
		private static DateTime CurrentSyncSession = DateTime.Now;
		private ScrollViewer GridViewScroll = null;

        public MainPage2()
        {
            this.InitializeComponent();
        }

		private async void Page_Loaded(object sender, RoutedEventArgs e)
		{
			CategoryList.ItemsSource = menuitems;
			GridViewScroll = ContentList.GetFirstDescendantOfType<ScrollViewer>();
			if (GridViewScroll != null)
			{
				GridViewScroll.ViewChanged += gridViewScroll_ViewChanged;
			}
		}

		private async void gridViewScroll_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{
			try
			{
				if (!e.IsIntermediate && GridViewScroll.HorizontalOffset >= GridViewScroll.ScrollableWidth * 0.99)
				{
					await LoadNextEntitiesAsync();
				}
			}
			catch
			{ }
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			await LoadEntitiesAsync("prayers");
		}

		private async void CategoryList_ItemClick(object sender, ItemClickEventArgs e)
		{
			MenuItem item = e.ClickedItem as MenuItem;
			if (item != null)
			{
				int i = menuitems.IndexOf(item);
				switch (i)
				{
					default:
					case 0:
						await LoadEntitiesAsync("prayers");
						break;
					case 1:
						await LoadEntitiesAsync("reflections");
						break;
					case 2:
						await LoadEntitiesAsync("worship");
						break;
					case 3:
						await LoadEntitiesAsync("stories");
						break;
					case 4:
						await LoadEntitiesAsync("quote");
						break;
				}
			}
		}

		private async Task LoadEntitiesAsync(string category)
		{
			CurrentSyncSession = DateTime.Now;
			DateTime syncSession = CurrentSyncSession;
			try
			{
				ErrorMessage.Visibility = Visibility.Collapsed;
				ProgressBar.IsIndeterminate = true;
				ProgressBar.Visibility = Visibility.Visible;
				if (entitycache.ContainsKey(category))
				{
					CurrentPage = entitycache[category];
					ContentList.ItemsSource = CurrentPage.EntityList;
				}
				else
				{
					YamaNewsApi api = new YamaNewsApi();
					entityPage page = await api.GetEntitiesAsync(category, 0, 40);
					if (syncSession == CurrentSyncSession && page != null)
					{
						CurrentPage = page;
						CurrentCategory = category;
						ContentList.ItemsSource = page.EntityList;
						entitycache.Add(category, page);
					}
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
				ErrorMessage.Visibility = Visibility.Visible;
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

		private void ContentList_ItemClick(object sender, ItemClickEventArgs e)
		{
			entity item = e.ClickedItem as entity;
			if (item != null)
			{
				this.Frame.Navigate(typeof(ReadingPage), item);
			}
		}
	}
}
