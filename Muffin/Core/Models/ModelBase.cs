using System;
using System.Collections.Generic;
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
    public class ModelBase : PublishedContentModel, IModel //todo: return TM models instead of IModel for the properties
    {
        public ISiteRepository Repository
        {
            get { return DependencyResolver.Current.GetService<ISiteRepository>(); }
        }

        //todo: furter investigate below issue:
        //converting an IPublishedContent using .As<T> to modelbase based class when the content parameter is already loaded as a modelbase based object
        //can result in Stackoverflow exceptions, this can occur when using the CurrentContext.ContentCache
        //An exception need to be thronw in these situations, in all other situations like in a cshtml
        // one possibility could be to make Modelbase an abstract class and create a sealed class TypedModelBase : ModelBase which is used in default situations..
        //converting is allowed.
        //if(content is ModelBase)
        //throw new ApplicationException("Conversion!!");

        public ModelBase(IPublishedContent content)
            : base(content) { }

        public bool IsNull()
        {
            return false;
        }

        [DittoIgnore]
        public IModel Homepage
        {
            get
            {
                var home = Content.AncestorOrSelf(1);

                if (home is IModel)
                    return home as IModel;

                return home.As<ModelBase>();
            }
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
                  .Where(a => a.IsVisible()).Select(b => new ModelBase(b));
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
            get
            {
                return (from item in Children where item.IsVisible() select item.As<ModelBase>());
            }
        }

        [DittoIgnore]
        public override string Url
        {
            get { return Repository.FriendlyUrl(Content); }
        }
    }


}
