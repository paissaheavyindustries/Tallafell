using Dalamud.Game;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Game.ClientState.Objects.Types;
using CharacterStruct = FFXIVClientStructs.FFXIV.Client.Game.Character.Character;
using GameObjectStruct = FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject;
using System;
using Dalamud.Plugin.Services;

namespace Tallafell
{

    public sealed class Plugin : IDalamudPlugin
    {

        public string Name => "Tallafell";

        private DalamudPluginInterface _pi { get; init; }
        private IObjectTable _ot { get; init; }
        private IChatGui _cg { get; init; }
        private IClientState _cs { get; init; }
        private DateTime _nextCheck = DateTime.Now;

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] IObjectTable objectTable,
            [RequiredVersion("1.0")] IClientState clientState,
            [RequiredVersion("1.0")] IChatGui chatGui
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
                        bcs->CharacterData.ModelScale = 2.0f;
                        gos->Scale = 2.0f;
                    }
                }
            }
        }

    }
}
