using MutiaraIman.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MutiaraIman
{
    public class YamaNewsApi
    {
		private const string baseurl = "www.mutiara-iman.org";

		public async Task<entityPage> GetEntitiesAsync(string category, int page, int max)
		{
			HttpClient client = new HttpClient();
			try
			{
				string url = string.Format("http://{0}/module/{1}.json?page={2}&max={3}", baseurl, category, page, max);
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
				HttpResponseMessage response = await client.SendAsync(request);
				if (response.IsSuccessStatusCode)
				{
					string json = await response.Content.ReadAsStringAsync();
					if (!string.IsNullOrEmpty(json))
					{
						return JsonConvert.DeserializeObject<entityPage>(json);
					}
				}
				throw new JsonSerializationException();
			}
			finally
			{
				client.Dispose();
			}
		}
    }
}
