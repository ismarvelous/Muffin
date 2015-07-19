using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muffin.Core.Models;

namespace Muffin.Core.ViewModels
{
    public interface ICollectionContentViewModel<out T> : IEnumerable<IModel> 
        where T : IModel
    {
        T Content { get; }

        IEnumerable<IModel> Container { get; }

        /// <summary>
        /// Add a custom implementation for the paged results..
        /// </summary>
        Func<IEnumerable<IModel>> PagedResults { get; set; }

        /// <summary>
        /// returns all results, or when PagedResults is set; it retuns a selection of the results.
        /// </summary>
        IEnumerable<IModel> Results { get; }

        /// <summary>
        /// Total count of all results.
        /// Not the same as Results.Count, because Results contains the paged results.
        /// </summary>
        int TotalResults { get; }

        /// <summary>
        /// Set by the controller
        /// </summary>
        int CurrentPage { get; set; }

        /// <summary>
        /// Set by the controller
        /// </summary>
        int PageSize { get; set; }

        int TotalPages { get; }
    }

    public class CollectionContentViewModel<T> : ContentViewModel<T>, ICollectionContentViewModel<T>
        where T : IModel
	{
        protected IModel Source;
		public IEnumerable<IModel> Container { get; private set; }

	    public CollectionContentViewModel(T source)
	        : this(source, source)
	    {
	        
	    }

		public CollectionContentViewModel(T source, IModel containerParent)
			: this(source, containerParent.Children)
		{
		}

	    /// <summary>
	    /// Dynamic Model Collection used for list view pages..
	    /// </summary>
	    public CollectionContentViewModel(T source, IEnumerable<IModel> container)
            : base(source)
	    {
	        Source = source;
	        Container = container;
	    }

		public Func<IEnumerable<IModel>> PagedResults { get; set; }

        public virtual IEnumerable<IModel> Results
		{
			get
			{
			    return PagedResults == null ? Container : PagedResults();
			}
		}

	    private int? _totalResults;

		public int TotalResults
		{
			get 
			{
				if (!_totalResults.HasValue)
					_totalResults = Container.Count();

				return _totalResults.Value;
			}
		}

        public IEnumerator<IModel> GetEnumerator()
		{
			return Results.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public int CurrentPage { get; set; }

		public int PageSize { get; set; }

		public int TotalPages
		{
			get
			{
				var ret = (int)Math.Ceiling((double)TotalResults / PageSize);
				return ret < 1 ? 1 : ret;
			}
		}
	}
}
