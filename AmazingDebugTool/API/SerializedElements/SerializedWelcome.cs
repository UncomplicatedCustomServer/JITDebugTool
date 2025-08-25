using Exiled.Loader;

namespace JITDebugTool.API.SerializedElements
{
    internal class SerializedWelcome
    {
        public string Version { get; } = Plugin.Instance.Version.ToString();

        public SerializedTargetPlugin TargetPlugin { get; } = Plugin.Instance.patcher.plugin is not null ? new(Plugin.Instance.patcher.plugin) : null;

        public string TargetPluginName { get; } = Plugin.Instance.Config.Plugin;

        public bool Exiled { get; } = true;

        public string ExiledVersion { get; } = Loader.Version.ToString();
    }
}
