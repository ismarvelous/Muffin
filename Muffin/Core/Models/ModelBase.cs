using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Our.Umbraco.Ditto;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Muffin.Core.Models
{
    /// <summary>
    /// The foundation, PublishedContentModel base class..
    /// </summary>
    public class ModelBase : PublishedContentModel, IModel //todo: this has to be an abstract class
    {
        public ISiteRepository Repository
        {
            get { return DependencyResolver.Current.GetService<ISiteRepository>(); }
        }

        public ModelBase(IPublishedContent content) 
            : base(content)
        {
        }

        public bool IsNull()
        {
            return false;
        }

        [DittoIgnore]
        public IModel Homepage
        {
            get { return new ModelBase(Content.AncestorOrSelf(1)); }
        }

        [DittoIgnore]
        public DateTime PublishDate
        {
            get
            {
                var content = Repository.FindContentById(this.Id);
                var ret = content.ReleaseDate;

                return ret ?? UpdateDate;
            }
        }

        [DittoIgnore]
        public IEnumerable<IModel> Breadcrumbs
        {
            get
            {
                return this.Ancestors().OrderBy("level")
                  .Where(a => a.IsVisible()).As<ModelBase>();
            }
        }

        [DittoIgnore]
        public new IModel Parent
        {
            get { return base.Parent.As<ModelBase>(); }
        }

        [DittoIgnore]
        public new IEnumerable<IModel> Children
        {
            get { return base.Children.As<ModelBase>(); }
        }

        [DittoIgnore]
        public IEnumerable<IModel> NavigationChildren
        {
            get {
                return (from item in Children where item.IsVisible() select new ModelBase(item));
            }
        }

        [DittoIgnore]
        public override string Url
        {
            get { return Repository.FriendlyUrl(Content); }
        }
    }
}
