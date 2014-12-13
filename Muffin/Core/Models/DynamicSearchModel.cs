using System.Linq;
using Umbraco.Core.Models;

namespace Muffin.Core.Models
{
	/// <summary>
	/// Search "view" model used by the searchcontroller
	/// </summary>
    public class DynamicSearchModel : DynamicCollectionModel 
    {
        public string Query { get; private set; }

        public DynamicSearchModel(
            IPublishedContent source,
            ISiteRepository repository,
            string query)
			: base(source, repository, 
				repository.Find(string.IsNullOrWhiteSpace(query) ? "<NOT>" : query))

        {
            Query = string.IsNullOrWhiteSpace(query) ? "<NOT>" : query;
        }
    }
}