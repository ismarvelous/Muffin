using Muffin.Core;
using Muffin.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Muffin.Infrastructure.Converters;
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

        [TypeConverter(typeof(ImageCropper))]
        public ICropImageModel CroppedImage { get; set; }

        [TypeConverter(typeof(MediaPicker))]
        public ICropImageModel Afbeelding { get; set; }
        public string Intro { get; set; }

        [TypeConverter(typeof(Grid))]
        public GridModel Grid { get; set; }

    }

    public class Homepage : Base
    {
        public Homepage(IPublishedContent content)
            : base (content)
        {
            
        }

        [TypeConverter(typeof(RelatedLinks))]
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