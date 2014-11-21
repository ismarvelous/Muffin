using Umbraco.Core.Models;

namespace Muffin.Core.Models
{
	public class DynamicMediaModel : DynamicModel
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

		public override string ToString()
		{
			return this.Url;
		}
	}
}
