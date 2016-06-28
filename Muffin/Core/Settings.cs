using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Newtonsoft.Json.Converters;

namespace Muffin.Core
{
	/// <summary>
	/// Settings from the web.config Muffin.* keys
	/// </summary>
	public static class Settings
	{
		public static string EmptyImageUrl
		{
			get
			{
				var ret = WebConfigurationManager.AppSettings["Muffin.EmptyImageUrl"];
				return ret ?? string.Empty;
			}
		}
    }
}
