using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace Muffin.Core.Models
{
	public class DynamicMediaModel : DynamicModel, IImageModel
	{
		private readonly string _url;

		public DynamicMediaModel(IPublishedContent source, ISiteRepository repository)
			: base(source, repository)
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

        public string ToHtmlString()
        {
            return ToString();
        }
    }
}
