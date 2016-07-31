using System.Reflection;

namespace Machine.Specifications.Runner.DotNet.Helpers
{
    public static class AssemblyHelper
    {
        public static Assembly LoadTestAssemblyOrDependency(string assemblyPath)
        {
#if NETCORE
            // Appears that the dotnet cli does some magic, so that
            // when ran all paths and magic things are set as such that we can load by AssemblyName
            return Assembly.Load(new AssemblyName(System.IO.Path.GetFileNameWithoutExtension(assemblyPath)));
#else

            return Assembly.LoadFrom(assemblyPath);
#endif
        }
    }
}