using Dalamud.Plugin;
using Dalamud.Game.ClientState.Objects.Types;
using CharacterStruct = FFXIVClientStructs.FFXIV.Client.Game.Character.Character;
using GameObjectStruct = FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject;
using System;

namespace Tallafell
{

    public sealed class Plugin : IDalamudPlugin
    {

        public string Name => "Tallafell";

        private Service _svc = null;
        private DateTime _nextCheck = DateTime.Now;

        public Plugin(IDalamudPluginInterface pluginInterface)
        {
            _svc = pluginInterface.Create<Service>();
            _svc.pi.UiBuilder.Draw += DrawUI;
        }

        public void Dispose()
        {
            _svc.pi.UiBuilder.Draw -= DrawUI;
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
            foreach (IGameObject go in _svc.ot)
            {
                if (go is ICharacter)
                {
                    ICharacter bc = (ICharacter)go;
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
