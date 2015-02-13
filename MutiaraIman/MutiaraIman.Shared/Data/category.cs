using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MutiaraIman.Data
{
	[JsonObject(MemberSerialization.OptIn)]
    public class category : BindableBase
    {
		[JsonProperty] public string id;
		[JsonProperty] public logInformation logInformation;
		[JsonProperty] public string name;
		[JsonProperty] public string description;

		public string ID
		{
			get { return id; }
			set { SetProperty<string>(ref id, value, "ID"); }
		}

		public logInformation LogInformation
		{
			get { return logInformation; }
			set { SetProperty<logInformation>(ref logInformation, value, "LogInformation"); }
		}

		public string Name
		{
			get { return name; }
			set { SetProperty<string>(ref name, value, "Name"); }
		}

		public string Description
		{
			get { return description; }
			set { SetProperty<string>(ref description, value, "Description"); }
		}
    }
}
