using System;
using System.Collections.Generic;
using System.Linq;
using RLS.Game.Configs;
using RLS.UI;
using RLS.Utils.Pool;
using UnityEngine;
using Zenject;

namespace RLS.Game.UI
{
    [Window("ShopWindow/shop_window")]
    public class ShopWindow : Window, IAutoRebuildingContent
    {
        [Inject] private ShopConfigStorage _shopConfigStorage;
        [Inject] public ContainerRebuilder ContainerRebuilder { get; set; }
        
        [field:SerializeField] public List<RectTransform> RebuildingRects { get; private set; }

        [SerializeField] private UIListPool<ShopSection> _sectionsPool;

        public Action<IAutoRebuildingContent> RebuildRequested { get; set; }
        

        [Inject]
        private void Construct(GameObjectFactory factory)
        {
            _sectionsPool.SetupFactory(factory);
        }

        private void Awake()
        {
            (this as IAutoRebuildingContent).AppendRebuildingContent();
            InitializeSections();
        }

        public override void UpdateWindow()
        {
            RebuildRequested?.Invoke(this);
        }

        private void InitializeSections()
        {
            _sectionsPool.DeactivateAll();
            
            foreach (var sectionId in _shopConfigStorage.GetSectionIds())
            {
                _sectionsPool.GetNext().Setup(sectionId);
            }
            
            _sectionsPool.GetActiveElements().Last().MarkAsLast();
        }
    }
}