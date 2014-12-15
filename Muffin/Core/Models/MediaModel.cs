using Umbraco.Core.Dynamics;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace Muffin.Core.Models
{
    /// <summary>
    /// For media items, supports cropping of images.
    /// </summary>
	public class MediaModel : ModelBase, ICropImageModel
	{
		private readonly string _url;

		public MediaModel(IPublishedContent source)
			: base(source)
		{
			_url = source.Url;
		}

		public override string Url
		{
			get
			{
				return _url;
			}
		}

		public override string ToString() //todo: add support for multiple media types (not only default images)
		{
			return this.Url;
		}

	    public IUrlModel this[int width, int height]
	    {
	        get
	        {
                return new UrlModel
                {
                    Url = Url.GetCropUrl(height: height, width: width, imageCropMode:ImageCropMode.Crop)
                };
	        }
	    }

        public new IUrlModel this[string alias]
        {
            get { return DynamicNullMedia.Null; } //todo: define aliases in web.config!
        }

        public string ToHtmlString()
        {
            return ToString();
        }
    }
}
