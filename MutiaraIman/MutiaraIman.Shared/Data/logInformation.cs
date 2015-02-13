using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MutiaraIman.Data
{
	[JsonObject(MemberSerialization.OptIn)]
    public class logInformation : BindableBase
    {
		[JsonProperty] public long createDate;

		public long CreateDate
		{
			get { return createDate; }
			set { SetProperty<long>(ref createDate, value, "CreateDate"); }
		}
    }
}
