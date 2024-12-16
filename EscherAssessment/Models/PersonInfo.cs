using EscherAssessment.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscherAssessment.Models
{
    public class PersonInfo
    {
        public PersonInfo(string firstName, string surname, DateTime dateOfBirth, MaritalStatus maritalStatus, bool? parentalConsent = null)
        {
            FirstName = firstName;
            Surname = surname;
            DateOfBirth = dateOfBirth;
            MaritalStatus = maritalStatus;
            ParentalConsent = parentalConsent;
        }

        public string FirstName { get; }

        public string Surname { get; }

        public DateTime DateOfBirth { get; }

        public MaritalStatus MaritalStatus { get; }

        public bool? ParentalConsent { get; set; }
    }
}
