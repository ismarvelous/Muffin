using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muffin.Core.Models;

namespace Muffin.Core
{
    public static class ModelExtensions //todo: move to different file
    {
        public static DynamicModelBaseWrapper AsDynamic(this IModel model)
        {
            var wrapper = model as DynamicModelBaseWrapper;
            return wrapper ?? new DynamicModelBaseWrapper(model);
        }

        public static IEnumerable<DynamicModelBaseWrapper> AsDynamic(this IEnumerable<IModel> model)
        {
            return model.Select(itm => itm.AsDynamic());
        }
    }
}
