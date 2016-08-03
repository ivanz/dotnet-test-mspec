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
            PrintVersionInfo(typeof(Program).GetTypeInfo().Assembly);

            CommandLine commandLine = CommandLine.Parse(args);

            if (commandLine.DesignTime) {
                throw new NotSupportedException("DesignTime mode is not supported yet.");
            }

            string assemblyPath = commandLine.AssemblyFile;

            Assembly testAssembly = AssemblyHelper.LoadTestAssemblyOrDependency(assemblyPath);
            Assembly mspecAssembly = AssemblyHelper.LoadTestAssemblyOrDependency(
                Path.Combine(Path.GetDirectoryName(assemblyPath), "Machine.Specifications.dll")
            );

            PrintVersionInfo(mspecAssembly);


            ConsoleOutputRunListener runListener = new ConsoleOutputRunListener();
            ISpecificationRunListener allListeneres = new AggregateRunListener(new ISpecificationRunListener[] {
                runListener,
#if !UWP
                new AssemblyLocationAwareRunListener(new[] {testAssembly})
#endif
            });

            TestController testController = new TestController(mspecAssembly, allListeneres);

            if (commandLine.List) {
                Console.WriteLine(testController.DiscoverTestsRaw(testAssembly));
            } else {
                testController.RunAssemblies(new[] { testAssembly });

                if (runListener.FailureOccurred) {
#if UWP
                    throw new Exception("Tests failed"); // TODO
#else
                    Environment.Exit(-1);
#endif
                }
            }
        }

        private static void PrintVersionInfo(Assembly assembly)
        {
            Console.WriteLine(assembly.GetName().Name + ": " + assembly.GetVersion());
        }
    }
}