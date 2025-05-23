using GameJam.Plugins.Combat.Damage;
using GameJam.Plugins.UI.Legacy.ACV.Runtime;
using UltEvents;
using UnityEngine;

namespace GameJam.Plugins.Combat.UI
{
	[RequireComponent(typeof(CanvasGroup))]
	public class DamageFrame : MonoBehaviour
	{
		[SerializeField] private int _lowHPShow = 25;
		[SerializeField] private float _speed = 1;
		[SerializeField] private float _time = 2;
		private CanvasGroup _canvasGroup;
		private Damageable _player;
		private bool _showAlways;
		private TimeTrigger _timer;

		public void Init(Damageable damageable)
		{
			_canvasGroup = GetComponent<CanvasGroup>();
			_player = damageable;
			_player.OnDamage.AddListener(OnDamage);
			_canvasGroup.alpha = 0;
		}

		private void Update()
		{
			_showAlways = _player.Health < _lowHPShow;
			if (_showAlways)
			{
				_canvasGroup.alpha = 1;
			}
			else
			{
				if (_timer.IsReady())
				{
					_canvasGroup.alpha -= _speed * Time.deltaTime;
				}
			}
		}

		private void OnDamage(DamageInfo info)
		{
			_timer.Restart(_time);
			_canvasGroup.alpha = 1;
		}
	}
}