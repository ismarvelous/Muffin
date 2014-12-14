using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Umbraco.Core.Dynamics;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace Muffin.Core.Models
{
    /// <summary>
	/// Dynamic wrapper for the Modelbase. Implements the same interfase as ModelBase
	/// And is using the power of DynamicPublishedContent to access document type properties.
	/// </summary>
	public class DynamicModelBaseWrapper : DynamicObject, IModel<DynamicModelBaseWrapper>
    {
        protected ModelBase Source;

        public ISiteRepository Repository { get; protected set; }

        public DynamicModelBaseWrapper(ModelBase source)
        {
            Source = source;
            Repository = source.Repository;
        }

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
            var dyn = Source.AsDynamic() as DynamicPublishedContent;
            if(dyn != null && dyn.TryGetMember(binder, out result))
		    {
		        return true;
		    }

		    result = DynamicNull.Null;
		    return true; //dynamic null is a succesfull value;
		}

		public virtual DynamicModelBaseWrapper Homepage
		{
			get { return new DynamicModelBaseWrapper(Source.Homepage); }
		}

		public virtual DateTime PublishDate //late binding of the publishdate...
		{
			get { return Source.PublishDate; }
		}

		IPublishedContent IPublishedContent.Parent
		{
			get
			{
				return Source.Parent;
			}
		}

		public virtual DynamicModelBaseWrapper Parent
		{
			get
			{
				return new DynamicModelBaseWrapper(new ModelBase(Source.Parent));
			}
		}

		#region children

        public virtual IEnumerable<DynamicModelBaseWrapper> NavigationChildren
        {
            get { return Source.NavigationChildren.Select(itm => new DynamicModelBaseWrapper(itm)); }
        }

        public virtual IEnumerable<DynamicModelBaseWrapper> Children
        {
            get { return Source.Children.As<DynamicModelBaseWrapper>(); }
        }

		IEnumerable<IPublishedContent> IPublishedContent.Children
		{
			get { return Source.Children; }
		}

        public virtual IEnumerable<DynamicModelBaseWrapper> Breadcrumbs
        {
            get { return Source.NavigationChildren.Select(itm => new DynamicModelBaseWrapper(itm));  }
        }

		#endregion

		public virtual bool IsNull()
		{
			return false;
		}

		public virtual IEnumerable<IPublishedContent> ContentSet
		{
			get { return Source.ContentSet; }
		}

		public virtual global::Umbraco.Core.Models.PublishedContent.PublishedContentType ContentType
		{
            get { return Source.ContentType; }
		}

		public virtual DateTime CreateDate
		{
            get { return Source.CreateDate; }
		}

		public virtual int CreatorId
		{
            get { return Source.CreatorId; }
		}

		public virtual string CreatorName
		{
            get { return Source.CreatorName; }
		}

		public virtual string DocumentTypeAlias
		{
            get { return Source.DocumentTypeAlias; }
		}

		int IPublishedContent.DocumentTypeId
		{
            get { return Source.DocumentTypeId; }
		}

		public virtual int GetIndex()
		{
            return Source.GetIndex();
		}

		IPublishedProperty IPublishedContent.GetProperty(string alias, bool recurse)
		{
            return Source.GetProperty(alias, recurse);
		}

		IPublishedProperty IPublishedContent.GetProperty(string alias)
		{
            return Source.GetProperty(alias);
		}

		public virtual int Id
		{
            get { return Source.Id; }
		}

		bool IPublishedContent.IsDraft
		{
            get { return Source.IsDraft; }
		}

		public virtual PublishedItemType ItemType
		{
            get { return Source.ItemType; }
		}

		public virtual int Level
		{
            get { return Source.Level; }
		}

		public virtual string Name
		{
            get { return Source.Name; }
		}

		public virtual string Path
		{
            get { return Source.Path; }
		}

		ICollection<IPublishedProperty> IPublishedContent.Properties
		{
            get { return Source.Properties; }
		}

		public virtual int SortOrder
		{
            get { return Source.SortOrder; }
		}

		public virtual int TemplateId
		{
            get { return Source.TemplateId; }
		}

		public virtual DateTime UpdateDate
		{
            get { return Source.UpdateDate; }
		}

		public virtual string Url
		{
			get { return Source.Url; }
		}

		public virtual string UrlName
		{
			get { return Source.UrlName; }
		}

		public virtual Guid Version
		{
			get { return Source.Version; }
		}

		public virtual int WriterId
		{
            get { return Source.WriterId; }
		}

		public virtual string WriterName
		{
            get { return Source.WriterName; }
		}

		public virtual object this[string alias]
		{
            get { return Source[alias]; }
		}

		public virtual bool IsVisible()
		{
            return Source.IsVisible();
		}
	}
}
