using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Machine.Specifications.Core.Runner.DotNet.Configuration;
using Machine.Specifications.Core.Runner.DotNet.Helpers;
using Machine.Specifications.Explorers;
using Machine.Specifications.Model;
using Machine.Specifications.Runner;
using Machine.Specifications.Runner.Impl;
using Microsoft.Extensions.Testing.Abstractions;

namespace Machine.Specifications.Core.Runner.DotNet.Execution
{
    public class AssemblySpecificationRunner : ISpecificationExecutor
    {
        private readonly ISpecificationRunListener _runListener;

        public AssemblySpecificationRunner(ISpecificationRunListener runListener)
        {
            if (runListener == null)
                throw new ArgumentNullException(nameof(runListener));

            _runListener = runListener;
        }

        public void Run(string assemblyPath)
        {
            assemblyPath = Path.GetFullPath(assemblyPath);

            DefaultRunner mspecRunner = new DefaultRunner(_runListener, RunOptions.Default);

            Assembly assembly = AssemblyHelper.Load(assemblyPath);

            mspecRunner.RunAssembly(assembly);
        }

        // Run specific Its with support for behaviors. Note that this is not something MSpec currently supports
        // out of the box, so for now we have build our own login around this.
        //
        public void Run(string assemblyPath, IEnumerable<DotNetTestIdentifier> specifications)
        {
            assemblyPath = Path.GetFullPath(assemblyPath);

            Assembly assemblyToRun = AssemblyHelper.Load(assemblyPath);
            DefaultRunner mspecRunner = new DefaultRunner(_runListener, RunOptions.Default);

            IEnumerable<Context> specificationContextsInAssembly = new AssemblyExplorer().FindContextsIn(assemblyToRun) ?? Enumerable.Empty<Context>();

            Dictionary<string, Context> contextMap = specificationContextsInAssembly.ToDictionary(c => c.Type.FullName, StringComparer.Ordinal);

            try {
                // We use explicit assembly start and end to wrap the RunMember loop
                mspecRunner.StartRun(assemblyToRun);

                foreach (DotNetTestIdentifier test in specifications) {
                    Context context = contextMap[test.ContainerTypeFullName];
                    if (context == null)
                        continue;

                    Specification specification = context.Specifications.SingleOrDefault(spec => spec.FieldInfo.Name.Equals(test.FieldName, StringComparison.Ordinal));

                    if (specification is BehaviorSpecification) {
                        // MSpec doesn't expose any way to run an an "It" coming from a "[Behavior]", so we have to do some trickery
                        DotNetTestIdentifier listenFor = specification.ToDotNetTestIdentifier(context);
                        DefaultRunner behaviorRunner = new DefaultRunner(new SingleBehaviorTestRunListenerWrapper(_runListener, listenFor), RunOptions.Default);
                        behaviorRunner.RunMember(assemblyToRun, context.Type.GetTypeInfo());
                    } else {
                        mspecRunner.RunMember(assemblyToRun, specification.FieldInfo);
                    }
                }
            } finally {
                mspecRunner.EndRun(assemblyToRun);
            }
        }
    }
}