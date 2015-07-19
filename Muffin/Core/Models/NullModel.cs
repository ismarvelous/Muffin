using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Dynamics;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Muffin.Core.Models
{
    /// <summary>
    /// Null "IModel" item, returns default image for the url.
    /// This model acts like an IModel / IPublishedContent
    /// </summary>
    public class NullModel : IModel
    {
        //Same usage as UmbracoCore DynamicNull
        public static readonly NullModel Null = new NullModel(DynamicNull.Null);

        private readonly DynamicNull _dynamicNull;

        private NullModel(DynamicNull dn)
        {
            _dynamicNull = dn;
        }

        public int GetIndex()
        {
            return -1;
        }

        public IPublishedProperty GetProperty(string alias)
        {
            return null;
        }

        public IPublishedProperty GetProperty(string alias, bool recurse)
        {
            return null;
        }

        public IEnumerable<IPublishedContent> ContentSet
        {
            get { return new List<IPublishedContent>(); }
        }

        public PublishedContentType ContentType
        {
            get { return null; }
        }

        public int Id
        {
            get { return GetIndex(); }
        }

        public int TemplateId
        {
            get { return -1; }
        }

        public int SortOrder
        {
            get { return 0; }
        }

        public string Name
        {
            get { return string.Empty; }
        }

        public string UrlName
        {
            get { return string.Empty; }
        }

        public string DocumentTypeAlias
        {
            get { return string.Empty; }
        }

        public int DocumentTypeId
        {
            get { return -1; }
        }

        public string WriterName
        {
            get { return string.Empty; }
        }

        public string CreatorName
        {
            get { return string.Empty; }
        }

        public int WriterId
        {
            get { return -1; }
        }

        public int CreatorId
        {
            get { return -1; }
        }

        public string Path
        {
            get { return string.Empty; }
        }

        public DateTime CreateDate
        {
            get { return DateTime.Now; }
        }

        public DateTime UpdateDate
        {
            get { return DateTime.Now; }
        }

        public Guid Version
        {
            get { return Guid.Empty; }
        }

        public int Level
        {
            get { return -1; }
        }

        public string Url
        {
            get { return string.Empty; }
        }

        public PublishedItemType ItemType
        {
            get { return PublishedItemType.Content; }
        }

        public bool IsDraft
        {
            get { return false; }
        }

        public IModel Parent
        {
            get { return new NullModel(_dynamicNull); }
        }

        public IEnumerable<IModel> Children
        {
            get { return new List<IModel>(); }
        }

        public IEnumerable<IModel> NavigationChildren
        {
            get { return new List<IModel>(); }
        }

        public IEnumerable<IModel> Breadcrumbs
        {
            get { return new List<IModel>(); }
        }

        public ISiteRepository Repository
        {
            get { return DependencyResolver.Current.GetService<ISiteRepository>(); }
        }

        public IModel Homepage
        {
            get { return new NullModel(_dynamicNull); }
        }

        public DateTime PublishDate
        {
            get { return DateTime.Now; }
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
            get { return new List<IPublishedProperty>(); }
        }

        public object this[string alias]
        {
            get { return null; }
        }

        public bool IsNull()
        {
            return true;
        }
    }
}
