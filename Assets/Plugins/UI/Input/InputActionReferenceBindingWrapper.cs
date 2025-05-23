using Hierarchy2;
using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using static UnityEngine.InputSystem.InputBinding;

namespace GameJam.Plugins.UI.Input
{
    [CreateAssetMenu(menuName = "Input/InputActionReferenceBindingWrapper")]
    public class InputActionReferenceBindingWrapper : ScriptableObject,IAttentionIconBehaviour
    {
        [SerializeField, Required] private InputActionReference _inputAction;
        [SerializeField] private DisplayStringOptions _options;
        [SerializeField] private string _filterScheme;

        [Button]
        private void Log()
        {
            Debug.Log(ToString());

            foreach (InputBinding binding in _inputAction.action.bindings)
            {
                Debug.Log(binding.groups);
            }
        }

        public override string ToString()
        {
            if (InputUser.all.Count > 0)
            {
                InputAction action = _inputAction.action;
                var currentControlScheme = InputUser.all[0].controlScheme;
                int bindingIndex = action.GetBindingIndex(currentControlScheme.Value.bindingGroup);
                return _inputAction.action.GetBindingDisplayString(bindingIndex, out string _, out string _, _options);
            }

            return _inputAction.action.GetBindingDisplayString(_options);
        }

        public bool DrawAttentionIcon=>!_inputAction;
        public string Tooltip=>"InputAction is null";
    }
}
