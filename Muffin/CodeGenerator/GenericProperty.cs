using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muffin.CodeGenerator
{
    [Serializable()]
    public class GenericProperty
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
