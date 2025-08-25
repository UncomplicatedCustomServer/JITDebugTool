using Exiled.API.Interfaces;

namespace JITDebugTool.API.SerializedElements
{
    internal class SerializedTargetPlugin(IPlugin<IConfig> plugin)
    {
        public string Name { get; } = plugin.Name;

        public string Prefix { get; } = plugin.Prefix;

        public string Author { get; } = plugin.Author;

        public string Version { get; } = plugin.Version.ToString();

        public string AssemblyName { get; } = plugin.Assembly.FullName;

        public string AssemblySimpleName { get; } = plugin.Assembly.GetName().Name;
    }
}
