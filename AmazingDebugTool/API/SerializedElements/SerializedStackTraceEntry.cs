using JITDebugTool.API.Extensions;
using System.Diagnostics;

namespace JITDebugTool.API.SerializedElements
{
    internal class SerializedStackTraceEntry
    {
        public SerializedMethod Caller { get; }

        public string File { get; }

        public int Line { get; }

        public int Column { get; }

        public int ILOffset { get; }

        public int NativeOffset { get; }

        public SerializedStackTraceEntry(StackFrame frame)
        {
            if (Bucket.stackTraceMethodCache.TryGetValue(frame.GetMethod().GetSignature(), out SerializedMethod method))
            File = frame.GetFileName();
            Line = frame.GetFileLineNumber();
            Column = frame.GetFileColumnNumber();
            ILOffset = frame.GetILOffset();
            NativeOffset = frame.GetNativeOffset();
        }
    }
}
