using System;
using System.IO;
using System.Reflection;
using Machine.Specifications.Runner.DotNet.Execution.Console;
using Machine.Specifications.Runner.DotNet.Helpers;
using Machine.Specifications.Runner.DotNet.Controller;
using Machine.Specifications.Runner.DotNet.Execution;

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

            string assemblyPath = commandLine.AssemblyFile;
            string mspecPath = Path.Combine(Path.GetDirectoryName(assemblyPath),
                                            "Machine.Specifications.dll");

            Assembly mspecAssembly = AssemblyHelper.Load(mspecPath);
            Assembly testAssembly = AssemblyHelper.Load(assemblyPath);

            ConsoleOutputRunListener runListener = new ConsoleOutputRunListener();
            ISpecificationRunListener allListeneres = new AggregateRunListener(new ISpecificationRunListener[] {
                runListener,
                new AssemblyLocationAwareRunListener(new[] {testAssembly})
            });

            TestController testController = new TestController(mspecAssembly, allListeneres);

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