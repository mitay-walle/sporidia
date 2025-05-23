using UnityEngine;
using UnityEngine.UI;

namespace UI.Utility.Lerpers
{
    public class LerperGraphic : LerperBehaviour<Graphic>
    {
        [SerializeField] LerperCompositionColor _color;
        [SerializeField] LerperComposition _alpha;

        protected override void OnLerp(float t)
        {
            Color color = _target.color;
            
            if (_color.IsUsed)
            {
                _color.Lerp(t);
                color = _color.GetCurrent();
            }

            if (_alpha.IsUsed)
            {
                _alpha.Lerp(t);
                color.a = _alpha.GetCurrent();
            }

            if (_color.IsUsed || _alpha.IsUsed)
            {
                _target.color = color;
            }
            base.OnLerp(t);
        }
    }
}