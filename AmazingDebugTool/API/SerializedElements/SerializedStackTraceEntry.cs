using System.Diagnostics;

namespace JITDebugTool.API.SerializedElements
{
    internal class SerializedStackTraceEntry(StackFrame stackFrame)
    {
        public SerializedMethod Caller { get; } = new(stackFrame.GetMethod());

        public string File { get; } = stackFrame.GetFileName();

        public int Line { get; } = stackFrame.GetFileLineNumber();

        public int Column { get; } = stackFrame.GetFileColumnNumber();

        public int ILOffset { get; } = stackFrame.GetILOffset();

        public int NativeOffset { get; } = stackFrame.GetNativeOffset();
    }
}
