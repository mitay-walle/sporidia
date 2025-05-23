using System;
using GameJam.Scripts.GameLoop;
using UnityEngine;

namespace GameJam.Plugins.GameLoop
{
    public class DayToTwilightController : MonoBehaviour,IGameStateBlender
    {
        private PlayerAsist _playerAsist;
        private GameplayState _nextState;
        private HouseAutoMovement _autoMovement;
        
        public event Action<GameplayState> OnBlendDone;

        public void Init(GameplayEvents events)
        {
            _playerAsist = events.PlayerAsist;
            _autoMovement = _playerAsist.DayHouse.GetComponent<HouseAutoMovement>();
            
            _autoMovement.enabled = false;
            _playerAsist.DayPlayer.enabled = true;
            _autoMovement.OnReady += OnDayEndedCallback;
        }

        private void OnDayEndedCallback()
        {
            _autoMovement.enabled = false;
            _playerAsist.DayPlayer.enabled = true;

            _playerAsist.SyncHouse(GameplayState.Day);
            _playerAsist.ActiveCamera.Target.TrackingTarget = _playerAsist.DayPlayer.transform;
            OnBlendDone?.Invoke(_nextState);
        }

        public void StartBlend(GameplayState previousState, GameplayState nextState)
        {
            if (previousState != GameplayState.Day)
            {
                OnBlendDone?.Invoke(nextState);
                return;
            }
            
            _nextState = nextState;
            _autoMovement.enabled = true;
            _playerAsist.DayPlayer.enabled = false;
            _playerAsist.ActiveCamera.Target.TrackingTarget = _autoMovement.transform;
        }
    }
}
