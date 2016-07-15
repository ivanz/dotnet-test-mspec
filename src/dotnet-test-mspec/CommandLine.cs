using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace  Machine.Specifications.Runner.DotNet
{

    public class CommandLine
    {
        private CommandLine(string[] args)
        {
            ParseCore(args);
        }

        public bool DesignTime { get; set; }

        public bool List { get; set; }

        public int? Port { get; set; }

        public bool WaitCommand { get; set; }

        public string AssemblyFile { get; set; }

        public string ConfigFile { get; set; }


        public static CommandLine Parse(string[] args)
        {
            return new CommandLine(args);
        }

        private void ParseCore(string[] args)
        {
            Stack<string> arguments = new Stack<string>();
            for (int i = args.Length - 1; i >= 0; i--) {
                arguments.Push(args[i]);
            }

            if (arguments.Count == 0)
                throw new ArgumentException("must specify at least one assembly");

            AssemblyFile = arguments.Pop();
            if (arguments.Count > 0)
            {
                string value = arguments.Peek();
                if (!value.StartsWith("-") && value.EndsWith(".json"))
                {
                    ConfigFile = arguments.Pop();
                    if (!File.Exists(ConfigFile))
                        throw new ArgumentException(string.Format("config file not found: {0}", ConfigFile));
                }
            }

            while (arguments.Count > 0)
            {
                KeyValuePair<string, string> option = PopOption(arguments);
                string optionName = option.Key.ToLowerInvariant();

                if (!optionName.StartsWith("-"))
                    throw new ArgumentException(string.Format("unknown command line option: {0}", option.Key));
               
                // BEGIN: Special command line switches for dotnet <=> Visual Studio integration
                if (optionName == "-test" || optionName == "--test")
                {
                    if (option.Value == null)
                        throw new ArgumentException("missing argument for --test");

                    // TODO
                    throw new NotImplementedException();
                }
                else if (optionName == "-list" || optionName == "--list")
                {
                    GuardNoOptionValue(option);
                    List = true;
                }
                else if (optionName == "-designtime" || optionName == "--designtime")
                {
                    GuardNoOptionValue(option);
                    DesignTime = true;
                }
                else if (optionName == "-port" || optionName == "--port")
                {
                    if (option.Value == null)
                    {
                        throw new ArgumentException("missing argument for -port");
                    }

                    int port;
                    if (!int.TryParse(option.Value, out port) || port < 0)
                    {
                        throw new ArgumentException("incorrect argument value for -port (must be a positive number)");
                    }

                    Port = port;
                }
                else if (optionName == "-wait-command" || optionName == "--wait-command")
                {
                    GuardNoOptionValue(option);
                    WaitCommand = true;
                }
            }

            if (WaitCommand && !Port.HasValue)
            {
                throw new ArgumentException("when specifing --wait-command you must also pass a port using --port");
            }
        }

        private static KeyValuePair<string, string> PopOption(Stack<string> arguments)
        {
            var option = arguments.Pop();
            string value = null;

            if (arguments.Count > 0 && !arguments.Peek().StartsWith("-"))
                value = arguments.Pop();

            return new KeyValuePair<string, string>(option, value);
        }

        private static void GuardNoOptionValue(KeyValuePair<string, string> option)
        {
            if (option.Value != null)
                throw new ArgumentException(string.Format("error: unknown command line option: {0}", option.Value));
        }

    }
}
