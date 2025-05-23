using GameJam.Plugins.Disablers;
using UnityEngine;

namespace GameJam.Plugins.UI.Input
{
	public class CursorControl
	{
		public static CursorControl Instance = new();
		public CursorLockMode DefaultMode = CursorLockMode.Confined;

		public DisablerList Enablers;

		public CursorControl()
		{
			Enablers = new DisablerList(Toggle);
			Toggle(true);
		}

		private void Toggle(bool isDisabled)
		{
			Cursor.visible = !isDisabled;
			Cursor.lockState = isDisabled ? CursorLockMode.Locked : DefaultMode;
			Debug.Log($"Cursor Toggle | isDisabled {isDisabled}");
		}
	}
}