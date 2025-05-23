using System;
using GameJam.Plugins.Combat.Damage;
using UltEvents;
using UnityEditor;
using UnityEngine;

namespace GameJam.Plugins.Combat
{
	public class Trap : MonoBehaviour//, IHasFaction
	{
		[field: SerializeField] public Faction Faction { get; set; }
		[SerializeReference] private TrapEffect[] _effects;
		[SerializeField] private bool _disablebleByEsquirePush = true;
		[SerializeField] private bool _destroyOnEffect = true;
		[SerializeField] private float _destroyTime = 3;
		[SerializeField] private UltEvent OnTriggerAny;

		private void OnTriggerEnter(Collider col)
		{
			if (IsPlayer(col))
			{
				OnTriggerAny?.Invoke();
				foreach (TrapEffect effect in _effects)
				{
					effect.Apply(this, col.attachedRigidbody.gameObject);
				}
				if (_destroyOnEffect)
				{
					Destroy(gameObject, _destroyTime);
				}
			}
		}

		private bool IsPlayer(Collider collider) => true;

#if UNITY_EDITOR
		//[OnInspectorGUI]
		private void OnInspectorGUI()
		{
			if (TryGetComponent<Collider>(out var collider2D))
			{
				if (!collider2D.isTrigger)
				{
					EditorGUILayout.HelpBox("Collider2D.isTrigger = true", MessageType.Error);
				}
			}
			else
			{
				EditorGUILayout.HelpBox("need Collider2D", MessageType.Error);
			}
		}
#endif
	}

	[Serializable]
	public abstract class TrapEffect
	{
		public abstract void Apply(Trap trap, GameObject target);
	}

	// [Serializable]
	// public class Damage : TrapEffect
	// {
	// 	[SerializeField] private int damage = 1;
	//
	// 	public override void Apply(Trap trap, GameObject target)
	// 	{
	// 		if (target.TryGetComponent<Damageable>(out var damageable))
	// 		{
	// 			damageable.Recieve(trap, damage);
	// 		}
	// 	}
	// }
}