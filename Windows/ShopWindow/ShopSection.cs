using RLS.Game.Configs;
using RLS.Utils;
using RLS.Utils.Pool;
using TMPro;
using UnityEngine;
using Zenject;

namespace RLS.Game.UI
{
    public class ShopSection : MonoBehaviour
    {
        [Inject] private ShopConfigStorage _shopConfigStorage;
        [Inject] private LocalizationManager _localizationManager;

        private const string TitleKeyTemplate = "<<shop_section_title_{0}>>";
        
        [SerializeField] private UIListPool<ShopWindowItem> _itemsPool;
        [SerializeField] private GameObject _bottomBorderRoot;
        [SerializeField] private TextMeshProUGUI _title;

        [Inject]
        private void Construct(GameObjectFactory factory)
        {
            _itemsPool.SetupFactory(factory);
        }

        public void Setup(int sectionId)
        {
            _bottomBorderRoot.SetActive(false);
            
            foreach (var config in _shopConfigStorage.GetSectionConfigs(sectionId))
            {
                _itemsPool.GetNext().Setup(config);
            }

            _title.text = _localizationManager.Localize(string.Format(TitleKeyTemplate, sectionId));
        }

        public void MarkAsLast()
        {
            _bottomBorderRoot.SetActive(true);
        }
    }
}