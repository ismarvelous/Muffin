﻿using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;

namespace Muffin.Core.Models
{
	/// <summary>
	/// "View"model used by the container controller
	/// </summary>
	public class DynamicCollectionModel : DynamicModel, IPager, IEnumerable<DynamicModel>
	{
		public IEnumerable<DynamicModel> Container { get; private set; }

		public DynamicCollectionModel(IPublishedContent source, ISiteRepository repository, IPublishedContent containerParent)
			: base(source, repository)
		{
			Container = containerParent.Children.Select(n => new DynamicModel(n, Repository));
		}

	    /// <summary>
	    /// Dynamic Model Collection used for list view pages..
	    /// </summary>
	    /// <param name="source">Published content showing all data.</param>
	    /// <param name="repository"></param>
	    /// <param name="container">Container containing all list items</param>
	    public DynamicCollectionModel(IPublishedContent source, ISiteRepository repository, IEnumerable<IPublishedContent> container)
			:base(source, repository)
		{
			Container = container.Select(n => new DynamicModel(n, Repository));
		}

		public Func<IEnumerable<DynamicModel>> PagedResults { get; set; }

		public virtual IEnumerable<DynamicModel> Results
		{
			get
			{
			    return PagedResults == null ? Container : PagedResults();
			}
		}

		public int? _totalResults;

		public int TotalResults
		{
			get 
			{
				if (!_totalResults.HasValue)
					_totalResults = Container.Count();

				return _totalResults.Value;
			}
		}

		public IEnumerator<DynamicModel> GetEnumerator()
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
