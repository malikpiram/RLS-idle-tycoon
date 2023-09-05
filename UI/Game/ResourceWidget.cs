using System.Numerics;
using RLS.Game.Extensions;
using RLS.Game.Libraries;
using RLS.Game.Profile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RLS.Game.UI
{
    public class ResourceWidget : MonoBehaviour
    {
        [Inject] private ResourcesLibrary _resourcesLibrary;
        
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _valueText;
        [HideInInspector][SerializeField] private RectTransform _rectTransform;

        private void OnValidate()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        }

        public void SetValue(ResourceType resourceType, BigInteger value)
        {
            _icon.sprite = _resourcesLibrary.GetSprite(resourceType);
            _valueText.SetText(value.ToCurrencyString());
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        }
    }
}