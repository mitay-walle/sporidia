using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.UI.Primitives
{
	[ExecuteAlways]
	public class UITrailRenderer : MaskableGraphic
	{
		[SerializeField] private TrailRenderer _trail;
		[SerializeField] Texture m_Texture;
		private List<UIVertex> _uiVertices = new();
		private List<Vector3> _vertices = new();
		private List<Color32> _colors = new();
		private List<Vector4> _uvs = new();
		private List<int> _indices = new();

		private Camera _bakeCamera;
		private Mesh _mesh;

		/// Returns the texture used to draw this Graphic.
		public override Texture mainTexture
		{
			get
			{
				if (m_Texture == null)
				{
					if (material != null && material.mainTexture != null)
					{
						return material.mainTexture;
					}
					return s_WhiteTexture;
				}

				return m_Texture;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			_mesh = _mesh == null ? new Mesh() : _mesh;
			_mesh.name = $"{nameof(UITrailRenderer)}_{GetInstanceID()}";
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			DestroyImmediate(_mesh);
			_mesh = null;
		}

		private void Update()
		{
			if (!_mesh || !_trail) return;
			Camera camera = GetBakeCamera();
			if (!camera) return;
			camera.transform.SetPositionAndRotation(new Vector3(0, 0, -1000), Quaternion.identity);

			_trail.BakeMesh(_mesh, camera);
			_mesh.GetVertices(_vertices);
			_mesh.GetColors(_colors);
			_mesh.GetUVs(0, _uvs);
			_mesh.GetIndices(_indices, 0);
			SetVerticesDirty();
		}

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			_uiVertices.Clear();

			for (int i = 0; i < _vertices.Count; i++)
			{
				_uiVertices.Add(new UIVertex()
				{
					position = _vertices[i],
					uv0 = _uvs[i],
					color = _colors[i],
				});
			}

			vh.AddUIVertexStream(_uiVertices, _indices);
		}
		// protected override void OnPopulateMesh(Mesh m)
		// {
		// 	if (!_mesh) return;
		// 	m.SetVertices(_mesh.vertices);
		// 	m.SetColors(_mesh.colors);
		// 	m.SetUVs(0, _mesh.uv);
		// 	m.SetIndices(_mesh.GetIndices(0), MeshTopology.Triangles, 0);
		// }

		private Camera GetBakeCamera()
		{
			if (_bakeCamera)
			{
				_bakeCamera.orthographicSize = 1;
				return _bakeCamera;
			}

			// Find existing baking camera.
			var childCount = transform.childCount;
			for (var i = 0; i < childCount; i++)
			{
				if (transform.GetChild(i).TryGetComponent<Camera>(out var cam)
					&& cam.name == "[generated] UITrailRenderer BakingCamera")
				{
					_bakeCamera = cam;
					break;
				}
			}

			// Create baking camera.
			if (!_bakeCamera)
			{
				var go = new GameObject("[generated] UITrailRenderer BakingCamera");
				go.SetActive(false);
				go.transform.SetParent(transform, false);
				_bakeCamera = go.AddComponent<Camera>();
			}

			// Setup baking camera.
			_bakeCamera.enabled = false;
			_bakeCamera.orthographicSize = 1;
			_bakeCamera.transform.SetPositionAndRotation(new Vector3(0, 0, -1000), Quaternion.identity);
			_bakeCamera.orthographic = true;
			_bakeCamera.farClipPlane = 2000f;
			_bakeCamera.clearFlags = CameraClearFlags.Nothing;
			_bakeCamera.cullingMask = 0;// Nothing
			_bakeCamera.allowHDR = false;
			_bakeCamera.allowMSAA = false;
			_bakeCamera.renderingPath = RenderingPath.Forward;
			_bakeCamera.useOcclusionCulling = false;

			_bakeCamera.gameObject.SetActive(false);
			_bakeCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;

			return _bakeCamera;
		}
	}
}