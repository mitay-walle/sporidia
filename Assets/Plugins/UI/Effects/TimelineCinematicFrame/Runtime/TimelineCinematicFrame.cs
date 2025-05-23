using TriInspector;
using UnityEngine;

namespace GameJam.Plugins.UI.Effects.TimelineCinematicFrame.Runtime
{
	public class TimelineCinematicFrame : MonoBehaviour
	{
		[SerializeField] private RectTransform _top;
		[SerializeField] private RectTransform _bottom;
		[SerializeField, OnValueChanged(nameof(SetHeight))] private int _height = 300;
		[ShowInInspector, OnValueChanged(nameof(SetValue)),Range(0,1)] private float _state;

		public void SetValue(float normalizedState)
		{
			_state = normalizedState;
			Vector2 position = new Vector2(0, _height - normalizedState * _height);
			_top.anchoredPosition = position;
			_bottom.anchoredPosition = -position;
		}

		private void SetHeight(float value)
		{
			Vector2 sizeDelta = _top.sizeDelta;
			sizeDelta.y = value;
			_bottom.sizeDelta = _top.sizeDelta = sizeDelta;
		}
	}
}