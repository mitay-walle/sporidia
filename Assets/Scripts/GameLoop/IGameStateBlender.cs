using System;

namespace GameJam.Plugins.GameLoop
{
    public interface IGameStateBlender
    {
        event Action<GameplayState> OnBlendDone;

        public void Init(GameplayEvents events);
        public void StartBlend(GameplayState previousState,GameplayState nextState);
    }
}
