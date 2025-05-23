using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace UI.Utility.Lerpers
{
    public abstract class Lerper : MonoBehaviour
    {
        [Range(0, 1), SerializeField, OnValueChanged(nameof(LerpEditor))]
        protected float _current;

        [SerializeField] protected float _speed = 1;
        [SerializeField] protected bool _loop;

        [SerializeField] protected List<Lerper> _children = new List<Lerper>();

        protected float _targetT;

        public float GetCurrentTargetT() => _targetT;

        private Coroutine _lerpCor;

        public event Action<float> OnLerpAnimatedFinished;
        
        public void Lerp(float t, bool invert)
        {
            if (invert)
            {
                Lerp(1 - t);
            }
            else
            {
                Lerp(t);
            }
        }

        public void LerpAnimated(float t, bool invert = false)
        {
            if (invert) t = 1 - t;
            _targetT = t;

            if (gameObject.activeInHierarchy)
            {
                if (_lerpCor != null)
                {
                    StopCoroutine(_lerpCor);
                }
                
                _lerpCor = StartCoroutine(LerpAnimatedCor(t));
            }
            else
            {
                Lerp(t);
                OnLerpAnimatedFinished?.Invoke(t);
            }
        }

        IEnumerator LerpAnimatedCor(float t)
        {
            float start = _current;

            while (Math.Abs(_current - t) > .01f)
            {
                Lerp(Mathf.MoveTowards(_current, t, Time.deltaTime * _speed));
                RepaintScene();
                yield return null;
            }

            Lerp(t);
            _lerpCor = null;
            
            OnLerpAnimatedFinished?.Invoke(t);
            
            if (_loop)
            {
                Lerp(start);
                LerpAnimated(t);
                yield break;
            }
        }

        public void LerpForDelta(float t)
        {
            Lerp(_current+t);
        }
        public void Lerp(float t)
        {
            foreach (Lerper child in _children)
            {
                child.Lerp(t);
            }

            _current = t;
            OnLerp(t);
            SetDirty();
        }

        protected void LerpEditor()
        {
            Lerp(_current);
        }

        protected void OnDisable()
        {
            Stop();
        }

        [Button]
        public void Toggle()
        {
            LerpAnimated(_targetT == 0 ? 1 : 0);
        }

        [ShowIf("@_lerpCor != null"), Button]
        public void Stop()
        {
            if (_lerpCor != null)
            {
                StopCoroutine(_lerpCor);
                _lerpCor = null;
                Lerp(_targetT);
            }
        }

        protected abstract void OnLerp(float t);
        protected abstract void SetDirty();

        [Conditional("UNITY_EDITOR")]
        protected void RepaintScene()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying && SceneView.lastActiveSceneView)
            {
                SceneView.lastActiveSceneView.Repaint();
            }
#endif
        }

        public bool IsLerping() => _lerpCor != null;
    }
}