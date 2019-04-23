using System;
using Kifreak.KiImageOrganizer.Console.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kifreak.KiImageOrganizer.Tests.Services
{
    [TestClass]
    public class ActionServiceUnitTest
    {

        [TestMethod]
        public void HasAllActionOk()
        {
            string[] actions = new[] {"City", "Road", "YearMonth"};
            ActionService service = new ActionService();
            bool hasAllAction = service.HasNoExistAction(actions);
            Assert.IsFalse(hasAllAction);
        }

        [TestMethod]
        public void HasAllActionWithVerticalBar()
        {
            string[] actions = new[] { "Country|City", "Road", "YearMonth" };
            ActionService service = new ActionService();
            bool hasAllAction = service.HasNoExistAction(actions);
            Assert.IsFalse(hasAllAction);
        }

        [TestMethod]
        public void HasAllActionKo()
        {
            string[] actions = new[] { "City","Invented", "Road", "YearMonth" };
            ActionService service = new ActionService();
            bool hasAllAction = service.HasNoExistAction(actions);
            Assert.IsTrue(hasAllAction);
        }

        [TestMethod]
        public void HasAllActionWithEmptyActions()
        {
            string[] actions = new string[0];
            ActionService service = new ActionService();
            bool hasAllAction = service.HasNoExistAction(actions);
            Assert.IsFalse(hasAllAction);
        }


        [TestMethod]
        public void GetAllActionOk()
        {
            ActionService service = new ActionService();
            string allActions = service.ActionsToString();
            Assert.AreEqual("City,Road,Restaurant,Village,Country,County,AmenityType,AmenityName,Date,DateTime,Time,YearMonth", allActions);
        }

        [TestMethod]
        public void GetSubFoldersOk()
        {
            throw new NotImplementedException("Have to change City/ByDate the way to instantiate");
        }
        
    }
}