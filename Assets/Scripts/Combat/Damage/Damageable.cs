using System;
using GameJam.Plugins.Disablers;
using GameJam.Plugins.GameLoop;
using GameJam.Upgrades;
using Sirenix.OdinInspector;
using UltEvents;
using UnityEngine;

namespace GameJam.Plugins.Combat.Damage
{
	public class Damageable : MonoBehaviour, IHasFaction
	{
		[SerializeField] private UpgradableParams _maxHealthOverride;
		[field: SerializeField] public Faction Faction { get; private set; }
		[field: SerializeField] public int Health { get; private set; } = 1;
		[field: SerializeField] public int Max { get; private set; } = 4;
		[field: SerializeField] public DisablerList IsImmortal { get; private set; } = new();
		[SerializeField] private float _deathDisableTime = 3;

		public bool IsAlive => Health > 0;
		
		public UltEvent<DamageInfo> OnDamage;
		public UltEvent<DamageInfo> OnDeath;

		private void Awake()
		{
			Health = GetMaxHealth();
		}

		public void Recieve(IHasFaction source, int healthDelta)
		{
			if(source != null && source.Faction == Faction && healthDelta > 0) return;
			if (Health <= 0) return;
			if (!IsImmortal.IsEnabled && healthDelta > 0) return;

			var info = new DamageInfo()
			{
				Damage = healthDelta,
				Target = this,
				Source = source,
				Before = Health,
			};

			Health = Mathf.Clamp(0, Health - healthDelta, GetMaxHealth());
			OnDamage?.Invoke(info);
			FindAnyObjectByType<GameplayEvents>().OnDamageAnyInvoke(info);
			if (Health <= 0)
			{
				OnDeathInvoked(info);
			}
		}

		private void OnDeathInvoked(DamageInfo info)
		{
			OnDeath?.Invoke(info);
			Invoke(nameof(DisableOnDeath), _deathDisableTime);
			FindAnyObjectByType<GameplayEvents>().OnDeathAnyInvoke(info);
		}

		private void DisableOnDeath() => gameObject.SetActive(false);

		private int GetMaxHealth()
		{
			if (_maxHealthOverride)
				return (int)_maxHealthOverride.Value;

			return Max;
		}
		
		[Button, HideInEditorMode] private void Kill() => Recieve(this, 100000);

		[Button, HideInEditorMode] private void Damage1() => Recieve(this, 1);

		[Button, HideInEditorMode] private void Heal1() => Recieve(this, -1);

		public void AddMax(int delta)
		{
			if (delta == 0) return;

			Max += delta;

			var info = new DamageInfo()
			{
				Damage = 0,
				Target = this,
				Source = this,
				Before = Health,
			};

			OnDamage?.Invoke(info);
		}
	}
}