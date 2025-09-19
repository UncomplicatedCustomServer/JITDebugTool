using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;
using JITDebugTool.API.Features;
using System;

namespace JITDebugTool
{
    internal class Plugin : Plugin<Config>
    {
        public override string Name => "JITDebugTool";

        public override string Prefix => "jit_debug_tool";

        public override string Author => "FoxWorn3365 & UCS Collective";

        public override Version Version => new(1, 0, 0);

        public override PluginPriority Priority => PluginPriority.First;

        internal static Plugin Instance { get; private set; }

        internal Writer writer;

        internal Patcher patcher;

        private Harmony _harmony;

        public override void OnEnabled()
        {
            Instance = this;
            writer = new();
            writer.Start();
            new SocketServer();

            _harmony = new($"adb-{Guid.NewGuid()}");

            patcher = new(_harmony);
            patcher.PatchMethods();

            Log.Info("Welcome on JITDebugTool!");

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
