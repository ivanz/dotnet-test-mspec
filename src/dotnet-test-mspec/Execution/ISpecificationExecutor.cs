using System.Collections.Generic;
using Machine.Specifications.Core.Runner.DotNet.Configuration;
using Machine.Specifications.Core.Runner.DotNet.Helpers;
using Microsoft.Extensions.Testing.Abstractions;

namespace Machine.Specifications.Core.Runner.DotNet.Execution
{
    public interface ISpecificationExecutor
    {
        void Run(string assemblyPath);

        void Run(string assemblyPath, IEnumerable<DotNetTestIdentifier> specifications);
    }
}