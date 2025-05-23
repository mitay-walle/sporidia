using System;
using GameJam.Plugins.Timing;
using TMPro;
using UnityEngine;

namespace Game.UI
{
	public class FloatingTextUI : MonoBehaviour
	{
		private const string NUMBER_SIGN_FORMAT = "+#;-#;0";
		private const float DEFAULT_TIME = .75f;

		[SerializeField] private TMP_Text text;
		[SerializeField] private Color red = Color.red;
		[SerializeField] private Color green = Color.green;
		private Timer timer;
		private Action<GameObject> onRelease;

		public void Init(Action<GameObject> onRelease)
		{
			this.onRelease = onRelease;
		}

		public void ShowRedGreen(int value, Vector3 position, bool invert = false, float time = DEFAULT_TIME)
		{
			var color = (value > 0 && !invert) ? green : red;
			Show(value, position, color, time);
		}

		public void Show(string value, Vector3 position, Color color = default, float time = DEFAULT_TIME)
		{
			if (color == default) color = Color.white;
			transform.position = position;
			text.text = value;
			Show(color, time);
		}

		public void Show(int value, Vector3 position, Color color = default, float time = DEFAULT_TIME)
		{
			text.text = value.ToString();
			transform.position = position;
			Show(color, time);
		}

		private void Show(Color color, float time)
		{
			if (color == default) color = Color.white;
			text.color = color;
			gameObject.SetActive(true);
			timer.Restart(time);
		}

		private void Update()
		{
			if (timer.IsReady())
			{
				onRelease?.Invoke(gameObject);
				gameObject.SetActive(false);
			}
		}
	}
}