using UnityEngine;

namespace GameJam.Plugins.GameLoop.UI
{
	public class Pause : MonoBehaviour
	{
		private void Awake()
		{
			GameObject go = transform.GetChild(0).gameObject;
			go.SetActive(false);
			Time.timeScale = 1;
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.F2))
			{
				GameObject go = transform.GetChild(0).gameObject;
				go.SetActive(!go.activeSelf);
				Time.timeScale = go.activeSelf ? 0 : 1;
				if (go.activeSelf)
				{
					Refill();
				}
			}
		}

		private void Refill()
		{
			Debug.Log("fill pause screen info");
		}
	}
}