using UnityEngine;

namespace GameJam.Plugins.PlayerControl
{
	public class PlayerCamera : MonoBehaviour
	{
		[SerializeField] private float _speed = .1f;
		[SerializeField] private Transform _target;

		private void Awake()
		{
			//_target = FindAnyObjectByType<Player>().transform;
		}

		private void Update()
		{
			if (!_target) return;

			Vector3 newPosition = Vector3.Lerp(transform.position, _target.position, _speed * Time.deltaTime);
			newPosition.z = transform.position.z;
			transform.position = newPosition;
		}
	}
}