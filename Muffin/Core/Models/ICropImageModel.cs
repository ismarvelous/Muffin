using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Muffin.Core.Models
{
    public interface ICropImageModel : IUrlModel, IHtmlString, INullModel
    {
        IUrlModel this[int width, int height] { get; }
        IUrlModel this[string alias] { get; }
    }
}
