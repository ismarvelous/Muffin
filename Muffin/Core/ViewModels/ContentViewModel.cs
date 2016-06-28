using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Muffin.Core.Models;
using Umbraco.Web;

namespace Muffin.Core.ViewModels
{
    public interface IPageViewModel
    {
        IModel Homepage { get; }
        IEnumerable<IModel> NavigationChildren { get; }
        IEnumerable<IModel> Breadcrumbs { get; }
        ISiteRepository Repository { get; }
    }

    public interface IContentViewModel<out T> : IPageViewModel
        where T : IModel
    {
        T Content { get; }
    }

    public class ContentViewModel<T> :
        IContentViewModel<T> where T : IModel
    {
        public ContentViewModel(T currentContent)
        {
            Content = currentContent;

            //todo:

            Homepage = Content.AncestorOrSelf(1) as IModel;
            NavigationChildren = Enumerable.Empty<IModel>();
            Breadcrumbs = Enumerable.Empty<IModel>();
            Repository = DependencyResolver.Current.GetService<ISiteRepository>();
        }



        public T Content { get; set; }
        public IModel Homepage { get; }
        public IEnumerable<IModel> NavigationChildren { get; }
        public IEnumerable<IModel> Breadcrumbs { get; }
        public ISiteRepository Repository { get; }
    }
}
