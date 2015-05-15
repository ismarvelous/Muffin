using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Muffin.Core.Models
{
	/// <summary>
	/// "View"model used by the container controller
	/// </summary>
    public class CollectionModel : IPager, IEnumerable<IModel> //Wrapper object for collection / result models
	{
        protected IPublishedContentModelFactory ContentFactory
        {
            get
            {
                return DependencyResolver.Current.GetService<IPublishedContentModelFactory>();
            }
        }

        protected IModel Source;
		public IEnumerable<IModel> Container { get; private set; }

	    public CollectionModel(IModel source)
	        : this(source, source)
	    {
	        
	    }

		public CollectionModel(IModel source, IPublishedContent containerParent)
			: this(source, containerParent.Children)
		{
		}

	    /// <summary>
	    /// Dynamic Model Collection used for list view pages..
	    /// </summary>
	    public CollectionModel(IModel source, IEnumerable<IPublishedContent> container)
	    {
	        Source = source;
	        Container = container.Select(c => ContentFactory.CreateModel(c) as IModel);
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

	    public int GetIndex()
	    {
	        return Source.GetIndex();
	    }

	    public IPublishedProperty GetProperty(string alias)
	    {
	        return Source.GetProperty(alias);
	    }

	    public IPublishedProperty GetProperty(string alias, bool recurse)
	    {
	        return Source.GetProperty(alias, recurse);
	    }

	    public IEnumerable<IPublishedContent> ContentSet
	    {
	        get { return Source.ContentSet; }
	    }

	    public PublishedContentType ContentType
	    {
	        get { return Source.ContentType; }
	    }
	}
}
