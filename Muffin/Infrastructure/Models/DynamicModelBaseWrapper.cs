using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using Muffin.Core;
using Muffin.Core.Models;
using umbraco.MacroEngines;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using DynamicNull = Umbraco.Core.Dynamics.DynamicNull;

namespace Muffin.Infrastructure.Models
{
    /// <summary>
	/// Dynamic wrapper for IModels. Implements the same interfase as ModelBase
	/// And is using the power of DynamicPublishedContent to access document type properties.
	/// </summary>
    internal class DynamicModelBaseWrapper : DynamicObject, IModel
    {
        protected IModel Source;

        public ISiteRepository Repository => DependencyResolver.Current.GetService<ISiteRepository>();

        internal DynamicModelBaseWrapper(IModel source)
        {
            if(source is DynamicModelBaseWrapper)
                throw new ArgumentException("You try to declare a dynamicmodelbasewrapper with a source of the same type.");

            Source = source;
        }

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
		    var dyn = new DynamicPublishedContent(Source); //todo: this needs performance optimization.
            if(dyn.TryGetMember(binder, out result))
            {
                var prop = dyn.GetProperty(binder.Name, true);

                if (prop != null) //if this is a known property use a default IConverter to convert the propertyvalue.
                {
                    result = Repository.ConvertPropertyValue(dyn.ContentType.GetPropertyType(prop.PropertyTypeAlias).PropertyEditorAlias, result);
                } 
                else //be sure an available property is used when this one exsist.
                {
                    var typedprop = this.GetType().GetProperty(binder.Name);
                    result = typedprop != null ? typedprop.GetValue(this, null) : result;
                }

                return true;
            }

		    result = DynamicNull.Null;
		    return true; //dynamic null is a succesfull value;
		}


        public virtual DateTime PublishDate //late binding of the publishdate...
		{
			get { return Source.PublishDate; }
		}

		public new IModel Parent
		{
			get
			{
				return Source.Parent;
			}
		}

		#region children

        IPublishedContent IPublishedContent.Parent
        {
            get { return Parent; }
        }

        public virtual IEnumerable<IModel> Children
        {
            get { return Source.Children; }
        }

		IEnumerable<IPublishedContent> IPublishedContent.Children
		{
            get { return Source.Children; }
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

		public virtual PublishedContentType ContentType
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
