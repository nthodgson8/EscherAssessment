using EscherAssessment.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscherAssessment.Models
{
    public class Person
    {
        public Person(PersonInfo info)
        {
            Info = info;
        }

        public Person(PersonInfo info, PersonInfo spouseInfo)
        {
            Info = info;
            SpouseInfo = spouseInfo;
        }

        public PersonInfo Info { get; }

        public PersonInfo SpouseInfo { get; }
    }
}
