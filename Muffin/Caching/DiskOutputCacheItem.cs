using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Muffin.Caching
{
	public class DiskOutputCacheItem
	{
		public DiskOutputCacheItem() { }
		public DiskOutputCacheItem(string fileName, DateTime utcExpiry)
		{
			this.FileName = fileName;
			this.UtcExpiry = utcExpiry;
		}

		public string FileName { get; set; }
		public DateTime UtcExpiry { get; set; }
	}
}