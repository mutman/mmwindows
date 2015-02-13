using MutiaraIman.Data;
using MutiaraIman.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MutiaraIman
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		public static readonly DependencyProperty PrayersPageProperty = DependencyProperty.Register("PrayersPage", typeof(entityPage), typeof(MainPage), new PropertyMetadata(null));
		public entityPage PrayersPage
		{
			get { return (entityPage)GetValue(PrayersPageProperty); }
			set { SetValue(PrayersPageProperty, value); }
		}

		public static readonly DependencyProperty ReflectionsPageProperty = DependencyProperty.Register("ReflectionsPage", typeof(entityPage), typeof(MainPage), new PropertyMetadata(null));
		public entityPage ReflectionsPage
		{
			get { return (entityPage)GetValue(ReflectionsPageProperty); }
			set { SetValue(ReflectionsPageProperty, value); }
		}

		public static readonly DependencyProperty WorshipPageProperty = DependencyProperty.Register("WorshipPage", typeof(entityPage), typeof(MainPage), new PropertyMetadata(null));
		public entityPage WorshipPage
		{
			get { return (entityPage)GetValue(WorshipPageProperty); }
			set { SetValue(WorshipPageProperty, value); }
		}

		public static readonly DependencyProperty StoriesPageProperty = DependencyProperty.Register("StoriesPage", typeof(entityPage), typeof(MainPage), new PropertyMetadata(null));
		public entityPage StoriesPage
		{
			get { return (entityPage)GetValue(StoriesPageProperty); }
			set { SetValue(StoriesPageProperty, value); }
		}

		public static readonly DependencyProperty QuotePageProperty = DependencyProperty.Register("QuotePage", typeof(entityPage), typeof(MainPage), new PropertyMetadata(null));
		public entityPage QuotePage
		{
			get { return (entityPage)GetValue(QuotePageProperty); }
			set { SetValue(QuotePageProperty, value); }
		}

		private static Dictionary<string, entityPage> entitycache = new Dictionary<string, entityPage>();
		private DispatcherTimer ReloadTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 30) };
		private Dictionary<string, ScrollViewer> ListViewScrolls = new Dictionary<string, ScrollViewer>();

		public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
			ReloadTimer.Tick += ReloadTimer_Tick;
			HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

		private void Hub_SectionsInViewChanged(object sender, SectionsInViewChangedEventArgs e)
		{
			FindScrollViewers();
		}

		private void pageRoot_Loaded(object sender, RoutedEventArgs e)
		{
			FindScrollViewers();
		}

		private void FindScrollViewers()
		{
			if (ListViewScrolls.Count < 5)
			{
				FindScrollViewer(PrayersSection, "prayers");
				FindScrollViewer(ReflectionsSection, "reflections");
				FindScrollViewer(WorshipSection, "worship");
				FindScrollViewer(StoriesSection, "stories");
				FindScrollViewer(QuoteSection, "quote");
			}
		}

		private void FindScrollViewer(DependencyObject parentNode, string category)
		{
			if (!ListViewScrolls.ContainsKey(category))
			{
				ScrollViewer scrollViewer = parentNode.GetFirstDescendantOfType<ScrollViewer>();
				if (scrollViewer != null)
				{
					ListViewScrolls[category] = scrollViewer;
					scrollViewer.ViewChanged += scrollViewer_ViewChanged;
				}
			}
		}

		private async void scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{
			try
			{
				KeyValuePair<string, ScrollViewer> matchingPair = ListViewScrolls.FirstOrDefault(p => p.Value == sender);
				if (!e.IsIntermediate && matchingPair.Value.VerticalOffset >= matchingPair.Value.ScrollableHeight * 0.99)
				{
					await LoadNextEntitesAsync(matchingPair.Key);
				}
			}
			catch
			{
			}
		}

		private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
		{
			if (Frame.CanGoBack)
			{
				Frame.GoBack();
				e.Handled = true;
			}
		}

		private async void ReloadTimer_Tick(object sender, object e)
		{
			ReloadTimer.Stop();
			await LoadAllEntitiesAsync();
		}

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

			await LoadAllEntitiesAsync();
        }

		private void ListView_ItemClick(object sender, ItemClickEventArgs e)
		{
			entity item = e.ClickedItem as entity;
			if(item != null)
			{
				Frame.Navigate(typeof(ReadingPage), item);
			}
		}

		private async Task LoadAllEntitiesAsync()
		{
			StatusBarProgressIndicator progressBar = StatusBar.GetForCurrentView().ProgressIndicator;
			IAsyncAction showAction = progressBar.ShowAsync();
			showAction.Completed = async (action, status) =>
			{
				await Task.Delay(1000);
				progressBar.Text = "Loading...";
				progressBar.ProgressValue = null;
			};
			string[] categories = new string[] { "prayers", "reflections", "worship", "stories", "quote" };
			List<Task> tasks = new List<Task>();
			tasks.Add(showAction.AsTask());
			foreach (string category in categories)
			{
				if (!entitycache.ContainsKey(category))
				{
					tasks.Add(LoadEntitiesAsync(category));
				}
			}
			if (tasks.Count > 0)
			{
				await Task.WhenAll(tasks);
				ReloadTimer.Start();
			}
			else
			{
				ReloadTimer.Stop();
			}
			await progressBar.HideAsync();
			progressBar.Text = string.Empty;
		}

		private async Task LoadEntitiesAsync(string category)
		{
			try
			{
				if (entitycache.ContainsKey(category))
				{
					switch (category)
					{
						case "prayers":
							PrayersPage = entitycache[category];
							break;
						case "reflections":
							ReflectionsPage = entitycache[category];
							break;
						case "worship":
							WorshipPage = entitycache[category];
							break;
						case "stories":
							StoriesPage = entitycache[category];
							break;
						case "quote":
							QuotePage = entitycache[category];
							break;
					}
				}
				else
				{
					YamaNewsApi api = new YamaNewsApi();
					entityPage page = await api.GetEntitiesAsync(category, 0, 15);
					if (page != null)
					{
						switch (category)
						{
							case "prayers":
								PrayersPage = page;
								break;
							case "reflections":
								ReflectionsPage = page;
								break;
							case "worship":
								WorshipPage = page;
								break;
							case "stories":
								StoriesPage = page;
								break;
							case "quote":
								QuotePage = page;
								break;
						}
						entitycache.Add(category, page);
					}
				}
			}
			catch
			{
			}
		}

		private async Task LoadNextEntitesAsync(string category)
		{
			try
			{
				if (entitycache.ContainsKey(category))
				{
					entityPage currentPage = entitycache[category];
					if(currentPage.currentPage < currentPage.totalPage - 1)
					{
						StatusBarProgressIndicator progressBar = StatusBar.GetForCurrentView().ProgressIndicator;
						IAsyncAction showAction = progressBar.ShowAsync();
						showAction.Completed = async (action, status) =>
						{
							await Task.Delay(1000);
							progressBar.Text = "Loading...";
							progressBar.ProgressValue = null;
						};
						await showAction.AsTask();
						YamaNewsApi api = new YamaNewsApi();
						entityPage page = await api.GetEntitiesAsync(category, currentPage.currentPage + 1, 15);
						foreach (entity entity in page.entityList)
						{
							if (!string.IsNullOrEmpty(entity.title) && currentPage.entityList.SingleOrDefault(e => e.id == entity.id) == null)
							{
								currentPage.entityList.Add(entity);
							}
						}
						currentPage.currentPage = page.currentPage;
						await progressBar.HideAsync();
						progressBar.Text = string.Empty;
					}
				}
			}
			catch
			{
			}
		}
    }
}
