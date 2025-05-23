using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameJam.Plugins.GameLoop.UI
{
	public class WinNext : MonoBehaviour
	{
		private void Awake()
		{
			FindAnyObjectByType<GameplayEvents>().OnWin += Callback;
			gameObject.SetActive(false);
			GetComponentInChildren<Button>(true).onClick.AddListener(OnClick);
		}

		private void Callback()
		{
			gameObject.SetActive(true);
			//if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
			//{
			//	gameObject.SetActive(true);
			//}
		}

		private void OnClick()
		{
			FindAnyObjectByType<GameplayEvents>().PlayNext();
		}
	}
}