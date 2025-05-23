using UnityEngine;
using UnityEngine.Playables;

namespace GameJam.Plugins.UI.Effects.TimelineCinematicFrame.Runtime
{
	[System.Serializable]
	public class CinematicFrameBehaviour : PlayableBehaviour
	{
		public TimelineCinematicFrame TimelineCinematicFrame = null;
		[Range(0,1)]public float Value;

		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			TimelineCinematicFrame = playerData as TimelineCinematicFrame;
			if (TimelineCinematicFrame != null)
			{
				TimelineCinematicFrame.SetValue(Value * info.effectiveWeight);
			}
		}
	}
}