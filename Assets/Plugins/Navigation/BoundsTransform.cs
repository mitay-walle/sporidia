using JetBrains.Annotations;
using TriInspector;
using UnityEngine;
using UnityEngine.AI;

namespace GameJam.Plugins.Navigation
{
	public class BoundsTransform : MonoBehaviour
	{
		[SerializeField] private LayerMask _ground = -1;
		[SerializeField] private Color _color = Color.white / 2;
		[SerializeField] private bool _debug = true;
		[SerializeField] private float _debugTime = 10;
		[SerializeField, OnValueChanged("VerticalLines")] private float _verticalDensity;
		[ShowInInspector] private bool _test;

		public Bounds WorldBounds => new(transform.position, transform.lossyScale);

		private Vector3 Scale => transform.lossyScale;
		private Vector3 HalfScale => transform.lossyScale / 2;
		private float Height => transform.lossyScale.y;

		[Button]
		public Vector3 RandomPointOnNavMesh()
		{
			NavMesh.SamplePosition(RandomPoint(), out var hit, Scale.magnitude, -1);
			if (_debug) Debug.DrawLine(hit.position, hit.position + Vector3.up / 2, Color.green, _debugTime);
			return hit.position;
		}

		public Vector3 RandomPointOnGround()
		{
			Vector3 origin = transform.TransformPoint(RandomUnitTopX());
			Physics.Raycast(origin, Vector2.down, out var hit, Height, _ground);

			if (hit.collider != null)
			{
				if (_debug)
				{
					Debug.DrawLine(origin, hit.point, Random.ColorHSV(), _debugTime);
				}

				return hit.point;
			}

			if (_debug)
			{
				Debug.DrawLine(origin, origin + Vector3.down * Height, Random.ColorHSV(), _debugTime);
			}

			return origin + Vector3.down * Height;
		}

		public Vector3 RandomPoint()
		{
			Vector3 vector = transform.TransformPoint(RandomUnit());
			if (_debug) Debug.DrawLine(transform.position, vector, Random.ColorHSV(), _debugTime);
			return vector;
		}

		private Vector3 RandomUnitTopX()
		{
			return new(Random.value - .5f, .5f, 0);
		}

		private Vector3 RandomUnit()
		{
			return new(Random.value - .5f, Random.value - .5f, Random.value - .5f);
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.color = _color;
			Gizmos.DrawWireCube(default, Vector3.one);
		}

		//[OnInspectorGUI]
		private void OnInspectorGUI()
		{
			if (!_test) return;

			RandomPoint();
			RandomPointOnGround();
		}

		[UsedImplicitly]
		private void VerticalLines()
		{
			if (_verticalDensity > 1)
			{
				Bounds b = WorldBounds;
				for (int i = 0; i < _verticalDensity; i++)
				{
					float y = b.min.y + b.size.y * i / _verticalDensity;
					Debug.DrawLine(new(b.min.x, y), new(b.max.x, y), Random.ColorHSV(), 1);
				}
			}
		}
	}
}