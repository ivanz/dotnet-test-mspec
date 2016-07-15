using System;
using Machine.Specifications.Core.Runner.DotNet.Configuration;
using Machine.Specifications.Core.Runner.DotNet.Discovery;
using Machine.Specifications.Core.Runner.DotNet.Discovery.Console;
using Machine.Specifications.Core.Runner.DotNet.Execution;
using Machine.Specifications.Core.Runner.DotNet.Execution.Console;
using Machine.Specifications.Core.Runner.DotNet.Execution.DesignTime;
using Microsoft.Extensions.Testing.Abstractions;

namespace Machine.Specifications.Runner.DotNet
{
    public class Program
    {
        private static ITestDiscoverySink _testDiscoverySink;
        private static ITestExecutionSink _testExecutionSink;

        public static void Main(string[] args)
        {
            CommandLine commandLine = CommandLine.Parse(args);

            if (commandLine.DesignTime) {
                throw new NotSupportedException("DesignTime mode is not supported yet.");
            } else {
                _testDiscoverySink = new StreamingTestDiscoverySink(Console.OpenStandardOutput());
                _testExecutionSink = new StreamingTestExecutionSink(Console.OpenStandardOutput());
            }


            Settings settings = new Settings();

            if (commandLine.List) {

                IDiscoveryVisitor discoveryVisitor;

                if (commandLine.DesignTime) {
                    //discoveryVisitor = new DesignTimeDiscoveryVisitor(...sink...);
                    throw new NotSupportedException("DesignTime mode is not supported yet.");
                } else {
                    discoveryVisitor = new ConsoleDiscoveryVisitor();
                }

                AssemblySpecificationDiscoverer discoverer = new AssemblySpecificationDiscoverer(discoveryVisitor);
                discoverer.Discover(commandLine.AssemblyFile);
            } else {

                ISpecificationRunListener runListener;

                if (commandLine.DesignTime) {
                    throw new NotSupportedException("DesignTime mode is not supported yet.");
                    // runListener = new DesignTimeSpecificationRunListener(_estExecutionSink, settings);
                } else {
                    runListener = new ConsoleOutputRunListener();
                }

                AssemblySpecificationRunner runner = new AssemblySpecificationRunner(runListener);
                runner.Run(commandLine.AssemblyFile);
            }
        }
    }
}