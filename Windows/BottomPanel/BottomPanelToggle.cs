using RLS.Audio;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RLS.Game.UI
{
    [RequireComponent(typeof(BottomPanelToggleAction))]
    public class BottomPanelToggle : MonoBehaviour
    {
        [Inject] private AudioManager _audioManager;

        [SerializeField] private SoundType _clickSound;
        
        [SerializeField] private BottomPanelToggleAction _action;
        [SerializeField] private Toggle _toggle;

        private void OnValidate()
        {
            _action = GetComponent<BottomPanelToggleAction>();
        }

        private void Awake()
        {
            _toggle.onValueChanged.AddListener(OnClick);
        }

        public void ForceSelect()
        {
            _toggle.isOn = true;
            OnClick(true);
        }

        private void OnClick(bool isOn)
        {
            if (isOn)
            {
                _audioManager.Play(_clickSound);
            }

            _action.Invoke(isOn);
        }
    }
}