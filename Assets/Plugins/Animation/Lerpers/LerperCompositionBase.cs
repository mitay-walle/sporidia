using System;

namespace UI.Utility.Lerpers
{
    [Serializable]
    public abstract class LerperCompositionBase
    {
        public bool IsUsed;
        public Action<float> OnLerp;
        
        public abstract void Lerp(float t);
    }
}