using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Exiled.API.Features;

namespace AmazingDebugTool
{
    internal class Writer(string fileName)
    {
        private ConcurrentQueue<string> _builder = new();

        private readonly Stopwatch _stopwatch = new();

        private readonly string _fileName = fileName;

        public void Start()
        {
            _stopwatch.Start();
            _builder.Enqueue(string.Join(" | ", [
                "Elapsed".FillToAlign(9),
                "Assembly".FillToAlign(25),
                "Method".FillToAlign(150),
                "Ticks".FillToAlign(7),
                "Milliseconds",
                "Trace 1".FillToAlign(150),
                "Trace 2".FillToAlign(150)
            ]));
        }

        public void Write(object obj, Stopwatch stopwatch, MethodInfo method, object[] args, StackTrace stackTrace)
        {
            try
            {
                /*MethodBase callerMethod = stackTrace.GetFrame(2)?.GetMethod();
                string callerName = callerMethod != null ? callerMethod.Name : "Sconosciuto";
                string callerClassName = callerMethod != null ? callerMethod.DeclaringType.FullName : "Sconosciuto";*/

                /*if (_stopwatch is null)
                    Log.Error("ALARM 1");

                if (obj is null)
                    Log.Error("ALARM 2");

                if (method is null)
                    Log.Error("ALARM 3");

                if (stopwatch is null)
                    Log.Error("ALARM 4");

                if (stackTrace is null)
                    Log.Error("ALARM 5");*/
                string data;

                if (!Plugin.Instance.Config.Grid)
                    data = $"[+{_stopwatch.Elapsed.Minutes}:{_stopwatch.Elapsed.Seconds}:{_stopwatch.Elapsed.Milliseconds}] [{method.DeclaringType.Assembly.GetName().Name}] Method {method.DeclaringType.FullName}{(obj != null ? "." : "::")}{method.Name}({RenderArgs(method)}){(method.IsStatic ? "*" : "")} took {stopwatch.ElapsedTicks} ticks ({stopwatch.ElapsedMilliseconds}ms) to execute, invoked by {GetMethodVisualFromFrame(stackTrace, 2)}";
                else
                    data = string.Join(" | ", [
                            $"+{_stopwatch.Elapsed.Minutes}:{_stopwatch.Elapsed.Seconds}:{_stopwatch.Elapsed.Milliseconds}".FillToAlign(9),
                            method.DeclaringType.Assembly.GetName().Name.FillToAlign(25),
                            $"{method.DeclaringType.FullName}{(obj != null ? "." : "::")}{method.Name}({RenderArgs(method)}){(method.IsStatic ? "*" : "")}".FillToAlign(150),
                            stopwatch.ElapsedTicks.ToString().FillToAlign(7),
                            $"{stopwatch.ElapsedMilliseconds}ms".FillToAlign(12),
                            GetMethodVisualFromFrame(stackTrace, 2).FillToAlign(100),
                            GetMethodVisualFromFrame(stackTrace, 3).FillToAlign(100),
                    ]);

                if (Plugin.Instance.Config.Detailed)
                {
                    List<string> arguments = [];
                    ParameterInfo[] parameters = method.GetParameters();
                    for (int i = 0; i < parameters.Length; i++)
                        arguments.Add($"{parameters[i].Name}: '{TryGetFromIndex(i, args)}'");

                    data += $"\n                                                         Method name: {method.Name} | Args: {string.Join(",", arguments)}";
                }
                _builder.Enqueue(data);
            } catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public void Save()
        {
            StringBuilder builder = new();
            foreach (string item in _builder)
                builder.AppendLine(item);

            _builder = new();
            File.AppendAllText(_fileName, builder.ToString());
        }

        private object TryGetFromIndex(int index, object[] pool) => pool.Length >= index ? null : pool[index];

        private string GetMethodVisualFromFrame(StackTrace stackTrace, int frame = 2)
        {
            if (frame >= stackTrace.FrameCount)
                return "Empty";

            MethodBase callerMethod = stackTrace.GetFrame(frame)?.GetMethod();
            string callerName = callerMethod != null ? callerMethod.Name : "Sconosciuto";
            string callerClassName = callerMethod != null ? callerMethod.DeclaringType.FullName : "Sconosciuto";

            return $"{callerClassName}.{callerName}({RenderArgs(callerMethod)})";
        }

        private string RenderArgs(MethodBase method)
        {
            if (!Plugin.Instance.Config.ShowArgs)
                return string.Empty;

            List<string> args = [];

            foreach (ParameterInfo parameterInfo in method.GetParameters())
                args.Add($"{parameterInfo.ParameterType.Name} {parameterInfo.Name}");

            return string.Join(", ", args);
        }
    }
}