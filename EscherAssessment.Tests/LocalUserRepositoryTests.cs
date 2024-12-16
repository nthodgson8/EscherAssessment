using EscherAssessment.Enums;
using EscherAssessment.Interfaces;
using EscherAssessment.Models;
using EscherAssessment.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EscherAssessment.Tests
{
    [TestFixture]
    public class LocalUserRepositoryTests
    {
        private Mock<IConsoleService> _mockConsoleService = new Mock<IConsoleService>();
        private string _tempDirectory;

        [SetUp]
        public void Setup()
        {
            _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        }

        [TearDown]
        public void Cleanup()
        {
            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, true);
            }
        }

        [Test]
        public void Initialize_CreateFiles()
        {
            Directory.CreateDirectory(_tempDirectory);

            var repository = new LocalUserRepository(_mockConsoleService.Object, _tempDirectory);

            var peopleDir = Path.Combine(_tempDirectory, "People");
            var spouseDir = Path.Combine(peopleDir, "Spouses");
            var mainFilePath = Path.Combine(peopleDir, "People.txt");

            Assert.IsTrue(Directory.Exists(peopleDir));
            Assert.IsTrue(Directory.Exists(spouseDir));
            Assert.IsTrue(File.Exists(mainFilePath));
        }
    }
}
