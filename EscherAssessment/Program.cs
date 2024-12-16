using EscherAssessment.Interfaces;
using EscherAssessment.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscherAssessment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConsoleService consoleService = new ConsoleService();
            IUserInfoCollector userInfoCollector = new UserInfoCollector(consoleService);

            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var assessmentDir = Path.Combine(documentsPath, "NathanHodgson_EscherAssessment");

            IUserRepository userRepository = new LocalUserRepository(consoleService, assessmentDir);

            var app = new Application(userInfoCollector, userRepository, consoleService);
            app.Run();
        }
    }
}
