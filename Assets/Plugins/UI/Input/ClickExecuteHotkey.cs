using System.Linq;
using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

namespace GameJam.Plugins.UI.Input
{
	public class ClickExecuteHotkey : MonoBehaviour
	{
		[SerializeField, Required] private InputActionReference _action;

		private Selectable _selectable;

		private void Start()
		{
			if (!_action) return;
			_selectable = GetComponentInParent<Selectable>();

			switch (_selectable)
			{
				case Button button:
					{
						button.onClick.RemoveListener(ExecuteButton);
						button.onClick.AddListener(ExecuteButton);
						break;
					}
				case Toggle toggle:
					{
						toggle.onValueChanged.RemoveListener(ExecuteToggle);
						toggle.onValueChanged.AddListener(ExecuteToggle);
						break;
					}
			}
		}

		private void ExecuteButton()
		{
			Execute(true);
			Execute(false);
		}
		private void ExecuteToggle(bool value) => Execute(true);
		private void Execute(bool value)
		{
			if (!enabled || _selectable && !_selectable.interactable) return;
			if (!_action) return;

			InputControl found = _action.action.controls.FirstOrDefault(control => control is ButtonControl && control.device is { enabled: true });

			if (found == null)
			{
				Debug.LogError($"[ClickExecuteHotkey] ButtonControl with device.enabled not found");
				return;
			}

			using (StateEvent.From(found.device, out var eventPtr))
			{
				(found as ButtonControl).WriteValueIntoEvent(value ? 1f : 0f, eventPtr);
				InputSystem.QueueEvent(eventPtr);
			}
		}
	}
}