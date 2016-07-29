using System;
using System.Diagnostics;

namespace Machine.Specifications.Runner.DotNet.Execution
{
    public class RunStats
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public RunStats()
        {
            _stopwatch.Start();
            Start = DateTime.Now;
        }

        public DateTimeOffset Start { get; private set; }

        public DateTimeOffset End { get; private set; }

        public TimeSpan Duration {
            get { return _stopwatch.Elapsed; }
        }

        public void Stop()
        {
            _stopwatch.Stop();
            End = Start + _stopwatch.Elapsed;
        }
    }
}
