using System;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace Muffin.Core.Models
{
    public interface IModel : IPublishedContent, INullModel
    {
        DateTime PublishDate { get; }
        new IModel Parent { get; }
        new IEnumerable<IModel> Children { get; }
        //ISiteRepository Repository { get; }
        //todo: UmbracoNaviHide
    }
}
