using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using TriInspector.Editors;
#endif

namespace GameJam.Plugins.QualityOfLife.SceneGUIBehaviour
{
	public abstract class OnSceneGUIBehaviour : MonoBehaviour
	{
		public abstract void OnSceneGUI();
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(OnSceneGUIBehaviour),true)]
	public class OnSceneGUIBehaviourEditor : TriEditor
	{
		private Object[] _cachedTargets;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			// can't use 'targets' in OnSceneGUI, use cache
			_cachedTargets = targets;
		}

		protected void OnSceneGUI()
		{
			foreach (OnSceneGUIBehaviour targ in _cachedTargets)
			{
				targ.OnSceneGUI();
			}
		}
	}
#endif

}