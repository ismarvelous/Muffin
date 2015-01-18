using Muffin.Core;
using Muffin.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using umbraco.dialogs;
using Umbraco.Web;

namespace Example.Implementation.ViewModels
{
    public class Content : Base
    {
        public Content(IPublishedContent content)
            : base(content)
        {

        }

        public ICropImageModel CroppedImage { get; set; }
        public ICropImageModel Afbeelding { get; set; }
        public string Intro { get; set; }

        public GridModel Grid { get; set; }

    }

    public class Homepage : Base
    {
        public Homepage(IPublishedContent content)
            : base (content)
        {
            
        }

        public IEnumerable<LinkModel> SocialNetworks { get; set; } 
    }

    public class Base : ModelBase
    {
        public Base(IPublishedContent content)
            :base (content)
        {
            
        }

        public string Titel { get; set; }
        public string BrowserTitel { get; set; }
        public string MetaDescription { get; set; }

        public bool UmbracoNaviHide { get; set; }
    }
}