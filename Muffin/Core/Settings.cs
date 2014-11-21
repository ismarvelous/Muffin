using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Muffin.Core
{
	/// <summary>
	/// Settings from the web.config Macaw.Umbraco.Foundation.* keys
	/// </summary>
	public static class Settings
	{
		public static string EmptyImageUrl
		{
			get
			{
				var ret = WebConfigurationManager.AppSettings["Macaw.Umbraco.Foundation.EmptyImageUrl"];
				return ret ?? string.Empty;
			}
		}
	}
}
