using UnityEngine;

namespace GameJam.Plugins.UI.Legacy.ACV.Runtime
{
    public abstract class ACV_Generic<T> : ACV_Base
    {
        [SerializeField,Header("Targets")] protected T _target;
        public T GetTarget() => _target;
    }
}
