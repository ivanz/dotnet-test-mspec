using System;

namespace Machine.Specifications.Core.Runner.DotNet.Execution.Console
{
    public class ExceptionReporter
    {
        readonly IConsole _console;

        public ExceptionReporter(IConsole console)
        {
            _console = console;
        }

        public void ReportException(Exception ex)
        {
            _console.WriteLine(ex.Message);
        }
    }
}
