using System.Collections.Generic;
using RLS.Game.Configs;
using RLS.Game.Gameplay;
using RLS.Game.Profile;
using RLS.UI;
using UI.Windows.CardGettingWindow;
using Zenject;

namespace RLS.Game.Drop
{
    public class BonusCardDrop : DropBase
    {
        [Inject] private BonusCardConfigStorage _bonusCardConfigStorage;
        [Inject] private BonusConfigStorage _bonusConfigStorage;
        [Inject] private BonusCard.Factory _bonusCardFactory;
        [Inject] private InventoryModel _inventoryModel;
        [Inject] private BonusFactory _bonusFactory;
        [Inject] private WindowsManager _windowsManager;

        private int _cardId;
        
        public BonusCardDrop(DropConfig dropConfig) : base(dropConfig)
        {
            _cardId = dropConfig.Value;
        }

        public override void Process()
        {
            var cardConfig = _bonusCardConfigStorage.GetConfig(_cardId);
            var bonusConfigs = _bonusConfigStorage.GetBonuses(cardConfig.BonusId);

            var bonuses = new List<Bonus>();

            foreach (var bonusConfig in bonusConfigs)
            {
                bonuses.Add(_bonusFactory.Create(bonusConfig));
            }

            var card = _bonusCardFactory.Create().Initialize(cardConfig, bonuses);
            _inventoryModel.AddBonusCard(card);
            _windowsManager.Show<CardGettingWindow>(new CardGettingWindow.Args(card));
        }
    }
}