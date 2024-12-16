using EscherAssessment.Interfaces;
using EscherAssessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscherAssessment
{
    public class Application
    {
        private IUserInfoCollector _userInfoCollector;
        private IUserRepository _userRepository;
        private IConsoleService _consoleService;

        public Application(IUserInfoCollector userInfoCollector, IUserRepository userRepository, IConsoleService consoleService)
        {
            _userInfoCollector = userInfoCollector ?? throw new ArgumentNullException(nameof(userInfoCollector));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _consoleService = consoleService ?? throw new ArgumentNullException(nameof(consoleService));
        }

        public void Run()
        {
            _consoleService.GreetUser();

            while (true)
            {
                var person = _userInfoCollector.GetUserInformation();

                if (person == null)
                {
                    _consoleService.WriteLine("Registration invalid.");

                    if (_consoleService.PromptRestart())
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                try
                {
                    _userRepository.Save(person);
                }
                catch (Exception ex)
                {
                    _consoleService.WriteLine($"Failed to save person information. Reason: {ex.Message}");

                    if (_consoleService.PromptRestart())
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                _consoleService.WriteLine("User information saved successfully!");

                if (!_consoleService.PromptRestart())
                {
                    break;
                }
            }
        }
    }
}
