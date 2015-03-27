using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muffin.CodeGenerator;

namespace CodeGenTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new ViewModelsGenerator(@"c:\projects\muffin\example\uSync");

            var docs = generator.GetDocumentTypes();

            foreach (var doc in docs)
            {
                Console.WriteLine("{0}: {1}", doc.GetSafeClassName(), doc.GetSafeBaseClassName());    
            }

            Console.ReadLine();
        }
    }
}
