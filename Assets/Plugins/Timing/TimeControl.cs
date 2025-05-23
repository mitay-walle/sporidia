using GameJam.Plugins.Disablers;
using TriInspector;
using UnityEngine;

namespace Survival.Plugins.Development
{
	public class TimeControl
	{
		[ShowInInspector] public DisablerList DisablerList { get; private set; } = new();
		[ShowInInspector] private float timeScale => Time.timeScale;

		public TimeControl()
		{
			DisablerList.OnChanged += OnChanged;
			OnChanged(DisablerList.IsEnabled, null);
		}

		private void OnChanged(bool isEnabled, object disabler)
		{
			Time.timeScale = isEnabled ? 1 : 0;
		}
	}
}