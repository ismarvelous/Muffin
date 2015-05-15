using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muffin.Core.Models;

namespace Muffin.Core.ViewModels
{
    public interface IContentViewModel<out T>
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
        }

        public T Content { get; set; }
    }
}
