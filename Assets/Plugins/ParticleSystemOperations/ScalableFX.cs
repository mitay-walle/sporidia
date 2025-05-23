using System;
using JetBrains.Annotations;
using TriInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Plugins.ParticleSystemOperations
{
	public class ScalableFX : MonoBehaviour
	{
		[ShowInInspector, NonSerialized, Range(0f, 1000f), OnValueChanged("FillTest")] private float testSpeed = .5f;
		[SerializeField] private float maxSpeed = 50f;
		[SerializeField] private Option[] Options;

		[Flags]
		public enum AnimationType
		{
			Speed = 1,
			Emission = 2,
			Color = 4,
			Scale = 8,
			StartSpeed = 16,
			EmissionBurst = 32,
		}

		[Serializable]
		public struct Option
		{
			public AnimationType type;

			public ParticleSystem PS;
			[ShowIf("@type.HasFlag(AnimationType.Speed)")] public ParticleSystem.MinMaxCurve Speed;
			[ShowIf("@type.HasFlag(AnimationType.StartSpeed)")] public ParticleSystem.MinMaxCurve StartSpeed;
			[ShowIf("@type.HasFlag(AnimationType.Emission)")] public ParticleSystem.MinMaxCurve Emission;
			[ShowIf("@type.HasFlag(AnimationType.Color)")] public ParticleSystem.MinMaxGradient Color;
			[ShowIf("@type.HasFlag(AnimationType.Scale)")] public ParticleSystem.MinMaxCurve Scale;

			public Option(ParticleSystem ps)
			{
				type = default;
				PS = ps;
				Speed = default;
				StartSpeed = default;
				Emission = default;
				Color = default;
				Scale = default;
			}
		}

		[UsedImplicitly]
		public void ProcessNormalized(float factor) => Process(factor * maxSpeed);

		public void Process(float speed)
		{
			speed /= maxSpeed;

			for (int i = 0; i < Options.Length; i++)
			{
				Fill(ref Options[i], speed);
			}
		}

		private void Fill(ref Option option, float speed)
		{
			float random = Random.value;
			if (option.type.HasFlag(AnimationType.Speed))
			{
				var velOver = option.PS.velocityOverLifetime;
				var curve = velOver.speedModifier;
				curve.constant = option.Speed.Evaluate(speed, random);
				velOver.speedModifier = curve;
			}

			if (option.type.HasFlag(AnimationType.StartSpeed))
			{
				var velOver = option.PS.main;
				var curve = velOver.startSpeed;
				curve.constant = option.StartSpeed.Evaluate(speed, random);
				velOver.startSpeed = curve;
			}

			if (option.type.HasFlag(AnimationType.EmissionBurst))
			{
				var em = option.PS.emission;
				var burst = em.GetBurst(0);
				var count = burst.count;
				count.curveMultiplier = option.Emission.Evaluate(speed, random);
				burst.count = count;
				em.SetBurst(0, burst);
			}

			if (option.type.HasFlag(AnimationType.Emission))
			{
				var em = option.PS.emission;
				var count = em.rateOverTime;
				count.curveMultiplier = option.Emission.Evaluate(speed, random);
				em.rateOverTime = count;
			}

			var main = option.PS.main;

			if (option.type.HasFlag(AnimationType.Color))
			{
				var start = main.startColor;
				start.colorMax = option.Color.Evaluate(speed, random);
				main.startColor = start;
			}

			if (option.type.HasFlag(AnimationType.Scale))
			{
				var start = main.startSize;
				start.constant = option.Scale.Evaluate(speed, random);
				main.startSize = start;
			}
		}

    #region Editor
#if UNITY_EDITOR

		[Button]
		void FillTest()
		{
			Process(testSpeed);
			for (int i = 0; i < Options.Length; i++) EditorUtility.SetDirty(Options[i].PS);
			for (int i = 0; i < Options.Length; i++) Options[i].PS.Play();
		}

		[Button]
		void Reset()
		{
			Undo.RecordObject(this, "Reset");

			if (Options == null || Options.Length == 0)
			{
				var pses = GetComponentsInChildren<ParticleSystem>(true);

				if (Options == null) Options = new Option[0];

				for (int i = 0; i < pses.Length; i++)
					ArrayUtility.Add(ref Options, new Option(pses[i]));
			}

			EditorUtility.SetDirty(this);
		}

#endif
    #endregion
	}
}