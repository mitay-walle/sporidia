using GameJam.Plugins.Combat.Damage;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace GameJam.Plugins.Combat.UI
{
	[RequireComponent(typeof(Slider))]
	public class Healthbar : MonoBehaviour
	{
		[ShowInInspector, ReadOnly] private Damageable _damageable;

		public void Init(Damageable damageable)
		{
			_damageable = damageable;
			damageable.OnDamage += OnDamage;
			OnDamage(default);
		}

		private void OnDamage(DamageInfo info)
		{
			GetComponent<Slider>().maxValue = _damageable.Max;
			GetComponent<Slider>().value = _damageable.Health;
		}
	}
}