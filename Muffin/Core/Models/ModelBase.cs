﻿using System;
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
    public class ModelBase : PublishedContentModel, IModel
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

        public IModel Homepage
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

        public IEnumerable<IModel> Breadcrumbs
        {
            get
            {
                return this.Ancestors().OrderBy("level")
                  .Where(a => a.IsVisible()).As<ModelBase>();
            }
        }

        public new IModel Parent
        {
            get { return base.Parent.As<ModelBase>(); }
        }

        public new IEnumerable<IModel> Children
        {
            get { return base.Children.As<ModelBase>(); }
        }

        public IEnumerable<IModel> NavigationChildren
        {
            get
            {
                foreach (var item in this.Children)
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
