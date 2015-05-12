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

        protected IPublishedContentModelFactory ContentFactory
        {
            get
            {
                return PublishedContentModelFactoryResolver.Current.Factory;
            }
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
            : base(content) {

                if (this.GetType().IsAssignableFrom(content.GetType()))
                    throw new ArgumentException("You can not define a ModelBase using a content type of the same type.");
        }

        public bool IsNull()
        {
            return false;
        }

        private IModel _homepage;
        [DittoIgnore]
        public IModel Homepage
        {
            get
            {
                if(_homepage == null)
                {
                    var home = Content.AncestorOrSelf(1);

                    if(home is IModel) //for safety reasons
                        _homepage = home as IModel;
                    else
                        _homepage = ContentFactory.CreateModel(home) as IModel;
                }

                return _homepage;
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

        private IEnumerable<IModel> _breadcrumbs;
        [DittoIgnore]
        public IEnumerable<IModel> Breadcrumbs
        {
            get
            {
                if(_breadcrumbs == null)
                {
                    _breadcrumbs = this.Ancestors().OrderBy("level")
                        .Where(a => a.IsVisible()).Select(c => ContentFactory.CreateModel(c) as IModel);
                }

                return _breadcrumbs;
            }
        }

        private IModel _parent;
        [DittoIgnore]
        public new IModel Parent
        {
            get 
            {
                if(_parent == null)
                {
                    if (base.Parent is IModel) //for safety reasons
                        _parent = base.Parent as IModel;
                    else
                        _parent = ContentFactory.CreateModel(base.Parent) as IModel;
                }

                return _parent;
            }
        }

        private IEnumerable<IModel> _children;
        [DittoIgnore]
        public new IEnumerable<IModel> Children
        {
            get 
            { 
                if(_children == null)
                {
                    _children = base.Children.Select(c => ContentFactory.CreateModel(c) as IModel);
                }

                return _children;
            }
        }

        private IEnumerable<IModel> _navigationChildren;

        [DittoIgnore]
        public IEnumerable<IModel> NavigationChildren
        {
            get
            {
                if(_navigationChildren == null)
                {
                    _navigationChildren = this.Children.Where(c => c.IsVisible());
                }

                return _navigationChildren;
            }
        }

        //[DittoIgnore]
        //public override string Url
        //{
        //    get 
        //    {
        //        return Repository.FriendlyUrl(Content); 
        //    }
        //}
    }


}
