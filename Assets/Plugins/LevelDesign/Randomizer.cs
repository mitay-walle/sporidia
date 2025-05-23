using System;
using System.Collections.Generic;
using TriInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

namespace Plugins.LevelDesign
{
	public interface IRandomizer
	{
		void Randomize(Randomizer randomizer);
	}

	[Serializable]
	public class ScaleRandomizer : IRandomizer
	{
		[SerializeField] private MinMaxCurve _x = Randomizer.MinMaxCurveDefault_minus1_to_1();
		[SerializeField] private bool _ySameX = true;
		[SerializeField, HideIf("_ySameX")] private MinMaxCurve _y = Randomizer.MinMaxCurveDefault_minus1_to_1();
		[SerializeField] private bool _zSameX = true;
		[SerializeField, HideIf("_zSameX")] private MinMaxCurve _z = Randomizer.MinMaxCurveDefault_minus1_to_1();

		public void Randomize(Randomizer randomizer)
		{
			float x = _x.Evaluate(Random.value, Random.value);
			randomizer.transform.localScale = new Vector3(x, _ySameX ? x : _y.Evaluate(Random.value, Random.value), _zSameX ? x : _z.Evaluate(Random.value, Random.value));
		}
	}

	[Serializable]
	public class RotationRandomizer : IRandomizer
	{
		[SerializeField] private MinMaxCurve _x;
		[SerializeField] private bool _ySameX = false;
		[SerializeField, HideIf("_ySameX")] private MinMaxCurve _y;
		[SerializeField] private bool _zSameX = false;
		[SerializeField, HideIf("_zSameX")] private MinMaxCurve _z = Randomizer.MinMaxCurveDefault_0_to_360();

		public void Randomize(Randomizer randomizer)
		{
			float value = Random.value;
			float x = _x.Evaluate(Random.value, value);
			randomizer.transform.localEulerAngles = new Vector3(x, _ySameX ? x : _y.Evaluate(Random.value, Random.value), _zSameX ? x : _z.Evaluate(Random.value, Random.value));
		}
	}

	[Serializable]
	public class SpriteRandomizer : IRandomizer
	{
		[SerializeField, Required] private SpriteRenderer _renderer;
		[SerializeField, Required] private bool _resetCollider = true;
		[SerializeField, Required] private Sprite[] _sprites;
		private List<Vector2> _buffer = new();

		public void Randomize(Randomizer randomizer)
		{
			if (_renderer)
			{
				_renderer.sprite = _sprites[Mathf.RoundToInt((_sprites.Length - 1) * Random.value)];
				if (_resetCollider && _renderer.GetComponent<PolygonCollider2D>() && _renderer.sprite.GetPhysicsShapeCount() > 0)
				{
					_renderer.sprite.GetPhysicsShape(0, _buffer);
					_renderer.GetComponent<PolygonCollider2D>().SetPath(0, _buffer);
				}
			}
		}
	}

	[Serializable]
	public class SpriteFlipRandomizer : IRandomizer
	{
		[SerializeField, Required] private SpriteRenderer _renderer;
		[SerializeField] private bool _x = true;
		[SerializeField] private bool _y;

		public void Randomize(Randomizer randomizer)
		{
			if (_renderer)
			{
				if (_x) _renderer.flipX = Random.value > .5f;
				if (_y) _renderer.flipY = Random.value > .5f;
			}
		}
	}

	[Serializable]
	public class SpriteColorRandomizer : IRandomizer
	{
		[SerializeField, Required] private SpriteRenderer _renderer;
		[SerializeField] private MinMaxGradient _color;

		public void Randomize(Randomizer randomizer)
		{
			if (_renderer)
			{
				_renderer.color = _color.Evaluate(Random.value, Random.value);
			}
		}
	}

	[ExecuteAlways]
	public class Randomizer : MonoBehaviour
	{
		[SerializeField] private bool _onEnable;
		[SerializeField] private bool _onEnableEdit = true;
		[SerializeReference] private IRandomizer[] _randomizers =
		{
			new ScaleRandomizer(),
			new RotationRandomizer(),
		};

		public UnityEvent OnRandomize;

		private void OnEnable()
		{
			if (_onEnableEdit && !Application.isPlaying) Randomize();
			if (_onEnable && Application.isPlaying) Randomize();
		}

		[Button]
		public void Randomize()
		{
#if UNITY_EDITOR
			Undo.RecordObject(transform, "randomize");
			Undo.RecordObject(this, "randomize");
#endif

			foreach (IRandomizer randomizer in _randomizers)
			{
				randomizer.Randomize(this);
			}
			OnRandomize?.Invoke();
		}

		public static MinMaxCurve MinMaxCurveDefault_01_to_1() => new MinMaxCurve() { constantMin = .1f, constantMax = 1, mode = ParticleSystemCurveMode.TwoConstants };
		public static MinMaxCurve MinMaxCurveDefault_minus1_to_1() => new MinMaxCurve() { constantMin = -1, constantMax = 1, mode = ParticleSystemCurveMode.TwoConstants };
		public static MinMaxCurve MinMaxCurveDefault_0_to_360() => new MinMaxCurve() { constantMin = 0, constantMax = 360, mode = ParticleSystemCurveMode.TwoConstants };
	}
}