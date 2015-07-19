using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
                return DependencyResolver.Current.GetService<IPublishedContentModelFactory>();
            }
        }

        public ModelBase(IPublishedContent content)
            : base(content)
        {

            if (this.GetType().IsAssignableFrom(content.GetType()))
                throw new ArgumentException("You can not define a ModelBase using a content type of the same type.");
        }

        public bool IsNull()
        {
            return false;
        }

        private IModel _homepage;
        [MuffinIgnore]
        public IModel Homepage
        {
            get
            {
                if (_homepage == null)
                {
                    var home = Content.AncestorOrSelf(1);

                    if (home is IModel) //for safety reasons
                        _homepage = home as IModel;
                    else
                        _homepage = ContentFactory.CreateModel(home) as IModel;
                }

                return _homepage;
            }
        }

        [MuffinIgnore]
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
        [MuffinIgnore]
        public IEnumerable<IModel> Breadcrumbs
        {
            get
            {
                return _breadcrumbs ?? (_breadcrumbs = this.Ancestors().OrderBy("level")
                    .Where(a => a.IsVisible()).Select(c => ContentFactory.CreateModel(c)).OfType<IModel>());
            }
        }

        private IModel _parent;
        [MuffinIgnore]
        public new IModel Parent
        {
            get
            {
                if (_parent == null)
                {
                    var model = base.Parent as IModel;
                    if (model != null) //for safety reasons
                        _parent = model;
                    else
                        _parent = ContentFactory.CreateModel(base.Parent) as IModel;
                }

                return _parent;
            }
        }

        private IEnumerable<IModel> _children;
        [MuffinIgnore]
        public new IEnumerable<IModel> Children
        {
            get {
                return _children ??
                       (_children = base.Children.Select(c => ContentFactory.CreateModel(c)).OfType<IModel>());
            }
        }

        private IEnumerable<IModel> _navigationChildren;

        [MuffinIgnore]
        public IEnumerable<IModel> NavigationChildren
        {
            get { return _navigationChildren ?? (_navigationChildren = this.Children.Where(c => c.IsVisible())); }
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
