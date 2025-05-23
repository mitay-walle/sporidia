using UnityEngine;
using UnityEngine.UI;

namespace GameJam.Plugins.UI.Legacy.Animated
{
    public sealed class AnimatedRawImage : AnimatedGraphicGeneric<RawImage>
    {
        protected override void FillValue(float value)
        {
            var oldColor = Target.color;
            Target.color = new Color(oldColor.r,oldColor.g,oldColor.b,value);
        }
    }
}