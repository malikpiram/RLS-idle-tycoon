using RLS.Game.Configs;
using RLS.Game.Drop;
using RLS.Game.Profile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RLS.Game.UI
{
    public class ShopWindowItem : MonoBehaviour
    {
        [Inject] private DropConfigStorage _dropConfigStorage;
        [Inject] private DropProcessor _dropProcessor;
        [Inject] private IDropFactory _dropFactory;
        [Inject] private ResourcesModel _resourcesModel;
        
        [field:SerializeField] protected Button Button { get; private set; }
        
        [SerializeField] private ResourceWidget _resourceWidget;
        [SerializeField] private TextMeshProUGUI _cashText;
        [SerializeField] private DropVisualizerBehaviour _dropVisualizer;

        private int _dropPack;
        private ShopConfig _shopConfig;

        public void Setup(ShopConfig shopConfig)
        {
            _dropPack = shopConfig.DropPack;
            _shopConfig = shopConfig;
            
            _resourceWidget.gameObject.SetActive(!shopConfig.IsPurchase);
            _cashText.gameObject.SetActive(shopConfig.IsPurchase);
            _dropVisualizer.SetDrop(_dropFactory.Create(_dropConfigStorage.GetDropByRule(_dropPack)![0]));

            if (shopConfig.IsPurchase)
            {
                _cashText.SetText($"{shopConfig.Price - .01f}$");
            }
            else
            {
                _resourceWidget.SetValue(shopConfig.Resource, shopConfig.Price);
            }
        }
        
        private void Awake()
        {
            Button.onClick.AddListener(OnClick);
        }

        protected void OnClick()
        {
            if (_shopConfig.IsPurchase ||
                _resourcesModel.TrySubtract(_shopConfig.Resource, _shopConfig.Price))
            {
                _dropProcessor.ProcessDrop(_dropPack);
            }
        }
    }
}