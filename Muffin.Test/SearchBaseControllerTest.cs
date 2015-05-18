using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Muffin.Controllers;
using Muffin.Core;
using Muffin.Core.Models;
using Umbraco.Web.Models;
using Umbraco.Core.Models;
using Moq;
using System.Collections.Generic;
using System.Globalization;
using Autofac.Integration.Mvc;
using System.Web.Mvc;
using System.Linq;
using Autofac;
using Muffin.Core.ViewModels;
using Umbraco.Web.WebApi;

namespace Muffin.Test
{
    [TestClass]
    public class SearchControllerTest : BaseTestClass // Naming convention: Method_to_test__State_under_test__Expected_behavior
    {
        [TestMethod]
        public void Search__NoResultsInRepository__Total_results_equals_ResultsCount()
        {
            //1. Arrange
            
            Repository.Setup(s => s.Find(It.IsAny<string>()))
                .Returns(new List<IModel>()); //mocked object
            

            var mController = new Mock<SearchBaseController>(Repository.Object) { CallBase = true }; //abstract class callBase
            var renderModel = new RenderModel(Arrange.Content().Object, CultureInfo.InvariantCulture);

            //2.Act
            var result = mController.Object.Search(renderModel,
                p: 1,
                q: "lorem") as ViewResult;

            //3. Assert.
            Assert.IsTrue(!(result.Model as ICollectionContentViewModel<IModel>).Results.Any(), "Results are not '0' when no items are returned.");
            Assert.AreEqual((result.Model as ISearchContentViewModel<IModel>).TotalResults, (result.Model as ISearchContentViewModel<IModel>).Results.Count());
            Assert.IsTrue((result.Model as ISearchContentViewModel<IModel>).Query == "lorem", "Returned Query is not equal to given search query");

        }

        [TestMethod]
        public void Search__EmptyQuery__Returns_NOT_As_Query()
        {
            //1. Arrange
            
            Repository.Setup(s => s.Find(It.IsAny<string>()))
                .Returns(new List<IModel>());
            

            var mController = new Mock<SearchBaseController>(Repository.Object) { CallBase = true }; //abstract class callBase
            var renderModel = new RenderModel(Arrange.Content().Object, CultureInfo.InvariantCulture);

            //2.Act
            var result = mController.Object.Search(renderModel,
                p: 1,
                q: "") as ViewResult;

            //3. Assert.
            Assert.IsTrue((result.Model as ISearchContentViewModel<IModel>).Query == "<NOT>", "The <NOT> query syntax is not used for an empty search query");
        }

        [TestMethod]
        public void Search_Pagesize3_ItemCount5_DisplayPage2__Page2_Returns_2_Items()
        {
            //1. Arrange
            
            Repository.Setup(s => 
                s.Find(It.IsAny<string>())) 
                .Returns(Ret(Repository.Object)); //mocked object
            

            var mController = new Mock<SearchBaseController>(Repository.Object) { CallBase = true }; //abstract class callBase
            var renderModel = new RenderModel(Arrange.Content().Object, CultureInfo.InvariantCulture);

            //2.Act
            var result = mController.Object.Search(renderModel,
                p: 2,
                s: 3,
                q: "search query") as ViewResult;

            //3. Assert.
            Assert.IsTrue((result.Model as ISearchContentViewModel<IModel>).TotalResults == 5, "Total results does not contain 5 items");
            Assert.IsTrue((result.Model as ISearchContentViewModel<IModel>).Results.Count() == 2, "Resultset does not contain the correct amount of items");
        }

        public static IEnumerable<IModel> Ret(ISiteRepository rep)
        {
            return Arrange.BasicPages(rep).Take(5);
        }
    }
}
