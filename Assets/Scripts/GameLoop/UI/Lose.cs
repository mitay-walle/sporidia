using UnityEngine;
using UnityEngine.UI;

namespace GameJam.Plugins.GameLoop.UI
{
	public class Lose : MonoBehaviour
	{
		private void Awake()
		{
			FindAnyObjectByType<GameplayEvents>().OnLose += OnLose;
			gameObject.SetActive(false);
			GetComponentInChildren<Button>().onClick.AddListener(OnClick);
		}

		private void OnLose() => gameObject.SetActive(true);

		private void OnClick()
		{
			FindAnyObjectByType<GameplayEvents>().Restart();
		}
	}
}