using System;
using UnityEngine;

namespace GameJam.Plugins.GameLoop
{
    public class EmptyBlendController : MonoBehaviour,IGameStateBlender
    {

        public event Action<GameplayState> OnBlendDone;
        public void Init(GameplayEvents events)
        {
        }

        public void StartBlend(GameplayState previousState, GameplayState nextState)
        {
            OnBlendDone?.Invoke(nextState);
        }
    }
}
