using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace GameJam.Plugins.UI.Input
{
	public class HotkeySubmitUI : MonoBehaviour
	{
		[SerializeField, Required] private InputActionReference _action;

		private void Start() => Init();
		private void OnDestroy() => Dispose();

		public void Init()
		{
			if (_action) _action.action.started -= Execute;
			if (_action) _action.action.started += Execute;
		}
		private void Dispose()
		{
			if (_action) _action.action.started -= Execute;
		}

		private void Execute(InputAction.CallbackContext obj) => Execute();

		public void Execute()
		{
			ExecuteEvents.ExecuteHierarchy(gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
		}
	}
}