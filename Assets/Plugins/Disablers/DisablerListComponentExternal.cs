using System.Reflection;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace GameJam.Plugins.Disablers
{
	public class DisablerListComponentExternal<TComponent> : DisablerListComponentExternal where TComponent : Component
	{
		private void Reset() => _component = _component ? _component : GetComponentInChildren<TComponent>();
	}
	public class DisablerListComponentExternal : DisablerListBehaviour
	{
		[SerializeField, Required] protected Component _component;

		protected PropertyInfo _property;

		protected override void OnChanged()
		{
			if (_component)
			{
				if (_component is Behaviour beh)
				{
					beh.enabled = DisablerList.IsEnabled;
				}
				else
				{
					_property ??= _component.GetType().GetProperty("enabled");
					_property.SetValue(_component, DisablerList.IsEnabled);
				}
			}
		}

#if UNITY_EDITOR
		private void OnInspectorGUI()
		{
			if (!_component || _component is Behaviour) return;

			_property ??= _component.GetType().GetProperty("enabled");
			if (_property == null)
			{
				EditorGUILayout.HelpBox($"Component '{_component.GetType().Name}' has no Enabled property!", MessageType.Error);
			}
		}
  #endif
	}
}