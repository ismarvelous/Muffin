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
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Muffin.Core.ViewModels;

namespace Muffin.Test
{
    [TestClass]
    public class ContainerBaseControllerTest : BaseTestClass // Naming convention: Method_to_test__State_under_test__Expected_behavior
    {
        [TestMethod]
        public void Container__NoChilds__Total_results_equals_ResultsCount()
        {
            //1. Arrange
            var mController = new Mock<ContainerBaseController>(Repository.Object) { CallBase = true }; //abstract class callBase
            var renderModel = new RenderModel(Arrange.Content().Object, CultureInfo.InvariantCulture);

            //2.Act
            var result = mController.Object.Container(renderModel,
                p: 1) as ViewResult;

            //3. Assert.
            Assert.IsTrue(!(result.Model as CollectionContentViewModel<IModel>).Results.Any(), "Results are not '0' when no items are returned.");
            Assert.AreEqual((result.Model as CollectionContentViewModel<IModel>).TotalResults, (result.Model as CollectionContentViewModel<IModel>).Results.Count());
        }

        [TestMethod]
        public void Container__5Childs_PageSize4__Page2_Returns_1_Item()
        {
            //1. Arrange
            var mController = new Mock<ContainerBaseController>(Repository.Object) { CallBase = true }; //abstract class callBase
            var mContent = Arrange.Content("lorem parent page",
            new List<IModel>()
                {
                    { Arrange.Content("Lorem child page 1").Object },
                    { Arrange.Content("Ipsum child page 2").Object },
                    { Arrange.Content("Dolor child page 3").Object },
                    { Arrange.Content("Sit child page 4").Object },
                    { Arrange.Content("Consectetur child page 5").Object }
                });

            var renderModel = new RenderModel(mContent.Object, CultureInfo.InvariantCulture);

            //2.Act
            var result = mController.Object.Container(renderModel,
                p: 2,
                s: 4) as ViewResult;

            //3. Assert.
            Assert.IsTrue((result.Model as CollectionContentViewModel<IModel>).TotalResults == 5, "Total results does not contain 5 items");
            Assert.IsTrue((result.Model as CollectionContentViewModel<IModel>).Results.Count() == 1, "Resultset for page 2 does not contain the correct amount of items");
        }
    }
}
