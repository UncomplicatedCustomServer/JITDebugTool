using System;
using System.Diagnostics;

namespace JITDebugTool.API.SerializedElements
{
    internal class SerializedStopwatch(Stopwatch stopwatch, DateTimeOffset @base)
    {
        public long Milliseconds { get; } = stopwatch.ElapsedMilliseconds;

        public long Ticks { get; } = stopwatch.ElapsedTicks;

        public string Time { get; } = $"{@base.Minute}:{@base.Second}:{@base.Millisecond}*{@base.Ticks}";
    }
}
