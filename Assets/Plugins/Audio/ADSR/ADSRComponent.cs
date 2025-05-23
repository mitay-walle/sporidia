using System;
using UnityEngine;

namespace Plugins.Audio
{
	public class ADSRComponent : MonoBehaviour
	{
		public Setup[] Setups;

		[Serializable]
		public class Setup
		{
			public string Key;
			public bool IsPressed;
			public ADSR ADSR;

			public void OnUpdate()
			{
				ADSR.OnUpdate(IsPressed, Time.deltaTime);
			}
		}
		private void Update()
		{
			foreach (Setup setup in Setups)
			{
				setup.OnUpdate();
			}
		}

		public void SetPressedTrue() => SetPressed(Setups[0].Key, true);
		public void SetPressedFalse() => SetPressed(Setups[0].Key, false);
		public void SetPressedTrue(string key) => SetPressed(key, true);
		public void SetPressedFalse(string key) => SetPressed(key, false);
		public void SetPressed(string key, bool isPressed)
		{
			foreach (Setup setup in Setups)
			{
				if (setup.Key == key)
				{
					setup.IsPressed = isPressed;
				}
			}
		}
	}
}