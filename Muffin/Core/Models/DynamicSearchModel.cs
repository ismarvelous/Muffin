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
            ModelBase source,
            string query)
			: base(source, 
				source.Repository.Find(string.IsNullOrWhiteSpace(query) ? "<NOT>" : query))

        {
            Query = string.IsNullOrWhiteSpace(query) ? "<NOT>" : query;
        }
    }
}