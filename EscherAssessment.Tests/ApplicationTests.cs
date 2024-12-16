using EscherAssessment;
using EscherAssessment.Enums;
using EscherAssessment.Interfaces;
using EscherAssessment.Models;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading;

namespace ApplicationTests
{
    [TestFixture]
    public class ApplicationTests
    {
        private Mock<IUserInfoCollector> _mockUserInfoCollector = new Mock<IUserInfoCollector>();
        private Mock<IUserRepository> _mockUserRepository = new Mock<IUserRepository>();
        private Mock<IConsoleService> _mockConsoleService = new Mock<IConsoleService>();
        private StringWriter _stringWriter = new StringWriter();
        private Application _application;
        private PersonInfo _singlePersonInfo = new PersonInfo("Joe", "Schmoe", new DateTime(1990, 10, 10), MaritalStatus.Single);

        [SetUp]
        public void Setup()
        {
            Console.SetOut(_stringWriter);
            _application = new Application(_mockUserInfoCollector.Object, _mockUserRepository.Object, _mockConsoleService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _mockUserInfoCollector.Reset();
            _mockUserRepository.Reset();
            _mockConsoleService.Reset();
        }

        [Test]
        public void Run_GreetUser()
        {
            _mockConsoleService.Setup(s => s.GreetUser());

            _application.Run();

            _mockConsoleService.Verify(s => s.GreetUser(), Times.Once);
        }

        [Test]
        public void Run_NullUserInfo()
        {
            _mockConsoleService.Setup(s => s.GreetUser());
            _mockUserInfoCollector.Setup(c => c.GetUserInformation()).Returns((Person)null);
            _mockConsoleService.Setup(s => s.PromptRestart()).Returns(false);

            _application.Run();

            _mockConsoleService.Verify(s => s.WriteLine("Registration invalid."), Times.Once);
            _mockConsoleService.Verify(s => s.PromptRestart(), Times.Once);
        }

        [Test]
        public void Run_UserSave_Success()
        {
            var person = new Person(_singlePersonInfo);
            _mockConsoleService.Setup(s => s.GreetUser());
            _mockUserInfoCollector.Setup(c => c.GetUserInformation()).Returns(person);
            _mockConsoleService.Setup(s => s.PromptRestart()).Returns(false);

            _application.Run();

            _mockUserRepository.Verify(r => r.Save(It.IsAny<Person>()), Times.Once);
            _mockConsoleService.Verify(s => s.WriteLine("User information saved successfully!"), Times.Once);
        }

        [Test]
        public void Run_UserSave_Fail()
        {
            var person = new Person(_singlePersonInfo);
            _mockConsoleService.Setup(s => s.GreetUser());
            _mockUserInfoCollector.Setup(c => c.GetUserInformation()).Returns(person);
            _mockUserRepository.Setup(r => r.Save(It.IsAny<Person>())).Throws(new Exception("File save error"));
            _mockConsoleService.Setup(s => s.PromptRestart()).Returns(false);

            _application.Run();

            _mockConsoleService.Verify(s => s.WriteLine("Failed to save person information. Reason: File save error"), Times.Once);
            _mockConsoleService.Verify(s => s.PromptRestart(), Times.Once);
        }

        [Test]
        public void Run_AddAnotherUser()
        {
            var person = new Person(_singlePersonInfo);
            _mockConsoleService.Setup(s => s.GreetUser());
            _mockUserInfoCollector.Setup(c => c.GetUserInformation()).Returns(person);

            _mockConsoleService.SetupSequence(s => s.PromptRestart())
                               .Returns(true)
                               .Returns(false);

            _application.Run();

            _mockConsoleService.Verify(s => s.PromptRestart(), Times.Exactly(2));
        }
    }
}
