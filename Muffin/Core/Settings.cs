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
				var ret = WebConfigurationManager.AppSettings["Muffin.EmptyImageUrl"];
				return ret ?? string.Empty;
			}
		}

        public static string CurrentTheme
        {
            get
            {
                var ret = WebConfigurationManager.AppSettings["Muffin.Themes.Current"];
                return ret ?? string.Empty;
            }
        }

	}
}
