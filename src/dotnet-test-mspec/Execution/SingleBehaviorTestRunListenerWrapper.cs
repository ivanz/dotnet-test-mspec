using System;
using Machine.Specifications.Core.Runner.DotNet.Helpers;
using Machine.Specifications.Runner;

namespace Machine.Specifications.Core.Runner.DotNet.Execution
{
    /// <summary>
    /// The purpose of this class is to ignore everything, but a single specification's notifications.
    /// Also because [Behavior] "It" gets reported as belonging to the Behavior class rather than test class
    /// we need to map from one to the other for Visual Studio to capture the results.
    /// </summary>
    public class SingleBehaviorTestRunListenerWrapper : ISpecificationRunListener
    {
        private readonly ISpecificationRunListener _runListener;
        private readonly DotNetTestIdentifier _listenFor;
        private ContextInfo _currentContext;

        public SingleBehaviorTestRunListenerWrapper(ISpecificationRunListener runListener, DotNetTestIdentifier listenFor)
        {
            if (listenFor == null)
                throw new ArgumentNullException(nameof(listenFor));
            if (runListener == null)
                throw new ArgumentNullException(nameof(runListener));

            _runListener = runListener;
            _listenFor = listenFor;
        }


        public void OnContextEnd(ContextInfo context)
        {
            _currentContext = null;
            _runListener.OnContextEnd(context);
        }

        public void OnContextStart(ContextInfo context)
        {
            _currentContext = context;
            _runListener.OnContextStart(context);
        }

        public void OnSpecificationEnd(SpecificationInfo specification, Result result)
        {
            if (_listenFor != null && !_listenFor.Equals(specification.ToDotNetTestIdentifier(_currentContext)))
                return;

            _runListener.OnSpecificationEnd(specification, result);
        }

        public void OnSpecificationStart(SpecificationInfo specification)
        {
            if (_listenFor != null && !_listenFor.Equals(specification.ToDotNetTestIdentifier(_currentContext)))
                return;

            _runListener.OnSpecificationStart(specification);
        }

        public void OnAssemblyEnd(AssemblyInfo assembly)
        {
            _runListener.OnAssemblyEnd(assembly);
        }

        public void OnAssemblyStart(AssemblyInfo assembly)
        {
            _runListener.OnAssemblyStart(assembly);
        }

        public void OnFatalError(ExceptionResult exception)
        {
            _runListener.OnFatalError(exception);
        }

        public void OnRunEnd()
        {
            _runListener.OnRunEnd();
        }

        public void OnRunStart()
        {
            _runListener.OnRunStart();
        }
    }
}