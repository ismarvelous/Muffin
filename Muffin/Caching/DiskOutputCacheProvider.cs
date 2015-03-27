using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.IO;
using Umbraco.Core.Logging;
using System.Collections;
using System.Linq;

namespace Muffin.Caching
{
    /// <summary>
    /// based on: http://www.4guysfromrolla.com/articles/061610-1.aspx
    /// Behaves like a distributed cache when using Azure Websites
    /// Alternative: http://msdn.microsoft.com/en-us/library/windowsazure/dn448829.aspx
    /// </summary>
    public class DiskOutputCacheProvider : OutputCacheProvider, IEnumerable<KeyValuePair<string, object>>
    {
        static readonly object LockObject = new object();

        private readonly Dictionary<string, object> _cacheItems = new Dictionary<string, object>();
        private Dictionary<string, object> CacheItems
        {
            get
            {
                return _cacheItems;
            }
        }

        public string CacheFolder
        {
            get
            {
                var folder = HostingEnvironment.ApplicationPhysicalPath + @"App_data\Muffin.DiskCache\";
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                return folder;
            }
        }

        public DiskOutputCacheProvider()
        {
            
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return (new List<KeyValuePair<string, object>>(CacheItems)).GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return GetEnumerator();
        }

        public override object Add(string key, object entry, DateTime utcExpiry)
        {
            key = GetSafeFileName(key); //be sure the key is a file safe name..
            LogAction("Add", string.Format("Key: {0} | UtcExpiry: {1}", key, utcExpiry.ToString()));

            // See if this key already exists in the cache. If so, we need to return it and NOT overwrite it!
            var results = this.Get(key);
            if (results != null)
                return results;

            // If the item is NOT in the cache, then save it!
            this.Set(key, entry, utcExpiry);

            return entry;
        }

        public override object Get(string key)
        {
            key = GetSafeFileName(key); //be sure the key is a file safe name..
            LogAction("Get", string.Format("Key: {0}", key));

            object obj = null;
            CacheItems.TryGetValue(key, out obj);
            var item = obj as DiskOutputCacheItem;

            // Was the item found?
            if (item == null)
                return null;

            // Has the item expired?
            if (item.UtcExpiry < DateTime.UtcNow)
            {
                // Item has expired
                this.Remove(key);

                return null;
            }

            return GetCacheData(item);
        }

        public override void Remove(string key)
        {
            key = GetSafeFileName(key); //be sure the key is a file safe name..
            LogAction("Remove", string.Format("Key: {0}", key));

            object obj = null;
            this.CacheItems.TryGetValue(key, out obj);
            var item = obj as DiskOutputCacheItem;

            if (item != null)
            {
                // Attempt to delete the cached content on disk and then remove the item from CacheItems... 
                // If there is a problem, fail silently
                try
                {
                    RemoveCacheData(item);

                    CacheItems.Remove(key);
                }
                catch { }
            }
        }

        public override void Set(string key, object entry, DateTime utcExpiry)
        {
            key = GetSafeFileName(key); //be sure the key is a file safe name..
            LogAction("Set", string.Format("Key: {0} | UtcExpiry: {1}", key, utcExpiry.ToString()));

            // Create a DiskOutputCacheItem object
            var item = new DiskOutputCacheItem(key, utcExpiry);

            try
            {
                WriteCacheData(item, entry);

                // Add item to CacheItems, if needed, or update the existing key, if it already exists
                lock (LockObject)
                {
                    if (this.CacheItems.ContainsKey(key))
                        this.CacheItems[key] = item;
                    else
                        this.CacheItems.Add(key, item);
                }
            }
            catch (PathTooLongException) { } //fail silently, can't cach this item because of a too long file path.
        }

        protected virtual object GetCacheData(DiskOutputCacheItem item)
        {
            var fileToRetrieve = Path.Combine(this.CacheFolder, item.FileName);

            if (File.Exists(fileToRetrieve))
            {
                var formatter = new BinaryFormatter();
                using (var stream = new FileStream(fileToRetrieve, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return formatter.Deserialize(stream);
                }
            }
            else
                // Eep, could not find the file!
                return null;
        }

        protected virtual void RemoveCacheData(DiskOutputCacheItem item)
        {
            var fileToRetrieve = Path.Combine(this.CacheFolder, item.FileName);
            if (File.Exists(fileToRetrieve))
                File.Delete(fileToRetrieve);
        }

        protected virtual void WriteCacheData(DiskOutputCacheItem item, object entry)
        {
            var fileToWrite = Path.Combine(this.CacheFolder, item.FileName);

            var formatter = new BinaryFormatter();
            using (var stream = new FileStream(fileToWrite, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, entry);
            }
        }

        protected virtual string GetSafeFileName(string unsafeFileName)
        {
            var safeFileName = unsafeFileName;

            foreach (char c in Path.GetInvalidFileNameChars())
                safeFileName = safeFileName.Replace(c.ToString(), "_");

            return safeFileName;
        }


        protected virtual void LogAction(string actionName, string details)
        {
            LogHelper.Info(typeof(DiskOutputCacheProvider), string.Format("actionname: {0}, details: {1}", actionName, details));
        }
    }
}
