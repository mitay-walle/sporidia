using UnityEngine;
using UnityEngine.Events;

namespace Plugins.GameObjectOperations
{
	public class GameObjectEvents : MonoBehaviour
	{
		public UnityEvent Enabled;
		public UnityEvent Disabled;
		public UnityEvent Destroyed;
		private void OnEnable() => Enabled?.Invoke();
		private void OnDisable() => Disabled?.Invoke();
		private void OnDestroy() => Destroyed?.Invoke();
	}
}