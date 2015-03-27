namespace Muffin.Core.Models
{
	/// <summary>
	/// Search Umbraco database..
	/// </summary>
    public class SearchModel : CollectionModel //wrapper model for searchpage results
    {
        public string Query { get; private set; }

        public SearchModel(
            IModel source,
            string query)
			: base(source, 
				source.Repository.Find(string.IsNullOrWhiteSpace(query) ? "<NOT>" : query))

        {
            Query = string.IsNullOrWhiteSpace(query) ? "<NOT>" : query;
        }
    }
}