using System;
using Machine.Specifications.Runner.DotNet.Controller.Model;

namespace Machine.Specifications.Runner.DotNet.Execution.Console.Outputs
{
    class VerboseOutput : IOutput
    {
        readonly IConsole _console;

        public VerboseOutput(IConsole console)
        {
            _console = console;
        }

        public void RunStart()
        {
        }

        public void RunEnd()
        {
            EmptyLine();
        }

        public void AssemblyStart(AssemblyInfo assembly)
        {
            EmptyLine();
            _console.WriteLine("Specs in " + assembly.Name + ":");
        }

        public void AssemblyEnd(AssemblyInfo assembly)
        {
        }

        public void ContextStart(ContextInfo context)
        {
            EmptyLine();
            _console.WriteLine(context.FullName);
        }

        public void ContextEnd(ContextInfo context)
        {
        }

        public void SpecificationStart(SpecificationInfo specification)
        {
            _console.Write(">> " + specification.Name);
        }

        public void Passing(SpecificationInfo specification)
        {
            EmptyLine();
        }

        public void NotImplemented(SpecificationInfo specification)
        {
            _console.WriteLine(" (NOT IMPLEMENTED)");
        }

        public void Ignored(SpecificationInfo specification)
        {
            _console.WriteLine(" (IGNORED)");
        }

        public void Failed(SpecificationInfo specification, Result result)
        {
            _console.WriteLine(" (FAIL)");
            _console.WriteLine(result.Exception.ToString());
        }

        public void FatalError(ExceptionResult exception)
        {
            _console.WriteLine(exception.ToString());
            EmptyLine();
        }

        void EmptyLine()
        {
            _console.WriteLine(String.Empty);
            _console.WriteLine(String.Empty);
        }
    }
}