using Exiled.API.Features;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace AmazingDebugTool
{
    internal class Patch
    {
        private static void Prefix(out Tuple<Stopwatch, StackTrace> __state)
        {
            __state = new(new(), new());
            __state.Item1.Start();
        }

        private static void Postfix(object __instance, MethodInfo __originalMethod, Tuple<Stopwatch, StackTrace> __state)
        {
            __state.Item1.Stop();

            Task.Run(() => Plugin.Instance.Writer.Write(__instance, __state.Item1, __originalMethod, [], __state.Item2));
        }
    }
}
