using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Utility.Lerpers
{
    public class LerperRect : LerperComponent<RectTransform>
    {
        [SerializeField] private bool layotRebuild;
        [SerializeField] private LerperComposition _rect;
        [ShowIf("@_rect.IsUsed"), SerializeField] private RectTransform _from;
        [ShowIf("@_rect.IsUsed"), SerializeField] private RectTransform _to;
        
        [ShowIf("@_from == null || _to == null"), SerializeField]
        private LerperComposition sizeX;

        [ShowIf("@_from == null || _to == null"), SerializeField]
        private LerperComposition sizeY;

        [ShowIf("@_from == null || _to == null"), SerializeField]
        private LerperComposition posX;

        [ShowIf("@_from == null || _to == null"), SerializeField]
        private LerperComposition posY;

        [SerializeField] private LerperComposition _rotation;
        
        [SerializeField] private LerperComposition scaleX;
        [SerializeField] private LerperComposition scaleY;
        
        private void TryInit()
        {
            LerperComposition lerper = sizeX;
            if (lerper.OnLerp == null)
            {
                void Action(float result)
                {
                    Vector2 vector = _target.sizeDelta;
                    vector.x = result;
                    _target.sizeDelta = vector;
                }

                lerper.OnLerp = Action;
            }

            lerper = _rect;
            if (lerper.OnLerp == null)
            {
                void Action(float result)
                {
                    if (_from && _to)
                    {
                        _target.pivot = Vector2.LerpUnclamped(_from.pivot, _to.pivot, result);
                        _target.anchorMin = Vector2.LerpUnclamped(_from.anchorMin, _to.anchorMin, result);
                        _target.anchorMax = Vector2.LerpUnclamped(_from.anchorMax, _to.anchorMax, result);
                        _target.offsetMin = Vector2.LerpUnclamped(_from.offsetMin, _to.offsetMin, result);
                        _target.offsetMax = Vector2.LerpUnclamped(_from.offsetMax, _to.offsetMax, result);
                        _target.sizeDelta = Vector2.LerpUnclamped(_from.sizeDelta, _to.sizeDelta, result);
                        _target.anchoredPosition3D =
                            Vector3.LerpUnclamped(_from.anchoredPosition3D, _to.anchoredPosition3D, result);
                    }
                }

                lerper.OnLerp = Action;
            }

            lerper = sizeY;
            if (lerper.OnLerp == null)
            {
                void Action(float result)
                {
                    Vector2 vector = _target.sizeDelta;
                    vector.y = result;
                    _target.sizeDelta = vector;
                }

                lerper.OnLerp = Action;
            }

            lerper = posX;
            if (lerper.OnLerp == null)
            {
                void Action(float result)
                {
                    Vector2 vector = _target.anchoredPosition;
                    vector.x = result;
                    _target.anchoredPosition = vector;
                }

                lerper.OnLerp = Action;
            }

            lerper = posY;
            if (lerper.OnLerp == null)
            {
                void Action(float result)
                {
                    Vector2 vector = _target.anchoredPosition;
                    vector.y = result;
                    _target.anchoredPosition = vector;
                }

                lerper.OnLerp = Action;
            }
            
            lerper = _rotation;
            if (lerper.OnLerp == null)
            {
                void Action(float result)
                {
                    Vector3 vector = _target.localEulerAngles;
                    vector.z = result;
                    _target.localEulerAngles = vector;
                }

                lerper.OnLerp = Action;
            }
            
            lerper = scaleX;
            if (lerper.OnLerp == null)
            {
                void Action(float result)
                {
                    Vector3 vector = _target.localScale;
                    vector.x = result;
                    _target.localScale = vector;
                }

                lerper.OnLerp = Action;
            }
            
            lerper = scaleY;
            if (lerper.OnLerp == null)
            {
                void Action(float result)
                {
                    Vector3 vector = _target.localScale;
                    vector.y = result;
                    _target.localScale = vector;
                }

                lerper.OnLerp = Action;
            }
        }

        protected override void OnLerp(float t)
        {
            TryInit();

            _rect.Lerp(t);
            sizeX.Lerp(t);
            sizeY.Lerp(t);
            posX.Lerp(t);
            posY.Lerp(t);
            scaleX.Lerp(t);
            scaleY.Lerp(t);
            _rotation.Lerp(t);
            
            if (layotRebuild)
            {
                LayoutRebuilder.MarkLayoutForRebuild(_target);
            }
        }
    }
}