using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Muffin.Core;
using Muffin.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.ModelsBuilder;
using Umbraco.Web;

// ReSharper disable ConvertPropertyToExpressionBody

namespace Example.Implementation.Models
{
    public partial class Base : IModel
    {
        //todo: add support for the castle factory / normal type converters...
        [ImplementPropertyType("afbeelding")]
        public object Afbeelding
        {
            get { return this.GetPropertyValue<Muffin.Core.Models.ICropImageModel>("afbeelding"); }
        }

        protected ISiteRepository Repository
        {
            get { return DependencyResolver.Current.GetService<ISiteRepository>(); }
        }

        protected IPublishedContentModelFactory ContentFactory
        {
            get { return DependencyResolver.Current.GetService<IPublishedContentModelFactory>(); }
        }

        public bool IsNull()
        {
            return false;
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
            get
            {
                return _children ?? (_children = base.Children.Select(c => ContentFactory.CreateModel(c)).OfType<IModel>());
            }
        }
    }

    public partial class ContainerItemBase : IModel
    {
        protected ISiteRepository Repository
        {
            get { return DependencyResolver.Current.GetService<ISiteRepository>(); }
        }

        protected IPublishedContentModelFactory ContentFactory
        {
            get { return DependencyResolver.Current.GetService<IPublishedContentModelFactory>(); }
        }

        public bool IsNull()
        {
            return false;
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
            get
            {
                return _children ?? (_children = base.Children.Select(c => ContentFactory.CreateModel(c)).OfType<IModel>());
            }
        }
    }

    public partial class ContainerBase : IModel
    {
        protected ISiteRepository Repository
        {
            get { return DependencyResolver.Current.GetService<ISiteRepository>(); }
        }

        protected IPublishedContentModelFactory ContentFactory
        {
            get { return DependencyResolver.Current.GetService<IPublishedContentModelFactory>(); }
        }

        public bool IsNull()
        {
            return false;
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
            get
            {
                return _children ?? (_children = base.Children.Select(c => ContentFactory.CreateModel(c)).OfType<IModel>());
            }
        }
    }
}