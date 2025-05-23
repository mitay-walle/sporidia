using System;
using GameJam.Plugins.Timing;
using Gameplay.Interactions.Damages;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace GameJam.Scripts.GameLoop
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class HouseAutoMovement : MonoBehaviour
    {
        private const float NAV_MESH_SEARCH_DISTANCE = 25f;
        private const float REACH_DISTANCE = 0.5f;

        public event Action OnReady; 
        
        [SerializeField] private float _spawnDistance = 80;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private AnimatorParameter _moveAnimation = new();

        private Timer _timer = new Timer(10);
        
        private void OnEnable()
        {
            Vector3 position = transform.position;

            if (Vector3.Distance(position, _playerTransform.position) > _spawnDistance)
            {
                float angle = Random.Range(0, Mathf.PI * 2f);

               position = new Vector3(_spawnDistance * Mathf.Cos(angle), 0, _spawnDistance * Mathf.Sin(angle)) +
                                   _playerTransform.position;
            }
            

            if (NavMesh.SamplePosition(position, out var hit, NAV_MESH_SEARCH_DISTANCE, -1))
            {
                transform.position = hit.position;
                _timer.Restart();
            }
            else
            {
                transform.position = _playerTransform.position;
                OnReady?.Invoke();
            }
            
            _navMeshAgent.enabled = true;
            _navMeshAgent.destination = _playerTransform.position;
        }

        private void OnDisable()
        {
            _navMeshAgent.enabled = false;
            if (_moveAnimation.animator && !string.IsNullOrEmpty(_moveAnimation.parameter))
            {
                _moveAnimation.animator.SetFloat(_moveAnimation.parameter, 0);
            }
        }

        private void Update()
        {
            if (_moveAnimation.animator && !string.IsNullOrEmpty(_moveAnimation.parameter))
            {
                _moveAnimation.animator.SetFloat(_moveAnimation.parameter, _navMeshAgent.velocity.magnitude);
            }
            
            if (_timer.IsReady())
            {
                transform.position = _playerTransform.position;
                OnReady?.Invoke();
            }
            else if (Vector3.Distance(transform.position, _playerTransform.position) < REACH_DISTANCE)
            {
                OnReady?.Invoke();
            }
        }

        private void Reset()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }
    }
}
