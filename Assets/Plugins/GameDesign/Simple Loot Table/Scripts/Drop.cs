using System.Collections.Generic;
using Kellojo.SimpleLootTable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Enemies
{
	/// <summary>
	/// Drop items on death
	/// </summary>
	public class Drop : MonoBehaviour
	{
		[SerializeField] private GameObjectLootTable _loot;
		[SerializeField] private float _dropRadius = .5f;

		private void Start()
		{
			// if (TryGetComponent<Damageable>(out var damageable))
			// {
				// damageable.OnDeath += DropAll;
			// }
			// else
			{
				DropAll();
			}
		}

		//private void DropAll(DamageInfo damageInfo)=>DropAll();
		
		private void DropAll()
		{
			if (!_loot)
			{
				Debug.LogError("no loot", this);
				return;
			}
			List<GameObject> prefabs = _loot.GetGuaranteedDrops();
			foreach (GameObject prefab in prefabs)
			{
				Instantiate(prefab, transform.position + (Vector3)Random.insideUnitCircle * _dropRadius, Quaternion.identity);
			}

			prefabs = _loot.GetOptionalDrop();
			foreach (GameObject prefab in prefabs)
			{
				Instantiate(prefab, transform.position + (Vector3)Random.insideUnitCircle * _dropRadius, Quaternion.identity);
			}
		}
	}
}