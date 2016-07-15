using Machine.Specifications.Model;

namespace Machine.Specifications.Core.Runner.DotNet.Discovery.Console
{
    public class ConsoleDiscoveryVisitor : IDiscoveryVisitor
    {
        public void Visit(Context context, Specification spec)
        {
            System.Console.WriteLine($"{context.Type.Name.Replace("_", " ")} it {spec.Name}");
        }

        public void OnEnd()
        {
        }
    }
}
