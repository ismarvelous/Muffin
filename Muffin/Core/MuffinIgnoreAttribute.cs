using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muffin.Core
{
    /// <summary>
    /// The Ditto ignore property attribute. Used for specifying that Umbraco should
    /// ignore this property during conversion.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MuffinIgnoreAttribute : Attribute
    {
    }
}
