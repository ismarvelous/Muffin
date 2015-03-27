using System;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace Muffin.Core.Models
{
    public interface IModel : IPublishedContent, INullModel
    {
        IModel Homepage { get; }
        DateTime PublishDate { get; }
        new IModel Parent { get; }
        new IEnumerable<IModel> Children { get; }
        IEnumerable<IModel> NavigationChildren { get; }
        IEnumerable<IModel> Breadcrumbs { get; }
        ISiteRepository Repository { get; }

        //todo: UmbracoNaviHide
    }
}
