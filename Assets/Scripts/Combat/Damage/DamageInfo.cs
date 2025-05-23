using System;
using TriInspector;

namespace GameJam.Plugins.Combat.Damage
{
	[Serializable]
	public struct DamageInfo
	{
		public int Damage;
		public int Before;
		[ShowInInspector] public IHasFaction Source;
		public Damageable Target;
	}
}