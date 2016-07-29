namespace Machine.Specifications.Runner.DotNet.Execution.Console
{
    public class DefaultConsole : IConsole
    {
        public void Write(string line)
        {
            System.Console.Write(line);
        }

        public void Write(string line, params object[] parameters)
        {
            System.Console.Write(line, parameters);
        }

        public void WriteLine(string line)
        {
            System.Console.WriteLine(line);
        }

        public void WriteLine(string line, params object[] parameters)
        {
            System.Console.WriteLine(line, parameters);
        }
    }
}
