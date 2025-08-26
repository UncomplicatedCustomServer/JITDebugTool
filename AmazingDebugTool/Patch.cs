using Exiled.API.Features;
using JITDebugTool.API.Features;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace JITDebugTool
{
    internal class Patch
    {
        private static void Prefix(out Tuple<Stopwatch, StackTrace, CallEntry> __state)
        {
            __state = new(new(), new(), Plugin.Instance.writer.PreWrite());
            __state.Item1.Start();
        }

        private static void Postfix(object __instance, MethodInfo __originalMethod, Tuple<Stopwatch, StackTrace, CallEntry> __state)
        {
            __state.Item1.Stop();

            Task.Run(() => Writer.Write(__state.Item3, __instance, __state.Item1, __originalMethod, __state.Item2));
        }
    }
}
