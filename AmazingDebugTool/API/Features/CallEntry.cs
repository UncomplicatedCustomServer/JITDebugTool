using System;
using System.Diagnostics;
using System.Reflection;

namespace JITDebugTool.API.Features
{
    internal class CallEntry(MethodInfo method, Stopwatch stopwatch, StackTrace stackTrace, object @object)
    {
        public MethodInfo Method { get; } = method;

        public Stopwatch Stopwatch { get; } = stopwatch;

        public StackTrace StackTrace { get; } = stackTrace;

        public object Object { get; } = @object;

        public DateTimeOffset Time { get; } = DateTimeOffset.Now;
    }
}
