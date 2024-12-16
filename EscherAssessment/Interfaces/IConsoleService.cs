namespace EscherAssessment.Interfaces
{
    public interface IConsoleService
    {
        void Write(string message);

        void WriteLine(string message);

        string ReadLine();

        void GreetUser();

        bool PromptRestart();
    }
}