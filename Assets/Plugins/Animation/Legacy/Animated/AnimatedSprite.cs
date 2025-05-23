using UnityEngine;

namespace GameJam.Plugins.UI.Legacy.Animated
{
    public class AnimatedSprite : AnimatedGeneric<SpriteRenderer>
    {
        protected override void FillValue(float value)
        {
            var color = Target.color;
            color.a = value;
            Target.color = color;
        }
        protected override void SetColor(Color color)
        {
            Target.color = color;
        }
    }
}