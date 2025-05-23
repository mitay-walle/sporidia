using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Saving
{
	[CreateAssetMenu]
	public class SaveSettings : SerializedScriptableObject
	{
		public string gamepadOverrides;
		public bool showDamageLabels;
		public bool showStatusLabels;
		public int TargetFPS;
	}
}