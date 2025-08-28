using System.Collections.Generic;

namespace JITDebugTool.API.SerializedElements
{
    internal class SerializedMessage(List<SerializedCallEntry> calls, List<SerializedMethod> methods)
    {
        public List<SerializedCallEntry> Calls { get; } = calls;

        public List<SerializedMethod> Methods { get; } = methods;
    }
}
