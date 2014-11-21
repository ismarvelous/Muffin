using Umbraco.Core.Models;

namespace Muffin.Core.Models
{
	/// <summary>
	/// Result item contains highlightext, used by the repository
	/// </summary>
    public class DynamicSearchResultItem : DynamicModel 
    {

        public DynamicSearchResultItem(IPublishedContent source, ISiteRepository repository)
            : base(source, repository)
        {
        }

        public string HighlightText { get; set; }
    }
}
