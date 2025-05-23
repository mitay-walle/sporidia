using System;
using UnityEngine;

namespace UI.Utility.Lerpers
{
    [Serializable]
    public class LerperComposition : LerperCompositionBase
    {
        public Vector2 MinMax = Vector2.up;
        public LerperCurve _curve = new LerperCurve();
        private float _current;

        public float GetCurrent() => _current;

        public override void Lerp(float t)
        {
            if (IsUsed)
            {
                _current = Mathf.LerpUnclamped(MinMax.x, MinMax.y, _curve.Evaluate(t));
                OnLerp?.Invoke(_current);
            }
        }
    }
}