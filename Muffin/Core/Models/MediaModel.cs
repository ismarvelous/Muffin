using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Dynamics;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace Muffin.Core.Models
{
    /// <summary>
    /// For media items, supports cropping of images.
    /// </summary>
	public class MediaModel : PublishedContentModel, ICropImageModel, IModel
	{
        protected ISiteRepository Repository => DependencyResolver.Current.GetService<ISiteRepository>();
        protected IPublishedContentModelFactory ContentFactory => DependencyResolver.Current.GetService<IPublishedContentModelFactory>();

        private readonly string _url;

		public MediaModel(IPublishedContent source)
			: base(source)
		{
			_url = source.Url;
		}

		public override string Url => _url;

        public override string ToString() //todo: add support for multiple media types (not only default images)
		{
			return this.Url;
		}

	    public IUrlModel this[int width, int height] => new UrlModel
	    {
	        Url = Url.GetCropUrl(height: height, width: width, imageCropMode:ImageCropMode.Crop)
	    };

        public new IUrlModel this[string alias] => DynamicNullMedia.Null;

        public string ToHtmlString()
        {
            return ToString();
        }

        public bool IsNull()
        {
            return false;
        }

        [MuffinIgnore]
        public DateTime PublishDate
        {
            get
            {
                var content = Repository.FindContentById(this.Id);
                var ret = content.ReleaseDate;

                return ret ?? UpdateDate;
            }
        }

        private IModel _parent;
        [MuffinIgnore]
        public new IModel Parent
        {
            get
            {
                if (_parent == null)
                {
                    var model = base.Parent as IModel;
                    if (model != null) //for safety reasons
                        _parent = model;
                    else
                        _parent = ContentFactory.CreateModel(base.Parent) as IModel;
                }

                return _parent;
            }
        }

        private IEnumerable<IModel> _children;
        [MuffinIgnore]
        public new IEnumerable<IModel> Children
        {
            get
            {
                return _children ?? (_children = base.Children.Select(c => ContentFactory.CreateModel(c)).OfType<IModel>());
            }
        }
    }
}
