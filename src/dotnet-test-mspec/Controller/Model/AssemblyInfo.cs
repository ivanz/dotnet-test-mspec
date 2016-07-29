using System.Xml.Linq;

namespace Machine.Specifications.Runner.DotNet.Controller.Model
{
    public class AssemblyInfo
    {
        public AssemblyInfo(string name)
        {
            this.Name = name;
        }

        public AssemblyInfo()
        {
        }

        public string Name { get; private set; }
        public string CapturedOutput { get; set; }

        public static AssemblyInfo Parse(string assemblyInfoXml)
        {
            var document = XDocument.Parse(assemblyInfoXml);
            var name = document.SafeGet<string>("/assemblyinfo/name");
            var capturedoutput = document.SafeGet<string>("/assemblyinfo/capturedoutput");

            return new AssemblyInfo(name) {
                CapturedOutput = capturedoutput
            };
        }
    }
}