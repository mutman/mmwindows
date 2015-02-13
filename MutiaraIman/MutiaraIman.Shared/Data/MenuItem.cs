using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;

namespace MutiaraIman.Data
{
    public class MenuItem : BindableBase
    {
		private string category;
		private BitmapImage imageSource;
		private BitmapImage altImageSource;
		private string title;

		public string Category
		{
			get { return category; }
			set { SetProperty<string>(ref category, value, "Category"); }
		}

		public BitmapImage ImageSource
		{
			get { return imageSource; }
			set { SetProperty<BitmapImage>(ref imageSource, value, "ImageSource"); }
		}

		public BitmapImage AltImageSource
		{
			get { return altImageSource; }
			set { SetProperty<BitmapImage>(ref altImageSource, value, "AltImageSource"); }
		}

		public string Title
		{
			get { return title; }
			set { SetProperty<string>(ref title, value, "Title"); }
		}
    }
}
