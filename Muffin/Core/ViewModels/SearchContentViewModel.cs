using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muffin.Core.Models;

namespace Muffin.Core.ViewModels
{
    public interface ISearchContentViewModel<out T> : ICollectionContentViewModel<T> 
        where T : IModel
    {
        string Query { get; }
    }

    public class SearchContentViewModel<T> : CollectionContentViewModel<T>, ISearchContentViewModel<T>
        where T : IModel
    {
        public string Query { get; private set; }

        public SearchContentViewModel(
            T source,
            string query)
			: base(source, 
				source.Repository.Find(string.IsNullOrWhiteSpace(query) ? "<NOT>" : query))

        {
            Query = string.IsNullOrWhiteSpace(query) ? "<NOT>" : query;
        }
    }
}
