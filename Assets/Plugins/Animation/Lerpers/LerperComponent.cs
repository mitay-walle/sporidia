using TriInspector;
using UnityEditor;
using UnityEngine;

namespace UI.Utility.Lerpers
{
    public abstract class LerperComponent<T> : LerperGeneric<T> where T : Component
    {
        [SerializeField] protected T _target;
        [SerializeField] protected bool _disableComponent;

        #region Editor

#if UNITY_EDITOR
        [Button]
        protected virtual void Reset()
        {
            Undo.RecordObject(this, "reset");

            if (!_target) _target = GetComponentInChildren<T>(true);

            EditorUtility.SetDirty(this);
        }
#endif

        #endregion

        protected override void SetDirty()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) EditorUtility.SetDirty(_target);
#endif
        }
    }
}