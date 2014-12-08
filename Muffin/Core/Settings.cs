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

        #region Theme / custom view folder support
        public static string CurrentTheme
        {
            get
            {
                var ret = WebConfigurationManager.AppSettings["Muffin.Themes.Current"];
                return ret ?? string.Empty;
            }
        }

	    public static string CurrentThemeViewPath
	    {
	        get 
            {
	            return !string.IsNullOrEmpty(CurrentTheme) ? 
                    string.Format("~/Themes/{0}/Views", CurrentTheme) : 
                    "~/Views";
	        }
	    }

        public static string CurrentThemePath
        {
            get
            {
                return !string.IsNullOrEmpty(CurrentTheme) ?
                    string.Format("~/Themes/{0}", CurrentTheme) :
                    "~/";
            }
        }

        #endregion

    }
}
