using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GameJam.Plugins.UI.Windows
{
	[RequireComponent(typeof(Button))]
	public class CloseWindowButton : MonoBehaviour
	{
		[SerializeField] private Panel _window;
		[SerializeField] private InputActionReference _hotkey;

		private void OnEnable()
		{
			if (_hotkey)
			{
				_hotkey.action.started -= CloseWindow;
				_hotkey.action.started += CloseWindow;
			}
			if (TryGetComponent<Button>(out var button))
			{
				button.onClick.RemoveListener(CloseWindow);
				button.onClick.AddListener(CloseWindow);
			}
		}

		private void OnDisable()
		{
			if (_hotkey)
			{
				_hotkey.action.started -= CloseWindow;
			}
		}

		private void CloseWindow(InputAction.CallbackContext callbackContext) => CloseWindow();

		private void CloseWindow()
		{
			if (_window == null) _window = GetComponentsInParent<Panel>(true)[0];
			if (_window) _window.Hide();
		}
	}
}