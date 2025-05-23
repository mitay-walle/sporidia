using System.Collections.Generic;
using UnityEngine;

namespace Kellojo.SimpleLootTable
{
	[Icon("Assets/Gizmos/LootTable.tif")]
	[CreateAssetMenu(menuName = "Kellojo/Loot Table/Game Object Loot Table")]
	public class GameObjectLootTable : LootTableBase<GameObject>
	{
		//[Button]
		private void DropAtOrigin()
		{
			List<GameObject> prefabs = GetGuaranteedDrops();
			foreach (GameObject prefab in prefabs)
			{
				Instantiate(prefab, Random.insideUnitCircle, Quaternion.identity);
			}

			prefabs = GetOptionalDrop();
			foreach (GameObject prefab in prefabs)
			{
				Instantiate(prefab, Random.insideUnitCircle, Quaternion.identity);
			}
		}
	}

	[System.Serializable]
	public class GameObjectDropConfig : DropConfig<GameObject> { }
}