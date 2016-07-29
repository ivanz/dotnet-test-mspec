using System;
using System.IO;
using System.Reflection;
using Machine.Specifications.Core.Runner.DotNet.Execution.Console;
using Machine.Specifications.Core.Runner.DotNet.Helpers;
using Machine.Specifications.Runner.DotNet.Controller;

namespace Machine.Specifications.Runner.DotNet
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CommandLine commandLine = CommandLine.Parse(args);

            if (commandLine.DesignTime) {
                throw new NotSupportedException("DesignTime mode is not supported yet.");
            }


            ConsoleOutputRunListener runListener = new ConsoleOutputRunListener();

            string assemblyPath = commandLine.AssemblyFile;
            string mspecPath = Path.Combine(Path.GetDirectoryName(assemblyPath),
                                            "Machine.Specifications.dll");

            Assembly mspecAssembly = AssemblyHelper.Load(mspecPath);
            Assembly testAssembly = AssemblyHelper.Load(assemblyPath);

            TestController testController = new TestController(mspecAssembly, runListener);


            if (commandLine.List) {
                Console.WriteLine(testController.DiscoverTestsRaw(testAssembly));
            } else {
                testController.RunAssemblies(new[] { testAssembly });

                if (runListener.FailureOccurred)
                                Environment.Exit(-1);
            }
        }
    }
}