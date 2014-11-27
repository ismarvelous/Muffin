using System.Web;
using System.Web.Mvc;
using Muffin.Core;
using Umbraco.Core;

namespace Muffin.Mvc
{
	public static class Extensions
	{
		/// <summary>
		/// Translate the string by using the string itself as the key.
		/// use the querystring ?debugtranslation=true to show all translation keys on a rendered page.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string Translate(this string str)
		{
			return str.Translate(str);
		}

		/// <summary>
		/// Translate the string by using the given key
		/// use the querystring ?debugtranslation=true to show all translation keys on a rendered page.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string Translate(this string str, string key)
		{
			//show keys instead of translation..
			if (HttpContext.Current != null && !HttpContext.Current.Request.QueryString["debugtranslation"].IsNullOrWhiteSpace())
			{
				return string.Format("#{0}",key);
			}

			var repo = DependencyResolver.Current.GetService<ISiteRepository>();
			var ret = repo.Translate(key);

			return ret.IsNullOrWhiteSpace() ? str : ret;

			//todo: auto add items into the dictionary when a specific boolean is set in the webconfig.
		}

		public static string LimitLength(this string source, int maxLength)
		{
		    return source.Length <= maxLength ? source : source.Substring(0, maxLength);
		}
	}

}