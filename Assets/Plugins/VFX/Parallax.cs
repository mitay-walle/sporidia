using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Environment
{
	public class Parallax : MonoBehaviour
	{
		[SerializeField] public float ParallaxX;
		[SerializeField] public float ParallaxY;

		private Vector3 _initialPosition;

		void Start()
		{
			_initialPosition = transform.position;
			RenderPipelineManager.beginCameraRendering -= OnRender;
			RenderPipelineManager.beginCameraRendering += OnRender;
		}

		private void OnRender(ScriptableRenderContext context, Camera cam)
		{
			Vector3 parallax = cam.transform.position;
			parallax.x *= ParallaxX;
			parallax.y *= ParallaxY;
			parallax.z = 0;
			transform.position = _initialPosition + parallax;
		}

		private void OnDestroy()
		{
			RenderPipelineManager.beginCameraRendering -= OnRender;
		}
	}
}