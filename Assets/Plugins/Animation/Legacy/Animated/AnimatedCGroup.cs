using UnityEngine;

namespace GameJam.Plugins.UI.Legacy.Animated
{
    public class AnimatedCGroup : AnimatedGeneric<CanvasGroup>
    {
        protected override void FillValue(float value)
        {
            Target.alpha = value;
        }
    }
}