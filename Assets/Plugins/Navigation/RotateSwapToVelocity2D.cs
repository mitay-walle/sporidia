using TriInspector;
using UnityEngine;
using UnityEngine.AI;

namespace GameJam.Plugins.Navigation
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class RotateSwapToVelocity2D : MonoBehaviour
	{
		private Rigidbody2D _rigidbody2D;
		private NavMeshAgent _agent;
		[ShowInInspector, ReadOnly] private Vector3 _velocity;

		private void Awake()
		{
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_agent = GetComponent<NavMeshAgent>();
		}

		private void FixedUpdate()
		{
			_velocity = _rigidbody2D.linearVelocity;
			if (_agent)
			{
				if (_agent.velocity.sqrMagnitude > .001f)
				{
					_velocity = _agent.velocity;
				}
			}

			if (Mathf.Abs(_velocity.x) > .01f)
			{
				float y = _velocity.x > 0 ? 0 : 180;
				transform.rotation = Quaternion.Euler(0, y, 0);
			}
		}
	}
}