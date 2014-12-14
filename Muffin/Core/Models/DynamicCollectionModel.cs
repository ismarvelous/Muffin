using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;

namespace Muffin.Core.Models
{
	/// <summary>
	/// "View"model used by the container controller
	/// </summary>
    [Obsolete("We need to implement a generic class for this!!")] //todo: bad code!!!!
    public class DynamicCollectionModel : DynamicModelBaseWrapper, IPager, IEnumerable<IModel<IPublishedContent>>
	{
		public IEnumerable<ModelBase> Container { get; private set; }

		public DynamicCollectionModel(ModelBase source, IPublishedContent containerParent)
			: base(source)
		{
			Container = containerParent.Children.Select(n => new ModelBase(n)); //todo: bad code!!!!, don't use Modelbase hardcoded here..
		}

	    /// <summary>
	    /// Dynamic Model Collection used for list view pages..
	    /// </summary>
	    public DynamicCollectionModel(ModelBase source, IEnumerable<IPublishedContent> container)
			:base(source)
		{
            Container = container.Select(n => new ModelBase(n)); //todo: bad code!!!!, don't use Modelbase hardcoded here..
		}

		public Func<IEnumerable<IModel<IPublishedContent>>> PagedResults { get; set; }

        public virtual IEnumerable<IModel<IPublishedContent>> Results
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

        public IEnumerator<IModel<IPublishedContent>> GetEnumerator()
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
