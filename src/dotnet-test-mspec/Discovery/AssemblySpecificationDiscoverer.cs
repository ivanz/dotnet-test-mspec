using System;
using System.Reflection;
using Machine.Specifications.Explorers;
using System.Collections.Generic;
using Machine.Specifications.Model;
using Machine.Specifications.Core.Runner.DotNet.Helpers;

namespace Machine.Specifications.Core.Runner.DotNet.Discovery
{
    public class AssemblySpecificationDiscoverer
    {
        private readonly IDiscoveryVisitor _visitor;
        private readonly AssemblyExplorer _assemblyExplorer;

        public AssemblySpecificationDiscoverer(IDiscoveryVisitor visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            _visitor = visitor;
            _assemblyExplorer = new AssemblyExplorer();
        }

        public void Discover(string assemblyPath)
        {
            Assembly assembly = AssemblyHelper.Load(assemblyPath);

            IEnumerable<Context> contexts = _assemblyExplorer.FindContextsIn(assembly);

            foreach (Context context in contexts) {
                foreach (Specification spec in context.Specifications) {
                    _visitor.Visit(context, spec);                    
                }
            }

            _visitor.OnEnd();
        }
    }
}