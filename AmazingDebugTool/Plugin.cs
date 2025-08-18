using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;
using System;
using System.IO;

namespace AmazingDebugTool
{
    internal class Plugin : Plugin<Config>
    {
        public override string Name => "AmazingDebugTool";

        public override string Prefix => "adb";

        public override string Author => "FoxWorn3365 & UCS Collective";

        public override Version Version => new(1, 0, 0);

        public override PluginPriority Priority => PluginPriority.First;

        internal static Plugin Instance;

        internal Writer Writer;

        private Harmony _harmony;

        private Patcher _patcher;

        public override void OnEnabled()
        {
            Instance = this;
            Writer = new(Path.Combine(Paths.Configs, $"adt-log-{DateTime.Now:F}.txt".Replace("/", "-").Replace(" ", "-").Replace(":", "-")));
            Writer.Start();

            _harmony = new($"adb-{Guid.NewGuid()}");

            _patcher = new(_harmony);
            _patcher.PatchMethods();

            Log.Info("Welcome on AmazingDebugTool!");

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            _harmony.UnpatchAll();
            _harmony = null;

            Instance = null;

            base.OnDisabled();
        }
    }
}
