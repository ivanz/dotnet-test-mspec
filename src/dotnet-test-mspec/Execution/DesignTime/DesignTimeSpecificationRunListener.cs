using Machine.Specifications.Runner;
using System;
using Microsoft.Extensions.Testing.Abstractions;
using Machine.Specifications.Core.Runner.DotNet.Configuration;
using Machine.Specifications.Core.Runner.DotNet.Helpers;

namespace Machine.Specifications.Core.Runner.DotNet.Execution.DesignTime
{
    public class DesignTimeSpecificationRunListener : ISpecificationRunListener
    {
        private readonly ITestExecutionSink _sink;
        private ContextInfo _currentContext;
        private RunStats _currentRunStats;
        private readonly Settings _settings;

        public DesignTimeSpecificationRunListener(ITestExecutionSink sink, Settings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            if (sink == null)
                throw new ArgumentNullException(nameof(sink));

            _sink = sink;
            _settings = settings;
        }

        public void OnFatalError(ExceptionResult exception)
        {
            if (_currentRunStats != null)
            {
                _currentRunStats.Stop();
                _currentRunStats = null;
            }
        }

        public void OnSpecificationStart(SpecificationInfo specification)
        {
            Test testCase = ConvertSpecificationToTestCase(specification, _settings);
            _sink.SendTestStarted(testCase);

            _currentRunStats = new RunStats();
        }

        public void OnSpecificationEnd(SpecificationInfo specification, Result result)
        {
            _currentRunStats?.Stop();

            Test testCase = ConvertSpecificationToTestCase(specification, _settings);
            TestResult testResult = ConverResultToTestResult(testCase, result, _currentRunStats);

            _sink.SendTestResult(testResult);

        }

        public void OnContextStart(ContextInfo context)
        {
            _currentContext = context;
        }

        public void OnContextEnd(ContextInfo context)
        {
            _currentContext = null;
        }

        public void OnRunEnd()
        {
            _sink.SendTestCompleted();
        }

        #region Mapping
        private Test ConvertSpecificationToTestCase(SpecificationInfo specification, Settings settings)
        {
            DotNetTestIdentifier vsTestId = specification.ToDotNetTestIdentifier(_currentContext);

            return new Test() {
                FullyQualifiedName = vsTestId.FullyQualifiedName,
                DisplayName = settings.DisableFullTestNameInOutput ? specification.Name : $"{_currentContext?.TypeName}.{specification.FieldName}",
            };
        }

        private static TestResult ConverResultToTestResult(Test testCase, Result result, RunStats runStats)
        {
            TestResult testResult = new TestResult(testCase) 
            {
                ComputerName = Environment.MachineName,
                Outcome = MapSpecificationResultToTestOutcome(result),
            };

            if (result.Exception != null) 
            {
                testResult.ErrorMessage = result.Exception.Message;
                testResult.ErrorStackTrace = result.Exception.ToString();
            }

            if (runStats != null)
            {
                testResult.StartTime = runStats.Start;
                testResult.EndTime = runStats.End;
                testResult.Duration = runStats.Duration;
            }

            return testResult;
        }

        private static TestOutcome MapSpecificationResultToTestOutcome(Result result)
        {
            switch (result.Status)
            {
                case Status.Failing:
                    return TestOutcome.Failed;
                case Status.Passing:
                    return TestOutcome.Passed;
                case Status.Ignored:
                    return TestOutcome.Skipped;
                case Status.NotImplemented:
                    return TestOutcome.NotFound;
                default:
                    return TestOutcome.None;
            }
        }

        #endregion


        #region Stubs
        public void OnAssemblyEnd(AssemblyInfo assembly)
        {
        }

        public void OnAssemblyStart(AssemblyInfo assembly)
        {
        }

      

        public void OnRunStart()
        {
        }
        #endregion
    }
}