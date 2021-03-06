﻿using Kifreak.KiImageOrganizer.Console.Actions;
using Kifreak.KiImageOrganizer.Console.Configuration;
using Kifreak.KiImageOrganizer.Console.Formatters;
using Kifreak.KiImageOrganizer.Console.Models;
using Kifreak.KiImageOrganizer.Console.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Threading.Tasks;

namespace Kifreak.KiImageOrganizer.Tests.Services
{
    [TestClass]
    public class ActionServiceUnitTest
    {
        [ClassInitialize]
        public static void Init(TestContext context)
        {
            Config.Startup();
        }

        [TestMethod]
        public void HasAllActionOk()
        {
            string[] actions = new[] { "City", "Road", "YearMonth" };
            IActionService service = GetActionService();
            bool hasAllAction = service.HasNoExistAction(actions);
            Assert.IsFalse(hasAllAction);
        }

        [TestMethod]
        public void HasAllActionWithVerticalBar()
        {
            string[] actions = new[] { $"Country{Config.AlternativeCharacter}City", "Road", "YearMonth" };
            IActionService service = GetActionService();
            bool hasAllAction = service.HasNoExistAction(actions);
            Assert.IsFalse(hasAllAction);
        }

        [TestMethod]
        public void HasAllActionKo()
        {
            string[] actions = new[] { "City", "Invented", "Road", "YearMonth" };
            IActionService service = GetActionService();
            bool hasAllAction = service.HasNoExistAction(actions);
            Assert.IsTrue(hasAllAction);
        }

        [TestMethod]
        public void HasAllActionWithEmptyActions()
        {
            string[] actions = new string[0];
            IActionService service = GetActionService();
            bool hasAllAction = service.HasNoExistAction(actions);
            Assert.IsFalse(hasAllAction);
        }

        [TestMethod]
        public void GetAllActionOk()
        {
            IActionService service = GetActionService();
            string allActions = service.ActionsToString();
            Assert.AreEqual("City,Road,Village,Country,County,AmenityType,AmenityName,Date,DateTime,Time,YearMonth,Noop", allActions);
        }

        [TestMethod]
        public async Task GetSubFoldersOk()
        {
            var actionParser = new Mock<IActionParser>();
            actionParser.Setup(t =>
                    t.InvokeWithAlternative(It.IsAny<KeysAlternatives>(), It.IsAny<SubFolders>(), It.IsAny<string>()))
                .Returns(() => new MainFolder(Directory.GetCurrentDirectory()));
            IActionService service = new ActionService(actionParser.Object);
            var subFolder = await service.GetSubFolder(Directory.GetCurrentDirectory(), new[] { "City", "Day" },
                new MainFolder(Directory.GetCurrentDirectory()), new FileFormatters());
            actionParser.Verify(t => t.InvokeWithAlternative(It.IsAny<KeysAlternatives>(), It.IsAny<SubFolders>(), It.IsAny<string>()), Times.Exactly(2));
            Assert.AreEqual(Directory.GetCurrentDirectory(), subFolder);
        }

        private IActionService GetActionService()
        {
            return Config.Get<IActionService>();
        }
    }
}