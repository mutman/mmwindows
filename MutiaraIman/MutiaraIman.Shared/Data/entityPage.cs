using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MutiaraIman.Data
{
	[JsonObject(MemberSerialization.OptIn)]
    public class entityPage : BindableBase
    {
		[JsonProperty] public int rowCount;
		[JsonProperty] public int totalPage;
		[JsonProperty] public int limit;
		[JsonProperty] public int currentPage;
		[JsonProperty] public ObservableCollection<entity> entityList;

		public int RowCount
		{
			get { return rowCount; }
			set { SetProperty<int>(ref rowCount, value, "RowCount"); }
		}

		public int TotalPage
		{
			get { return totalPage; }
			set { SetProperty<int>(ref totalPage, value, "TotalPage"); }
		}

		public int Limit
		{
			get { return limit; }
			set { SetProperty<int>(ref limit, value, "Limit"); }
		}

		public int CurrentPage
		{
			get { return currentPage; }
			set { SetProperty<int>(ref currentPage, value, "CurrentPage"); }
		}

		public ObservableCollection<entity> EntityList
		{
			get { return entityList; }
			set { SetProperty<ObservableCollection<entity>>(ref entityList, value, "EntityList"); }
		}
    }
}
