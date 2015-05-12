using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Muffin.Core.Models
{
	/// <summary>
	/// "View"model used by the container controller
	/// </summary>
    public class CollectionModel : IModel, IPager, IEnumerable<IModel> //Wrapper object for collection / result models
	{
        protected IPublishedContentModelFactory ContentFactory
        {
            get
            {
                return PublishedContentModelFactoryResolver.Current.Factory;
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

	    public int Id
	    {
	        get { return Source.Id; }
	    }

	    public int TemplateId
	    {
	        get { return Source.TemplateId; }
	    }

	    public int SortOrder
	    {
	        get { return Source.SortOrder; }
	    }

	    public string Name
	    {
	        get { return Source.Name; }
	    }

	    public string UrlName
	    {
	        get { return Source.UrlName; }
	    }

	    public string DocumentTypeAlias
	    {
	        get { return Source.DocumentTypeAlias; }
	    }

	    public int DocumentTypeId
	    {
	        get { return Source.DocumentTypeId; }
	    }

	    public string WriterName
	    {
	        get { return Source.WriterName; }
	    }

	    public string CreatorName
	    {
	        get { return Source.CreatorName; }
	    }

	    public int WriterId
	    {
	        get { return Source.WriterId; }
	    }

	    public int CreatorId
	    {
	        get { return Source.CreatorId; }
	    }

	    public string Path
	    {
	        get { return Source.Path; }
	    }

	    public DateTime CreateDate
	    {
	        get { return Source.CreateDate; }
	    }

	    public DateTime UpdateDate
	    {
	        get { return Source.UpdateDate; }
	    }

	    public Guid Version
	    {
	        get { return Source.Version; }
	    }

	    public int Level
	    {
	        get { return Source.Level; }
	    }

	    public string Url
	    {
	        get { return Source.Url; }
	    }

	    public PublishedItemType ItemType
	    {
	        get { return Source.ItemType; }
	    }

	    public bool IsDraft
	    {
	        get { return Source.IsDraft; }
	    }

	    public IModel Parent
	    {
	        get { return Source.Parent; }
	    }

	    public IEnumerable<IModel> Children
	    {
	        get { return Source.Children; }
	    }

	    public IEnumerable<IModel> NavigationChildren
	    {
	        get { return Source.NavigationChildren; }
	    }

	    public IEnumerable<IModel> Breadcrumbs
	    {
	        get { return Source.Breadcrumbs; }
	    }

	    public ISiteRepository Repository
	    {
	        get { return Source.Repository; }
	    }

	    public IModel Homepage
	    {
	        get { return Source.Homepage; }
	    }

	    public DateTime PublishDate
	    {
	        get { return Source.PublishDate; }
	    }

	    IPublishedContent IPublishedContent.Parent
	    {
	        get { return Parent; }
	    }

	    IEnumerable<IPublishedContent> IPublishedContent.Children
	    {
	        get { return Children; }
	    }

	    public ICollection<IPublishedProperty> Properties
	    {
	        get { return Source.Properties; }
	    }

	    public object this[string alias]
	    {
	        get { return Source[alias]; }
	    }

	    public bool IsNull()
	    {
	        return Source.IsNull();
	    }
	}
}
