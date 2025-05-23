using UltEvents;
using UnityEngine;

namespace GameJam.Plugins.Navigation
{
	public class RotateUltEvent : MonoBehaviour
	{
		[SerializeField] private UltEvent<Vector3> _event;

		private void FixedUpdate()
		{
			Vector3 direction = default;
			_event.Invoke(direction);
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
		}
	}
}