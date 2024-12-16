using EscherAssessment.Enums;
using EscherAssessment.Interfaces;
using EscherAssessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EscherAssessment.Services
{
    public class UserInfoCollector : IUserInfoCollector
    {
        private IConsoleService _consoleService;

        public UserInfoCollector(IConsoleService consoleService)
        {
            _consoleService = consoleService ?? throw new ArgumentNullException(nameof(consoleService));
        }

        public Person GetUserInformation()
        {
            var firstName = GetFirstName();
            var surname = GetSurname();
            var dateOfBirth = GetDateOfBirth();
            var maritalStatus = GetMaritalStatus();

            var personInfo = new PersonInfo(firstName, surname, dateOfBirth, maritalStatus);

            var validated = ValidateAge(personInfo);

            if (!validated)
            {
                return null;
            }
            
            if (maritalStatus == MaritalStatus.Married)
            {
                var spouseInfo = GetSpouseInfo();
                validated = ValidateAge(spouseInfo);

                if (!validated)
                {
                    return null;
                }

                return new Person(personInfo, spouseInfo);
            }

            return new Person(personInfo);
        }

        private PersonInfo GetSpouseInfo()
        {
            _consoleService.WriteLine("\nSince you have a spouse, please provide their information too.");
            var firstName = GetFirstName();
            var surname = GetSurname();
            var dateOfBirth = GetDateOfBirth();
            
            return new PersonInfo(firstName, surname, dateOfBirth, MaritalStatus.Married);
        }

        private string GetFirstName()
        {
            return GetName("Enter first name: ");   
        }

        private string GetSurname()
        {
            return GetName("Enter surname: ");
        }

        private string GetName(string message)
        {
            string name;

            while (true)
            {
                _consoleService.Write(message);
                name = _consoleService.ReadLine();

                if (string.IsNullOrEmpty(name))
                {
                    _consoleService.WriteLine("Name must contain at least one character. Please try again.");
                    continue;
                }

                if (Regex.IsMatch(name, @"[^a-zA-Z\s]"))
                {
                    _consoleService.WriteLine("Invalid name. Please try again.");
                    continue;
                }

                break;
            }

            return name;
        }

        private DateTime GetDateOfBirth()
        {
            DateTime dateOfBirth;

            while (true)
            {
                _consoleService.Write("Enter date of birth (MM/dd/yyyy): ");
                var date = _consoleService.ReadLine();
                bool success = DateTime.TryParseExact(date, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateOfBirth);

                if (DateTime.Now < dateOfBirth)
                {
                    _consoleService.WriteLine("Please enter a valid birthday.");
                    continue;
                }

                if (success)
                {
                    break;
                }
                else
                {
                    _consoleService.WriteLine("Please enter a valid date.");
                }
            }

            return dateOfBirth;
        }

        private MaritalStatus GetMaritalStatus()
        {
            while (true)
            {
                _consoleService.Write("Are you married? (y/n): ");
                var userInput = _consoleService.ReadLine();

                if (userInput.ToLower() == "y")
                {
                    return MaritalStatus.Married;
                }
                else if (userInput.ToLower() == "n")
                {
                    return MaritalStatus.Single;
                }
                else
                {
                    _consoleService.WriteLine("Please enter valid input.");
                    continue;
                }
            }
        }

        private bool ValidateAge(PersonInfo personInfo)
        {
            var age = DateTime.Now.Year - personInfo.DateOfBirth.Year;

            // Handles the case where the person has not had their birthday yet this year
            if (DateTime.Now.AddYears(-age) < personInfo.DateOfBirth.Date)
            {
                age--;
            }

            if (age < 16)
            {
                _consoleService.WriteLine("\nYou are too young to register. You must be 18 years old.");
                return false;
            }

            if (age < 18)
            {
                _consoleService.WriteLine("\nSince you are under the age of 18, we require parental consent.");
                _consoleService.Write("My parents allow registration (y/n): ");
                var consentGiven = _consoleService.ReadLine().Trim().ToLower() == "y";

                if (consentGiven)
                {
                    personInfo.ParentalConsent = true;
                }

                return consentGiven;
            }

            return true;
        }
    }
}
