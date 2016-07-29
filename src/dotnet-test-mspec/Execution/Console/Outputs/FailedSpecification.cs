using Machine.Specifications.Runner.DotNet.Controller.Model;

namespace Machine.Specifications.Runner.DotNet.Execution.Console.Outputs
{
    struct FailedSpecification
    {
        public Result Result;
        public SpecificationInfo Specification;
    }
}