using UnityEngine;

namespace UI.Utility.Lerpers
{
    public abstract class LerperBehaviour<T> : LerperComponent<T> where T : Behaviour
    {
        protected override void OnLerp(float t)
        {
            if (_disableComponent)
            {
                _target.enabled = t != 0;
            }
        }
    }
}
