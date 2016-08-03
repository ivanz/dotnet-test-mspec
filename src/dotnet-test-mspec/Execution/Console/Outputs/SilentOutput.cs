using Machine.Specifications.Runner.DotNet.Controller.Model;

namespace Machine.Specifications.Runner.DotNet.Execution.Console.Outputs
{
    class SilentOutput : IOutput
    {
        public void RunStart()
        {
        }

        public void RunEnd()
        {
        }

        public void AssemblyStart(AssemblyInfo assembly)
        {
        }

        public void AssemblyEnd(AssemblyInfo assembly)
        {
        }

        public void ContextStart(ContextInfo context)
        {
        }

        public void ContextEnd(ContextInfo context)
        {
        }

        public void SpecificationStart(SpecificationInfo specification)
        {
        }

        public void Passing(SpecificationInfo specification)
        {
        }

        public void NotImplemented(SpecificationInfo specification)
        {
        }

        public void Ignored(SpecificationInfo specification)
        {
        }

        public void Failed(SpecificationInfo specification, Result result)
        {
        }
    }
}