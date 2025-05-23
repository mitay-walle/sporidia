using UnityEngine.Timeline;

namespace GameJam.Plugins.UI.Effects.TimelineCinematicFrame.Runtime
{
	[TrackClipType(typeof(CinematicFrameAsset))]
	[TrackBindingType(typeof(TimelineCinematicFrame))]
	public class CinematicFrameTrack : TrackAsset { }
}