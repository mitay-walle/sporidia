using GameJam.Plugins.Disablers;
using UnityEngine;

namespace GameJam.Plugins.Audio.Music
{
	[RequireComponent(typeof(PlayRandomSound))]
	public class MusicLayer : DisablerListBehaviour
	{
		[SerializeField] private float _fade = 1;
		[SerializeField] private float _volume = 1;
		private PlayRandomSound _player;

		protected override void OnChanged()
		{
			_player ??= GetComponent<PlayRandomSound>();
			if (DisablerList.IsEnabled)
			{
				_player.Play();
				_player.FadeIn(_fade, _volume);
			}
			else
			{
				_player.FadeOut(true);
			}
			base.OnChanged();
		}
	}
}