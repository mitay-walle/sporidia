using System;
using GameJam.Player;
using Unity.Cinemachine;
using UnityEngine;

namespace GameJam.Plugins.GameLoop
{
    [Serializable]
    public class PlayerAsist
    {
        [SerializeField] private CinemachineBrain _cinemachineBrain;
        
        [field: SerializeField] public PlayerMovement DayPlayer { get; private set; }
        [field: SerializeField] public Transform DayHouse;

        [field: SerializeField] public Transform TwilightHouse;

        [field: SerializeField] public PlayerMovement NightHouse;

        public CinemachineCamera ActiveCamera => _cinemachineBrain.ActiveVirtualCamera as CinemachineCamera;
        
        public void SyncHouse(GameplayState state)
        {
            switch (state)
            {
                case GameplayState.Initial:
                    break;
                case GameplayState.Day:
                    TwilightHouse.SetPositionAndRotation(DayHouse.position,DayHouse.rotation);
                    NightHouse.SetPositionAndRotation(DayHouse.position,DayHouse.rotation);
                    break;
                case GameplayState.Twighlight:
                    var position = TwilightHouse.position;
                    var rotation = TwilightHouse.rotation;
                    NightHouse.SetPositionAndRotation(position,rotation);
                    DayHouse.SetPositionAndRotation(position,rotation);
                    break;
                case GameplayState.Night:
                    DayHouse.SetPositionAndRotation(NightHouse.GetPosition(),NightHouse.GetRotation());
                    TwilightHouse.SetPositionAndRotation(NightHouse.GetPosition(),NightHouse.GetRotation());
                    break;
                case GameplayState.Lose:
                    break;
            }
        }
    }
}
