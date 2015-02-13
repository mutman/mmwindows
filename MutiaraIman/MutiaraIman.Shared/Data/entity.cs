using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MutiaraIman.Data
{
	[JsonObject(MemberSerialization.OptIn)]
    public class entity : BindableBase
    {
		[JsonProperty] public string id;
		[JsonProperty] public logInformation logInformation;
		[JsonProperty] public string title;
		[JsonProperty] public string content;
		[JsonProperty] public category category;
		[JsonProperty] public string reminder;

		public string ID
		{
			get { return ID; }
			set { SetProperty<string>(ref id, value, "ID"); }
		}

		public logInformation LogInformation
		{
			get { return logInformation; }
			set { SetProperty<logInformation>(ref logInformation, value, "LogInformation"); }
		}

		public string Title
		{
			get { return title; }
			set { SetProperty<string>(ref title, value, "Title"); }
		}

		public string Content
		{
			get { return content; }
			set { SetProperty<string>(ref content, value, "Content"); }
		}

		public category Category
		{
			get { return category; }
			set { SetProperty<category>(ref category, value, "Category"); }
		}

		public string Reminder
		{
			get { return reminder; }
			set { SetProperty<string>(ref reminder, value, "Reminder"); }
		}
    }
}
