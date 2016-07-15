using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Machine.Specifications.Model;

namespace Machine.Specifications.Core.Runner.DotNet.Discovery
{
    public interface IDiscoveryVisitor
    {
        void Visit(Context context, Specification spec);
        void OnEnd();
    }
}
