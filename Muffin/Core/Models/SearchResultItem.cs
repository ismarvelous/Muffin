using Umbraco.Core.Models;

namespace Muffin.Core.Models
{
	/// <summary>
	/// Result item contains highlightext, used by the repository
	/// </summary>
    public class SearchResultItem : ModelBase 
    {

        public SearchResultItem(IPublishedContent source)
            : base(source)
        {
        }

        public string HighlightText { get; set; }
    }
}
