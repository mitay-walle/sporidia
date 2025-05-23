using UnityEngine;

namespace Game.Saving
{
	[CreateAssetMenu]
	public class SaveSettings : ScriptableObject
	{
		public string gamepadOverrides;
		public bool showDamageLabels;
		public bool showStatusLabels;
		public int TargetFPS;
	}
}