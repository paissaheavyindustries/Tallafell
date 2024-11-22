using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace Tallafell
{

    internal class Service
    {

        [PluginService] public IDalamudPluginInterface pi { get; private set; }
        [PluginService] public IObjectTable ot { get; private set; }
        [PluginService] public IChatGui cg { get; private set; }
        [PluginService] public IClientState cs { get; private set; }

    }

}
