using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;

using Machine.Specifications.Runner.DotNet.Controller.Model;


namespace Machine.Specifications.Runner.DotNet.Controller
{
    // Used to implement version agnostic test runners
    public class TestController
    {
        private const string CONTROLLER_TYPE = "Machine.Specifications.Controller.Controller";
        private const string START_RUN_METHOD = "StartRun";
        private const string END_RUN_METHOD = "EndRun";

        private readonly ISpecificationRunListener _runListener;
        private readonly Assembly _frameworkAssembly;
        private readonly object _controller;

        public TestController(Assembly frameworkAssembly, ISpecificationRunListener runListener)
        {
            _runListener = runListener;
            _frameworkAssembly = frameworkAssembly;
            _controller = CreateController(frameworkAssembly);
        }

        private object CreateController(Assembly frameworkAssembly)
        {
            return Activator.CreateInstance(_frameworkAssembly.GetType(CONTROLLER_TYPE), (Action<string>)OnListenEvent);
        }

        public void RunAssemblies(IEnumerable<Assembly> assemblies)
        {
            _controller
                .GetType()
                .GetMethod(START_RUN_METHOD)
                .Invoke(_controller, null);

            _controller
                .GetType()
                .GetMethod("RunAssemblies")
                .Invoke(_controller, new object[] { assemblies });

            _controller
                .GetType()
                .GetMethod(END_RUN_METHOD)
                .Invoke(_controller, null);
        }

        // Temporary - just to show how to get the xml out
        public string DiscoverTestsRaw(Assembly assembly)
        {
            return (string)_controller
                                .GetType()
                                .GetMethod("DiscoverTests")
                                .Invoke(_controller, new object[] { assembly });
        }

        private void OnListenEvent(string value)
        {
            using (var stringReader = new StringReader(value)) {
                XDocument doc = XDocument.Load(stringReader);
                XElement element = doc.XPathSelectElement("/listener/*");

                switch (element.Name.ToString())
                {
                    case "onassemblystart":
                        _runListener.OnAssemblyStart(AssemblyInfo.Parse(element.XPathSelectElement("//onassemblystart/*").ToString()));
                        break;
                    case "onassemblyend":
                        _runListener.OnAssemblyEnd(AssemblyInfo.Parse(element.XPathSelectElement("//onassemblyend/*").ToString()));
                        break;
                    case "onrunstart":
                        _runListener.OnRunStart();
                        break;
                    case "onrunend":
                        _runListener.OnRunEnd();
                        break;
                    case "oncontextstart":
                        _runListener.OnContextStart(ContextInfo.Parse(element.XPathSelectElement("//oncontextstart/*").ToString()));
                        break;
                    case "oncontextend":
                        _runListener.OnContextEnd(ContextInfo.Parse(element.XPathSelectElement("//oncontextend/*").ToString()));
                        break;
                    case "onspecificationstart":
                        _runListener.OnSpecificationStart(SpecificationInfo.Parse(element.XPathSelectElement("//onspecificationstart/*").ToString()));
                        break;
                    case "onspecificationend":
                        _runListener.OnSpecificationEnd(
                            SpecificationInfo.Parse(element.XPathSelectElement("//onspecificationend/specificationinfo").ToString()),
                            Result.Parse(element.XPathSelectElement("//onspecificationend/result").ToString()));
                        break;
                    case "onfatalerror":
                        _runListener.OnFatalError(ExceptionResult.Parse(element.XPathSelectElement("//onfatalerror/*").ToString()));
                        break;
                }
            }

        }

    }
}