using System;
using System.Collections.Generic;
using TriInspector;
using UltEvents;
using UnityEngine;

namespace GameJam.Plugins.Design.Stats
{
	[Serializable]
	public class Stat
	{
		[field: SerializeField] public float BaseValue { get; private set; }
		[ShowInInspector, ReadOnly] public float Value { get; private set; }

		public Action<StatInfo> OnChanged;
		public UltEvent OnIncrease;
		public UltEvent OnDecrease;

		private List<StatModifier> statModifiers = new();

		public Stat() { Calculate(); }

		public Stat(float baseValue = default)
		{
			BaseValue = baseValue;
			statModifiers = new List<StatModifier>();
			Calculate();
		}

		public void Init() => Calculate();

		public void AddBase(float delta)
		{
			BaseValue += delta;
			Calculate();
		}

		public void Add(StatModifier mod)
		{
			statModifiers.Add(mod);
			statModifiers.Sort(Sorter);
			Calculate();
		}

		public void Remove(StatModifier mod)
		{
			statModifiers.Remove(mod);
			Calculate();
		}

		private void Calculate()
		{
			float before = Value;
			float finalValue = BaseValue;

			for (int i = 0; i < statModifiers.Count; i++)
			{
				StatModifier mod = statModifiers[i];

				if (mod.Type == StatModType.Flat)
				{
					finalValue += mod.Value;
				}
				else if (mod.Type == StatModType.Percent)
				{
					finalValue *= 1 + mod.Value;
				}
			}

			float after = (float)Math.Round(finalValue, 4);
			Value = after;

			if (Math.Abs(before - after) > .0001f)
			{
				if (after - before > 0)
				{
					OnIncrease?.Invoke();
				}
				else
				{
					OnDecrease?.Invoke();
				}
				StatInfo info = new()
				{
					Before = before,
					After = after,
					Delta = after - before,
				};
				OnChangedValue(info);
				OnChanged?.Invoke(info);
			}
		}

		protected virtual void OnChangedValue(StatInfo info)
		{
			// custom code
		}

		public bool Contains(StatModifier mod)
		{
			return statModifiers.Contains(mod);
		}

		private int Sorter(StatModifier a, StatModifier b) => a.Order.CompareTo(b.Order);
	}
}