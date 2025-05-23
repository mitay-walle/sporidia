using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameJam.Plugins.Timing;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace GameJam.Plugins.VFX.RecolorSpriteRenderers
{
	[Serializable, InlineProperty]
	public struct PulseValues
	{
		[HideLabel] public Color color;
		[Range(0, 1)] public float lerp;
		public float duration;
		public int count;
	}

	[Serializable, InlineProperty]
	public struct RecolorValues
	{
		[HideLabel] public Color color;
		[HideLabel, Range(0, 1)] public float lerp;

		public RecolorValues(Color color, float lerp = 1) : this()
		{
			this.color = color;
			this.lerp = lerp;
		}
	}
	public class RecolorSprites : MonoBehaviour
	{
		private static int ID_VALUE = Shader.PropertyToID("_ReColorValue");
		private static int ID_COLOR = Shader.PropertyToID("_ReColorColor");

		[SerializeField] private PulseValues _damagePulse = new() { color = Color.white, lerp = 1, count = 1, duration = .2f };
		[SerializeField] private RecolorValues _deathValues = new() { color = new(1, 0, 0, .5f), lerp = .5f, };
		[SerializeField, Required] private List<Renderer> _targets = new();
		[ShowInInspector, ListDrawerSettings(HideAddButton = false, HideRemoveButton = true, AlwaysExpanded = true)] private List<RecolorValues> permanentColors = new();
		[ShowInInspector] private bool IsPulsing => pulse != null;
		private Coroutine pulse;
		private Timer _Timer;

		public bool HasPermanents() => permanentColors.Count > 0;

		public void Awake() => ResetPulseValues();

		[Button]
		public void PulseToDamage()
		{
			Pulse(_damagePulse);
		}

		[Button]
		public void AddDeathPermanent()
		{
			Pulse(_damagePulse);
		}

		[Button]
		public void AddPermanent(RecolorValues values)
		{
			permanentColors.Add(values);
			SetToLastPermanentOrDefault();
		}

		[Button]
		public void RemovePermanent(RecolorValues values)
		{
			if (permanentColors.Contains(values))
			{
				permanentColors.Remove(values);
			}

			SetToLastPermanentOrDefault();
		}

		[Button]
		public void Pulse(PulseValues values)
		{
			Pulse(values.color, values.count, values.duration, values.lerp);
		}

		public void Pulse(RecolorValues values)
		{
			Pulse(values.color, max: values.lerp);
		}

		public void Pulse(Color color, int count = 1, float periodSeconds = .25f, float max = 1)
		{
			if (periodSeconds == 0 || count == 0) return;

			if (pulse != null)
			{
				StopCoroutine(pulse);
			}

			if (gameObject.activeInHierarchy)
			{
				pulse = StartCoroutine(PulseCor(color, count, periodSeconds, max));
			}
		}

		protected IEnumerator PulseCor(Color color, int count, float duration, float lerp)
		{
			for (int i = 0; i < count; i++)
			{
				_Timer.Restart(duration);
				float t = 0;
				if (!HasPermanents())
				{
					SetColor(color);
				}

				while (!_Timer.IsReady())
				{
					float elapsed = _Timer.GetTimeElapsed();
					float normTime = Mathf.Clamp01(elapsed / duration);

					t = (Mathf.Cos(normTime * 2) + 1) / 2;

					if (HasPermanents())
					{
						var last = GetLastPermanentOrDefault();
						SetColor(Color.Lerp(last.color, color, t));
						SetLerp(Mathf.Lerp(last.lerp, lerp, t));
					}
					else
					{
						SetLerp(t * lerp);
					}

					yield return null;
				}
			}

			ResetPulseValues();
		}

		private void OnDisable()
		{
			ResetPulseValues();
		}

		public void ResetPulseValues()
		{
			pulse = null;
			SetToLastPermanentOrDefault();
		}

		private RecolorValues GetLastPermanentOrDefault()
		{
			if (HasPermanents()) return permanentColors[^1];

			return new(Color.white);
		}

		private void SetToLastPermanentOrDefault()
		{
			var values = new RecolorValues { color = Color.white, lerp = 0 };
			if (HasPermanents()) values = permanentColors[^1];

			SetColor(values.color);
			SetLerp(values.lerp);
		}

		[Button]
		private void SetLerp(float lerp)
		{
			_targets.ForEach(sprite => sprite.material.SetFloat(ID_VALUE, lerp));
		}

		[Button]
		private void SetColor(Color color)
		{
			_targets.ForEach(sprite => sprite.material.SetColor(ID_COLOR, color));
		}

		#region Editor
#if UNITY_EDITOR

		[Button]
		protected virtual void Reset()
		{
			Undo.RecordObject(this, "reset");

			if (_targets.Count == 0)
			{
				_targets = GetComponentsInChildren<SpriteRenderer>(true).OfType<Renderer>().ToList();
			}
		}
#endif
		#endregion
	}
}