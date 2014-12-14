using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Muffin.Core.Models
{
    public interface IModel<out T> : IPublishedContent, INullModel
        where T : IPublishedContent
    {
        T Homepage { get; }
        DateTime PublishDate { get; }
        IEnumerable<T> NavigationChildren { get; }
        IEnumerable<T> Breadcrumbs { get; }
    }

    public class ModelBase : PublishedContentModel, IModel<ModelBase>
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

        public ModelBase Homepage
        {
            get { return new ModelBase(Content.AncestorOrSelf(1)); }
        }

        public DateTime PublishDate
        {
            get
            {
                var content = Repository.FindContentById(this.Id);
                var ret = content.ReleaseDate;

                return ret ?? UpdateDate;
            }
        }

        public IEnumerable<ModelBase> Breadcrumbs
        {
            get
            {
                return Content.Ancestors().OrderBy("level")
                  .Where(a => a.IsVisible()).As<ModelBase>();
            }
        }

        public new IEnumerable<ModelBase> Children
        {
            get { return base.Children.As<ModelBase>(); }
        }

        public IEnumerable<ModelBase> NavigationChildren
        {
            get
            {
                foreach (var item in Content.Children)
                    if (item.IsVisible())
                        yield return new ModelBase(item);
            }
        }

        public override string Url
        {
            get { return Repository.FriendlyUrl(Content); }
        }
    }
}
