using EscherAssessment.Enums;
using EscherAssessment.Interfaces;
using EscherAssessment.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscherAssessment.Services
{
    public class LocalUserRepository : IUserRepository
    {
        private IConsoleService _consoleService;

        private readonly string _mainFileName = "People";
        private string _mainFilePath = "";
        private string _spouseDir = "";
        private readonly string _workingDir;

        public LocalUserRepository(IConsoleService consoleService, string workingDir)
        {
            _consoleService = consoleService ?? throw new ArgumentNullException(nameof(consoleService));
            _workingDir = workingDir ?? throw new ArgumentNullException(nameof(workingDir));

            Initialize();
        }

        public void Save(Person person)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_mainFilePath, true))
                {
                    string consent;

                    if (person.Info.ParentalConsent == null)
                    {
                        consent = "null";
                    }
                    else
                    {
                        consent = person.Info.ParentalConsent.ToString();
                    }

                    var content = $"{person.Info.FirstName}|{person.Info.Surname}|{person.Info.DateOfBirth:dd-MM-yyyy}|{person.Info.MaritalStatus}|{consent}";

                    if (person.Info.MaritalStatus == MaritalStatus.Married)
                    {
                        var spouseFilePath = CreateSpouseFile(person.SpouseInfo);
                        content += $"|{spouseFilePath}";
                    }

                    writer.WriteLine(content);
                }
            }
            catch (Exception ex)
            {
                _consoleService.WriteLine($"Exception thrown: {ex}");
                throw;
            }
        }

        private void Initialize()
        {
            try
            {
                var peopleDir = Path.Combine(_workingDir, "People");

                _spouseDir = Path.Combine(peopleDir, "Spouses");
                _mainFilePath = Path.Combine(peopleDir, $"{_mainFileName}.txt");

                if (File.Exists(_mainFilePath))
                {
                    return;
                }

                Directory.CreateDirectory(_workingDir);
                Directory.CreateDirectory(peopleDir);
                Directory.CreateDirectory(_spouseDir);

                File.Create(_mainFilePath).Dispose();
            }
            catch (Exception ex)
            {
                _consoleService.WriteLine($"File initialization failed. Reason: {ex.Message}");
                _consoleService.WriteLine("Exiting application...");

                // Give the user time to read the exception message
                Task.Delay(3000);
                throw;
            }
        }

        private string CreateSpouseFile(PersonInfo spouseInfo)
        {
            var filePath = Path.Combine(_spouseDir, $"{spouseInfo.FirstName} {spouseInfo.Surname} [ {Guid.NewGuid()} ].txt");

            var content = $"{spouseInfo.FirstName}|{spouseInfo.Surname}|{spouseInfo.DateOfBirth:dd-MM-yyyy}|{spouseInfo.MaritalStatus}";

            File.WriteAllText(filePath, content);

            return filePath;
        }
    }
}
