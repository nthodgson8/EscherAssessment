using EscherAssessment.Enums;
using EscherAssessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscherAssessment.Interfaces
{
    public interface IUserInfoCollector
    {
        Person GetUserInformation();
    }
}
