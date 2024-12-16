using EscherAssessment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscherAssessment.Services
{
    public class ConsoleService : IConsoleService
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void Write(string message)
        {
            Console.Write(message);
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public void GreetUser()
        {
            Console.WriteLine(" #################### Person Registration Application #################### \n");
            Console.WriteLine(" Welcome!");
            Console.WriteLine(" Please enter your information to be registered. If you have a spouse,\n you will be required to register them too.");
            Console.WriteLine("\n #########################################################################\n\n");
        }

        public bool PromptRestart()
        {
            Console.Write("\nWould you like to enter information for another person? (y/n): ");
            var userInput = Console.ReadLine();
            return userInput.ToLower() == "y";
        }
    }
}
