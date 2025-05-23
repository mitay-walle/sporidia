using System;
using UnityEngine;

namespace UI.Utility.Lerpers
{
    [Serializable]
    public class LerperCompositionColor : LerperCompositionBase
    {
        public Color min = Color.black;
        public Color max = Color.white;
        private Color _current;
        
        public override void Lerp(float t)
        {
            if (!IsUsed) return;
            _current = Color.Lerp(min,max,t);
        }

        public Color GetCurrent() => _current;
    }
}
