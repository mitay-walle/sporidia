using UnityEngine;

namespace UI.Utility.Lerpers
{
    public class LerperCanvasGroup : LerperBehaviour<CanvasGroup>
    {
        [SerializeField] private LerperComposition _alpha;
        [SerializeField] private bool _changeInteractable;
        
        protected override void OnLerp(float t)
        {
            if (_alpha.IsUsed)
            {
                _alpha.Lerp(t);
                _target.alpha = _alpha.GetCurrent();    
            }

            if (_changeInteractable)
            {
                _target.blocksRaycasts = _target.interactable = GetCurrentTargetT() > 0;
            }
            base.OnLerp(t);
        }
    }
}
