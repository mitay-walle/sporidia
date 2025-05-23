using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameJam.Plugins.GameLoop.UI
{
	public class WinFinal : MonoBehaviour
	{
		private void Awake()
		{
			FindAnyObjectByType<GameplayEvents>().OnWin += Callback;
			gameObject.SetActive(false);
		}

		private void Callback()
		{
			if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
			{
				gameObject.SetActive(true);
			}
		}

		private void OnClick()
		{
			FindAnyObjectByType<GameplayEvents>().Restart();
		}
	}
}