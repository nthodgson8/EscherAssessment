using EscherAssessment.Enums;
using EscherAssessment.Interfaces;
using EscherAssessment.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscherAssessment.Tests
{
    [TestFixture]
    public class UserInfoCollectorTests
    {
        private Mock<IConsoleService> _mockConsoleService = new Mock<IConsoleService>();

        [SetUp]
        public void Setup()
        {
            _mockConsoleService = new Mock<IConsoleService>();
        }

        [TearDown]
        public void TearDown()
        {
            _mockConsoleService.Reset();
        }

        [Test]
        public void GetUserInformation_Single()
        {
            _mockConsoleService.SetupSequence(s => s.ReadLine())
                .Returns("Joe")
                .Returns("Schmoe")
                .Returns("01/01/1990")
                .Returns("n");

            var userInfoCollector = new UserInfoCollector(_mockConsoleService.Object);
            var person = userInfoCollector.GetUserInformation();

            Assert.AreEqual("Joe", person.Info.FirstName);
            Assert.AreEqual("Schmoe", person.Info.Surname);
            Assert.AreEqual(new DateTime(1990, 1, 1), person.Info.DateOfBirth);
            Assert.AreEqual(MaritalStatus.Single, person.Info.MaritalStatus);
            Assert.IsNull(person.SpouseInfo);
        }

        [Test]
        public void GetUserInformation_Married()
        {
            _mockConsoleService.SetupSequence(s => s.ReadLine())
                .Returns("Joe")
                .Returns("Schmoe")
                .Returns("01/01/1990")
                .Returns("y")
                .Returns("Tiffany")
                .Returns("Schmoe")
                .Returns("02/02/1992");

            var userInfoCollector = new UserInfoCollector(_mockConsoleService.Object);
            var person = userInfoCollector.GetUserInformation();

            Assert.AreEqual("Joe", person.Info.FirstName);
            Assert.AreEqual("Schmoe", person.Info.Surname);
            Assert.AreEqual(new DateTime(1990, 1, 1), person.Info.DateOfBirth);
            Assert.AreEqual(MaritalStatus.Married, person.Info.MaritalStatus);

            Assert.IsNotNull(person.SpouseInfo);
            Assert.AreEqual("Tiffany", person.SpouseInfo.FirstName);
            Assert.AreEqual("Schmoe", person.SpouseInfo.Surname);
            Assert.AreEqual(new DateTime(1992, 2, 2), person.SpouseInfo.DateOfBirth);
        }

        [Test]
        public void GetDateOfBirth_InvalidDateEntered()
        {
            _mockConsoleService.SetupSequence(s => s.ReadLine())
                .Returns("Joe")
                .Returns("Schmoe")
                .Returns("invalid")
                .Returns("01/01/1990")
                .Returns("n");

            var userInfoCollector = new UserInfoCollector(_mockConsoleService.Object);
            var person = userInfoCollector.GetUserInformation();

            _mockConsoleService.Verify(s => s.WriteLine("Please enter a valid date."), Times.Once);
            Assert.AreEqual(new DateTime(1990, 1, 1), person.Info.DateOfBirth);
        }

        [Test]
        public void GetMaritalStatus_InvalidInput_Retry()
        {
            _mockConsoleService.SetupSequence(s => s.ReadLine())
                .Returns("Joe")
                .Returns("Schmoe")
                .Returns("01/01/1990")
                .Returns("x")
                .Returns("n");

            var userInfoCollector = new UserInfoCollector(_mockConsoleService.Object);
            var person = userInfoCollector.GetUserInformation();

            _mockConsoleService.Verify(s => s.WriteLine("Please enter valid input."), Times.Once);
            Assert.AreEqual(MaritalStatus.Single, person.Info.MaritalStatus);
        }
    }

}
