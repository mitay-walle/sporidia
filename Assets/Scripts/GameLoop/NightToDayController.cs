using System;
using UnityEngine;

namespace GameJam.Plugins.GameLoop
{
    public class NightToDayController : MonoBehaviour,IGameStateBlender
    {
        private static Collider[] BUFFER = new Collider[1];
        private const float OFFSET = 2f;

        private PlayerAsist _playerAsist;

        public event Action<GameplayState> OnBlendDone;
        
        public void Init(GameplayEvents events)
        {
            _playerAsist = events.PlayerAsist;
        }

        public void StartBlend(GameplayState previousState, GameplayState nextState)
        {
            if(previousState != GameplayState.Night) return;
            
            var radius = _playerAsist.DayPlayer.GetComponent<CapsuleCollider>().radius;
            var height = _playerAsist.DayPlayer.GetComponent<CapsuleCollider>().height;

            Vector3 position = GetSphereCastPosition(false, height);
            int overlapCount = Physics.OverlapSphereNonAlloc(position, radius, BUFFER, LayerMask.NameToLayer("Player"));

            if (overlapCount == 0)
            {
                _playerAsist.DayPlayer.SetPositionAndRotation(GetSpawnPosition(false),_playerAsist.NightHouse.GetRotation());
            }
            else
            {
                position = GetSphereCastPosition(true, height);
                overlapCount = Physics.OverlapSphereNonAlloc(position, radius, BUFFER, LayerMask.NameToLayer("Player"));
                
                if (overlapCount == 0)
                {
                    _playerAsist.DayPlayer.SetPositionAndRotation(GetSpawnPosition(true),_playerAsist.NightHouse.GetRotation());
                }
                else
                {
                    Vector3 cache = _playerAsist.NightHouse.GetPosition();
                    _playerAsist.NightHouse.SetPositionAndRotation(GetSpawnPosition(true), _playerAsist.NightHouse.GetRotation());
                    _playerAsist.DayPlayer.SetPositionAndRotation(cache,_playerAsist.NightHouse.GetRotation());
                }
            }
            
            _playerAsist.SyncHouse(GameplayState.Night);
            
            OnBlendDone?.Invoke(nextState);
        }
        
        private Vector3 GetSpawnPosition(bool reverse)
        {
            return _playerAsist.NightHouse.GetPosition() +
                   (reverse ? -_playerAsist.NightHouse.GetForward() : _playerAsist.NightHouse.GetForward());
        }
        
        private Vector3 GetSphereCastPosition(bool reverse,float height)
        {
            return GetSpawnPosition(reverse) + Vector3.up*height*0.5f;
        }
    }
}
