using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muffin.CodeGenerator
{
    [Serializable()]
    public class DocumentType
    {
        public Info Info { get; set; }
        public GenericProperty[] GenericProperties { get; set; }
    }

    public class Info
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Master { get; set; }
    }
}
