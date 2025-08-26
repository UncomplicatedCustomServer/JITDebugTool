using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Loader;
using HarmonyLib;
using JITDebugTool.API.SerializedElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JITDebugTool
{
    internal class Patcher
    {
        public readonly Assembly targetAssembly;

        public readonly IReadOnlyList<Type> types;

        public readonly IPlugin<IConfig> plugin;

        public SerializedPluginData pluginData = null;

        private readonly Harmony _harmony;

        private readonly MethodInfo _prefixMethod = typeof(Patch).GetMethod("Prefix", BindingFlags.Static | BindingFlags.NonPublic);
        private readonly MethodInfo _postfixMethod = typeof(Patch).GetMethod("Postfix", BindingFlags.Static | BindingFlags.NonPublic);

        public int PatchedMethods = 0;

        public Patcher(Harmony harmony)
        {
            plugin = Loader.GetPlugin(Plugin.Instance.Config.Plugin);

            if (plugin is null)
            {
                Log.Warn($"ERROR: Plugin {Plugin.Instance.Config.Plugin} not found!");
                Log.Warn($"Available plugins: {string.Join(",", Loader.Plugins.Select(p => p.Name))}");
                return;
            }

            targetAssembly = plugin.Assembly;

            List<Type> types = [..plugin.Assembly.GetTypes()];
            types.RemoveAll(t => t.IsInterface);
            types.RemoveAll(t => Plugin.Instance.Config.IgnoreTypes.Contains(t.Name));
            types.RemoveAll(t => t.GetCustomAttributes(typeof(HarmonyPatch), false).Any()); // Ignore harmony plz

            this.types = types;
            _harmony = harmony;
        }

        public void PatchMethods()
        {
            pluginData = new([.. types.Select(t => t.FullName)], DateTimeOffset.Now.ToUnixTimeMilliseconds(), types.Count);

            foreach (Type type in types)
                foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(m => !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_")))
                    if (!Plugin.Instance.Config.IgnoreMethods.Contains(method.Name))
                        PatchMethod(method, type);

            Log.Info($"Successfully patched {types.Count} types and {PatchedMethods} methods!");
            pluginData.TotalPatchedMethods = PatchedMethods;
        }

        private void PatchMethod(MethodInfo method, Type type)
        {
            try
            {
                _harmony.Patch(
                    original: method,
                    prefix: new HarmonyMethod(_prefixMethod),
                    postfix: new HarmonyMethod(_postfixMethod)
                );

                PatchedMethods++;
                Log.Info($"Successfully patched method {type.FullName}::{method.Name}() !");
                pluginData.Methods.Add(new(method));
            } catch (Exception e)
            {
                pluginData.NotPatchedMethods.Add(new(method));
                if (e.GetType() == typeof(NotSupportedException))
                    Log.Warn($"Wont patch method {type.FullName}{(method.IsStatic ? "::" : ".")}{method.Name}() - not supported!");
                else
                    Log.Error($"Wont patch method {type.FullName}{(method.IsStatic ? "::" : ".")}{method.Name}() - Generic exception:\n{e}");
            }
        }
    }
}
