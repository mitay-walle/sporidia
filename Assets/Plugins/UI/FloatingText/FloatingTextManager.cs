using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.UI
{
	[CreateAssetMenu]
	public class FloatingTextManager : ScriptableObject
	{
		[SerializeField] private FloatingTextUI prefab;
		private IObjectPool<GameObject> pool;
		private Action<GameObject> OnReleaseAction;

		public void Init(Transform root)
		{
			pool = new ObjectPool<GameObject>(Instantiate, null, null, Destroy);
			OnReleaseAction = OnRelease;
		}

		private GameObject Instantiate()
		{
			return Instantiate(prefab).gameObject;
		}

		public FloatingTextUI Get()
		{
			FloatingTextUI newPrefab = pool.Get().GetComponent<FloatingTextUI>();
			newPrefab.Init(OnReleaseAction);
			return newPrefab;
		}

		private void OnRelease(GameObject gameObject) => pool.Release(gameObject);
	}
}