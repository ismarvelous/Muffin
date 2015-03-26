using Muffin.Core;
using Muffin.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Muffin.Infrastructure.Converters;
using Our.Umbraco.Ditto;
using Umbraco.Core.Models;
using umbraco.dialogs;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Example.Implementation.ViewModels
{
    //public partial class Content : Base
    //{
    //    public Content(IPublishedContent content)
    //        : base(content)
    //    {

    //    }

    //    [TypeConverter(typeof(ImageCropper))]
    //    public ICropImageModel Croppedimage { get; set; }

    //    [TypeConverter(typeof(MediaPicker))]
    //    public ICropImageModel Afbeelding { get; set; }
    //    public string Intro { get; set; }

    //    [TypeConverter(typeof(Grid))]
    //    public GridModel Grid { get; set; }

    //}

////    public partial class Homepage : Base
////    {
////        public Homepage(IPublishedContent content)
////            : base(content)
////        {

////        }

////        [TypeConverter(typeof(RelatedLinks))]
////        public IEnumerable<LinkModel> SocialNetworks { get; set; }
////    }

//    public partial class Base : ModelBase
//    {
//        public Base(IPublishedContent content)
//            : base(content)
//        {

//        }

//        public string Titel { get; set; }
//        public string BrowserTitel { get; set; }
//        public string MetaDescription { get; set; }

//        public bool UmbracoNaviHide { get; set; }
//    }
}