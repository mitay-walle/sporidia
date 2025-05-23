using UnityEngine;

namespace GameJam.Plugins.Audio.Music
{
	public class MusicComponent : MonoBehaviour
	{
		[field: SerializeField] public eMusic Music { get;
			set; }

		private void OnEnable()
		{
			FindObjectOfType<Plugins.Music>()?.Add(this);
		}

		private void OnDisable()
		{
			FindObjectOfType<Plugins.Music>()?.Remove(this);
		}
	}
} 