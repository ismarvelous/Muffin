//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Muffin.Core;
//using umbraco.editorControls;
//using umbraco.interfaces;
//using Umbraco.Core;
//using Umbraco.Core.Persistence;
//using Umbraco.Core.Services;

//namespace Muffin.CodeGenerator
//{
//    public class Application : UmbracoApplicationBase
//    {
//        public string BaseDirectory { get; private set; }
//        public string DataDirectory { get; private set; }
//        protected readonly string AppPath;

//        public Application(string appPath)
//        {
//            AppPath = Path.GetDirectoryName(appPath);
//        }


//        protected override IBootManager GetBootManager()
//        {
//            var binDirectory = new DirectoryInfo(AppPath);
//            BaseDirectory = ResolveBasePath(binDirectory);
//            DataDirectory = Path.Combine(BaseDirectory, "app_data");
//            var appDomainConfigPath = new DirectoryInfo(Path.Combine(binDirectory.FullName, "config"));

//            //Copy config files to AppDomain's base directory
//            if (binDirectory.FullName.Equals(BaseDirectory) == false &&
//                appDomainConfigPath.Exists == false)
//            {
//                appDomainConfigPath.Create();
//                var baseConfigPath = new DirectoryInfo(Path.Combine(BaseDirectory, "config"));
//                var sourceFiles = baseConfigPath.GetFiles("*.config", SearchOption.TopDirectoryOnly);
//                foreach (var sourceFile in sourceFiles)
//                {
//                    sourceFile.CopyTo(sourceFile.FullName.Replace(baseConfigPath.FullName, appDomainConfigPath.FullName), true);
//                }
//            }

//            AppDomain.CurrentDomain.SetData("DataDirectory", DataDirectory);

//            return new CoreBootManager(this);
//        }

//        public void Start(object sender, EventArgs e)
//        {
//            base.Application_Start(sender, e);
//        }

//        private string ResolveBasePath(DirectoryInfo currentFolder)
//        {
//            var folders = currentFolder.GetDirectories();
//            if (folders.Any(x => x.Name.Equals("app_data", StringComparison.OrdinalIgnoreCase)) &&
//                folders.Any(x => x.Name.Equals("config", StringComparison.OrdinalIgnoreCase)))
//            {
//                return currentFolder.FullName;
//            }

//            if (currentFolder.Parent == null)
//                throw new Exception("Base directory containing an 'App_Data' and 'Config' folder was not found."+
//                    " These folders are required to run this console application as it relies on the normal umbraco configuration files.");

//            return ResolveBasePath(currentFolder.Parent);
//        }
//    }


//    //public class ConsoleBootManager : CoreBootManager
//    //{
//    //    public ConsoleBootManager(UmbracoApplicationBase umbracoApplication, string baseDirectory)
//    //        : base(umbracoApplication)
//    //    {
//    //        base.InitializeApplicationRootPath(baseDirectory);
//    //    }
//    //}

//}
