using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Machine.Specifications.Runner.DotNet.Controller;
using Machine.Specifications.Runner.DotNet.Controller.Model;

namespace Machine.Specifications.Runner.DotNet.Execution
{
    /// <summary>
    /// Listens for new assembly runs and changes the current working directory
    /// before the tests start executing.
    /// </summary>
    public class AssemblyLocationAwareRunListener : ISpecificationRunListener
    {
        private readonly IEnumerable<Assembly> _assemblies;

        public AssemblyLocationAwareRunListener(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));

            _assemblies = assemblies;
        }

        public void OnAssemblyStart(AssemblyInfo assembly)
        {
            Assembly loadedAssebmly = _assemblies.FirstOrDefault(a => a.GetName().Name.Equals(assembly.Name, StringComparison.OrdinalIgnoreCase));

            Directory.SetCurrentDirectory(Path.GetDirectoryName(loadedAssebmly.Location));
        }

        public void OnAssemblyEnd(AssemblyInfo assembly)
        {
        }

        public void OnRunStart()
        {
        }

        public void OnRunEnd()
        {
        }

        public void OnContextStart(ContextInfo context)
        {
        }

        public void OnContextEnd(ContextInfo context)
        {
        }

        public void OnSpecificationStart(SpecificationInfo specification)
        {
        }

        public void OnSpecificationEnd(SpecificationInfo specification, Result result)
        {
        }

        public void OnFatalError(ExceptionResult exception)
        {
        }
    }
}