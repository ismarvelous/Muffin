﻿using System.Collections;
using System.Dynamic;
using Umbraco.Core.Dynamics;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace Muffin.Core.Models
{
	/// <summary>
	/// Null media item, returns default image for the url.
	/// </summary>
    public class DynamicNullMedia : DynamicObject, IEnumerable, ICropImageModel
	{
		//Same usage as UmbracoCore DynamicNull
		public static readonly DynamicNullMedia Null = new DynamicNullMedia(DynamicNull.Null);

		private readonly DynamicNull _dynamicNull;

		private DynamicNullMedia(DynamicNull dn)
		{
			_dynamicNull = dn;
		}

		public virtual string Url => Settings.EmptyImageUrl;

	    public virtual bool IsNull()
		{
			return true;
		}

		public virtual IEnumerator GetEnumerator()
		{
			return _dynamicNull.GetEnumerator();
		}

		public virtual string ToHtmlString()
		{
		    return ToString();
		}

		//DynamicNull proxy functions
		public DynamicNull ToContentSet()
		{
			return _dynamicNull.ToContentSet();
		}

		public int Count()
		{
			return _dynamicNull.Count();
		}

		public bool HasValue()
		{
			return _dynamicNull.HasValue();
		}

		//Dynamic Object

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			return _dynamicNull.TryGetMember(binder, out result);
		}

		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			return _dynamicNull.TryGetIndex(binder, indexes, out result);
		}

		public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
		{
			return _dynamicNull.TryInvoke(binder, args, out result);
		}

		//base functions

		public override string ToString()
		{
		    return Url;
		}

        public virtual IUrlModel this[int width, int height] => new UrlModel
        {
            Url = Url.GetCropUrl(height: height, width: width, imageCropMode: ImageCropMode.Crop)
        };

	    public IUrlModel this[string alias] => Null;
	}
}
