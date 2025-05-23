using System;
using Plugins;
using TriInspector;
using UnityEngine;

namespace UI.Utility.Lerpers
{
    [Serializable]
    public class LerperCurve
    {
        [SerializeField] private eCurvePattern _pattern;

        [ShowIf("@_pattern==eCurvePattern.Custom"), SerializeField]
        private AnimationCurve _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [SerializeField] private float _snap;


        public float Evaluate(float t)
        {
            float result = 0;
            switch (_pattern)
            {
                case eCurvePattern.Linear:
                    result = Mathf.Lerp(0, 1, t);
                    break;
                case eCurvePattern.Cubic:
                    result = t * t * (3 - 2 * t);
                    break;
                case eCurvePattern.Custom:
                    result = _curve.Evaluate(t);
                    break;
                case eCurvePattern.Back:
                    result = Easing.Back.InOut(t);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_snap > 0)
            {
                result = Snapping.Snap(result, _snap);
            }

            return result;
        }
    }
}