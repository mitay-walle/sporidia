using System;
using GameJam.Plugins.Combat.Damage;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameJam.Plugins.GameLoop
{
	public class GameplayEvents : MonoBehaviour
	{
		public Action<DamageInfo> OnDamageAny;
		public Action<DamageInfo> OnDeathAny;
		public Action OnLose;
		public Action OnWin;

		public void Win()
		{
			Debug.Log("win");
			OnWin?.Invoke();
		}

		private void Lose()
		{
			Debug.Log("lose");
			OnLose?.Invoke();
		}

		public void Restart()
		{
			Debug.Log("restart");
			string currentSceneName = SceneManager.GetActiveScene().name;
			SceneManager.LoadScene(currentSceneName);
		}

		public void OnDeathAnyInvoke(DamageInfo info)
		{
			OnDeathAny?.Invoke(info);
			if (info.Target.GetComponent<IPlayerGameLoop>() != null)
			{
				OnPlayerDeath(info);
			}
		}

		public void OnDamageAnyInvoke(DamageInfo info)
		{
			OnDamageAny?.Invoke(info);
		}
		
		private void OnPlayerDeath(DamageInfo info)
		{
			Lose();
		}

		public void PlayNext()
		{
			int index = SceneManager.GetActiveScene().buildIndex + 1;
			Debug.Log($"play next {index}");
			SceneManager.LoadScene(index);
		}
	}
}