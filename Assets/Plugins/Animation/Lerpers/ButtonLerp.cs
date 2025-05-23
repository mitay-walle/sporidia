using UnityEngine;
using UnityEngine.UI;

namespace UI.Utility.Lerpers
{
    public class ButtonLerp : MonoBehaviour
    {
        [SerializeField] private Lerper _lerper;
        
        private void Start()
        {
            var btn = GetComponentInChildren<Button>();
            btn.onClick.AddListener(OnValueChangedHandler);
        }

        private void OnValueChangedHandler()
        {
            _lerper.Toggle();
        }
    }
}
