using System;
using System.Diagnostics;
using System.Reflection;

namespace JITDebugTool.API.Features
{
    internal class CallEntry(Guid? guid = null)
    {
        public Guid Id { get; private set; } = guid ?? Guid.NewGuid();

        public MethodInfo Method { get; private set; }

        public Stopwatch Stopwatch { get; private set; }

        public StackTrace StackTrace { get; private set; }

        public object Object { get; private set; }

        public DateTimeOffset Time { get; } = DateTimeOffset.Now;

        public bool IsReady { get; private set; } = false;

        internal void Populate(MethodInfo method, Stopwatch stopwatch, StackTrace stackTrace, object @object)
        {
            Method = method;
            Stopwatch = stopwatch;
            Object = @object;
            StackTrace = stackTrace;
            IsReady = true;
        }
    }
}
