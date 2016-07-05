using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Muffin.Core;
using Muffin.Core.Models;
using Muffin.Infrastructure.Converters;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.ModelsBuilder;

// ReSharper disable ConvertPropertyToExpressionBody

namespace Example.Implementation.Models
{
    public partial class Base : IModel
    {
        [ImplementPropertyType("afbeelding")] //this avoids creation of the property by the builder.
        //use your own type converter...[TypeConverter(typeof(YourOwnLocalConverter))] 
        //or use the default stuff..
        public virtual ICropImageModel Afbeelding
        {
            get; set; //when using get; and set; the castle factory handles all the magic
            // when set; is not implemented, it ignores the property
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