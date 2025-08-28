using JITDebugTool.API.Extensions;
using System.Diagnostics;

namespace JITDebugTool.API.SerializedElements
{
    internal class SerializedStackTraceEntry
    {
        public string Caller { get; }

        public string File { get; }

        public int Line { get; }

        public int Column { get; }

        public int ILOffset { get; }

        public int NativeOffset { get; }

        public SerializedStackTraceEntry(StackFrame frame)
        {
            Caller = frame.GetMethod().GetSignature();
            if (!Plugin.Instance.writer.fullMethods.ContainsKey(Caller))
            {
                SerializedMethod method = new(frame.GetMethod(), true);
                Plugin.Instance.writer._methods.Enqueue(method);
                Plugin.Instance.writer.fullMethods[Caller] = method;
            }

            File = frame.GetFileName();
            Line = frame.GetFileLineNumber();
            Column = frame.GetFileColumnNumber();
            ILOffset = frame.GetILOffset();
            NativeOffset = frame.GetNativeOffset();
        }
    }
}
