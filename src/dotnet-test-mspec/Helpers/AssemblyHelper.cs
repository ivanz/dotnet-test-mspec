using System.Reflection;

namespace Machine.Specifications.Runner.DotNet.Helpers
{
    public static class AssemblyHelper
    {
        public static Assembly Load(string assemblyPath)
        {
#if NETCORE
            return System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
#else
            return Assembly.LoadFrom(assemblyPath);
#endif
        }
    }
}