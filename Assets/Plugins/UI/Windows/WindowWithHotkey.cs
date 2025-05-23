using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace GameJam.Plugins.UI.Windows
{
	/// <summary>
	/// Show/Hide/Toggle object
	/// </summary>
	public abstract class WindowWithHotkey : Panel
	{
		[SerializeField] private InputActionReference _hotkey;
		[SerializeField] private InputActionReference _closeHotkey;
		public virtual void InitHotkey()
		{
			if (_hotkey)
			{
				_hotkey.action.started += Toggle;
			}
			
			if (_closeHotkey)
			{
				_closeHotkey.action.started += Close;
			}
		}
		private void Toggle(CallbackContext obj)
		{
			Debug.Log($"Click hotkey '{_hotkey.action.GetBindingDisplayString()}' for '{name}'", this);
			Toggle();
		}
		
		private void Close(CallbackContext obj)
		{
			if (IsVisible)
			{
				Debug.Log($"Click hotkey '{_closeHotkey.action.GetBindingDisplayString()}' for '{name}'", this);
				Toggle(false);
			}
		}
		
		protected virtual void OnDestroy()
		{
			DisposeHokey();
		}
		public void DisposeHokey()
		{
			if (_hotkey)
			{
				_hotkey.action.started -= Toggle;
			}
			
			if (_closeHotkey)
			{
				_closeHotkey.action.started -= Close;
			}
		}
	}
}