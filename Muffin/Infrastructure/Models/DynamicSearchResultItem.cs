using Muffin.Core.Models;

namespace Muffin.Infrastructure.Models
{
	/// <summary>
	/// Result item contains highlightext, used by the repository
	/// </summary>
    internal class DynamicSearchResultItem : DynamicModelBaseWrapper 
    {

        internal DynamicSearchResultItem(IModel source)
            : base(source)
        {
        }

        public string HighlightText { get; set; }
    }
}
