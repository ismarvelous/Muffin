using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Caching;
using Umbraco.Core.Logging;

namespace Muffin.Caching
{
	/// <summary>
	/// Is a dummy cache provider which for debuging distributed caching 
	/// </summary>
	public class DebugOutputCacheProvider : OutputCacheProvider, IEnumerable<KeyValuePair<string, object>>
	{
		protected void LogMessage(string message)
		{
			Debug.WriteLine(message);
			Console.WriteLine(message);
			LogHelper.Info(typeof(DebugOutputCacheProvider), message);
		}

		public override object Get(string key)
		{
			var message = string.Format("Get '{0}' from cache", key);
			LogMessage(message);
			return null;
		}

		public override object Add(string key, object entry, DateTime utcExpiry)
		{
			string message = string.Format("Add '{0}' of type '{1}' which expires at: {2} ", key, entry.GetType().FullName, utcExpiry);
			LogMessage(message);
			return null;
		}

		public override void Set(string key, object entry, DateTime utcExpiry)
		{
			var message = string.Format("Set '{0}' of type '{1}' which expires at: {2} ", key, entry.GetType().FullName, utcExpiry);
			LogMessage(message);
		}

		public override void Remove(string key)
		{
			var message = string.Format("Remove '{0}' from cache", key);
			LogMessage(message);
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			var message = string.Format("GetEnumerator from cache");
			return (new Dictionary<string, object>()).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			var message = string.Format("GetEnumerator from cache");
			return (new Dictionary<string, object>()).GetEnumerator();
		}
	}
}
