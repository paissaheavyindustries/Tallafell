using Dalamud.Game;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.ClientState.Objects.Types;
using CharacterStruct = FFXIVClientStructs.FFXIV.Client.Game.Character.Character;
using GameObjectStruct = FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject;
using Dalamud.Game.Gui;
using Dalamud.Game.ClientState;
using System;

namespace Tallafell
{

    public sealed class Plugin : IDalamudPlugin
    {

        public string Name => "Tallafell";

        private DalamudPluginInterface _pi { get; init; }
        private ObjectTable _ot { get; init; }
        private ChatGui _cg { get; init; }
        private ClientState _cs { get; init; }
        private DateTime _nextCheck = DateTime.Now;

        [PluginService]
        public static SigScanner TargetModuleScanner { get; private set; }

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] ObjectTable objectTable,
            [RequiredVersion("1.0")] ClientState clientState,
            [RequiredVersion("1.0")] ChatGui chatGui
        )
        {
            _pi = pluginInterface;
            _ot = objectTable;
            _cg = chatGui;
            _cs = clientState;
            _pi.UiBuilder.Draw += DrawUI;
        }

        public void Dispose()
        {
            _pi.UiBuilder.Draw -= DrawUI;
        }

        private void DrawUI()
        {
            if (DateTime.Now > _nextCheck)
            {
                Tallafellify();
                _nextCheck = DateTime.Now.AddMicroseconds(100);
            }
        }

        private unsafe void Tallafellify()
        {
            foreach (GameObject go in _ot)
            {
                if (go is Character)
                {
                    Character bc = (Character)go;
                    if (bc.Customize[0] == 3)
                    {
                        CharacterStruct* bcs = (CharacterStruct*)bc.Address;
                        GameObjectStruct* gos = (GameObjectStruct*)go.Address;
                        bcs->ModelScale = 2.0f;
                        gos->Scale = 2.0f;
                    }
                }
            }
        }

    }
}
