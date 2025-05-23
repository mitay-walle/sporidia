using TriInspector;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using TriInspector.Editors;
using UnityEditor.IMGUI.Controls;
#endif

namespace Plugins.LevelDesign
{
    #if UNITY_EDITOR
	[CustomEditor(typeof(BoundsRandomSpawner)), CanEditMultipleObjects]
	public class BoundsRandomSpawnerEditor : TriEditor
	{
		private BoxBoundsHandle _handle;

		private void OnSceneGUI()
		{
			Handles.matrix = (target as BoundsRandomSpawner).transform.localToWorldMatrix;
			_handle ??= new BoxBoundsHandle();

			_handle.size = (target as BoundsRandomSpawner).Bounds.size;
			_handle.center = (target as BoundsRandomSpawner).Bounds.center;
			EditorGUI.BeginChangeCheck();
			_handle.DrawHandle();
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(target, "scene gui");
				(target as BoundsRandomSpawner).Bounds = new Bounds(_handle.center, _handle.size);
			}
		}
	}
  #endif

	public class BoundsRandomSpawner : MonoBehaviour
	{
		[SerializeField] private bool _onEnable = true;
		[SerializeField] private bool _onEnableEdit;

		public Bounds Bounds = new Bounds(default, Vector3.one);
		public GameObject[] Prefabs;
		public ParticleSystem.MinMaxCurve Count = 10;
		public bool CountRespectsBoundsSize = true;
		public bool Randomize = true;

		private void OnEnable()
		{
			if (_onEnableEdit && !Application.isPlaying) Respawn();
			if (_onEnable && Application.isPlaying) Respawn();
		}

		[Button]
		private void Respawn()
		{
			Clear();
			Spawn();
		}

		public void Spawn()
		{
			float countInitial = Count.Evaluate(0, Random.value);
			if (CountRespectsBoundsSize)
			{
				countInitial *= (Bounds.extents.x + Bounds.extents.y + Bounds.extents.z) / 3;
			}

			int count = Mathf.RoundToInt(countInitial);

			for (int i = 0; i < count; i++)
			{
				var prefab = Prefabs[Mathf.RoundToInt((Prefabs.Length - 1) * Random.value)];
				GameObject go;
#if UNITY_EDITOR
				if (!Application.isPlaying)
				{
					go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, transform);
				}
				else
#endif
				{
					go = Instantiate(prefab, transform);
				}

				Vector3 m = Bounds.min;
				Vector3 M = Bounds.max;
				go.transform.localPosition = new Vector3(Random.Range(m.x, M.x), Random.Range(m.y, M.y), Random.Range(m.z, M.z));

				if (Randomize && go.GetComponent<Randomizer>())
				{
					go.GetComponent<Randomizer>().Randomize();
				}
			}
		}
		[Button]
		private void Clear()
		{
			int count = transform.childCount;
			for (int i = count - 1; i >= 0; i--)
			{
				GameObject go = transform.GetChild(0).gameObject;
				go.transform.SetParent(null);
				DestroyImmediate(go);
			}

			transform.DetachChildren();
		}
	}
}