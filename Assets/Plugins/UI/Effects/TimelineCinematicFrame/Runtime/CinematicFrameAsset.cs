using UnityEngine;
using UnityEngine.Playables;

namespace GameJam.Plugins.UI.Effects.TimelineCinematicFrame.Runtime
{
	public class CinematicFrameAsset : PlayableAsset
	{
		public ExposedReference<TimelineCinematicFrame> TimelineCinematicFrame;
		public float Value;

		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			var playable = ScriptPlayable<CinematicFrameBehaviour>.Create(graph);

			var cinematicFrameBehaviour = playable.GetBehaviour();
			cinematicFrameBehaviour.TimelineCinematicFrame = TimelineCinematicFrame.Resolve(graph.GetResolver());
			cinematicFrameBehaviour.Value = Value;

			return playable;
		}
	}
}