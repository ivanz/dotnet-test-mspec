using System.Reflection;
using System.Runtime.Loader;

namespace Machine.Specifications.Core.Runner.DotNet.Helpers
{
    public static class AssemblyHelper
    {
        public static Assembly Load(string assemblyPath)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
        }
    }
}