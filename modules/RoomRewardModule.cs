using System.Numerics;
using RLS.Game.Configs;
using RLS.Game.Profile;
using RLS.Game.Signals;
using UnityEngine;
using Zenject;

namespace RLS.Game.Gameplay
{
    public class RoomRewardModule : RoomModule
    {
        [Inject] private RoomUpgradeConfigStorage _roomUpgradeConfigStorage;

        public BigInteger ResultReward => (BigInteger) ((double)CleanReward * GetIncomeBonus());
        public BigInteger CleanReward { get; private set; }
        
        private BonusModel _bonusModel;
        private int _currentLevelStep = 0;

        private BigInteger _currentBaseReward;
        
        protected override void OnInitialize()
        {
            CleanReward = RoomConfig.StartReward;
            SignalBus.Subscribe<UpgradeRoomStepSignal>(OnRoomUpgradeStep);
            SignalBus.Subscribe<UpgradeRoomSignal>(OnRoomUpgrade);
            InitializeReward();
        }

        private void OnRoomUpgrade(UpgradeRoomSignal signal)
        {
            if (!signal.ProductionRoomModel.Equals(ProductionRoomModel)) return;
            IncreaseRewardOnUpgrade();
        }

        private void OnRoomUpgradeStep(UpgradeRoomStepSignal signal)
        {
            if (!signal.ProductionRoomModel.Equals(ProductionRoomModel)) return;
            IncreaseRewardOnStep();
        }

        private void IncreaseRewardOnUpgrade()
        {
            _currentLevelStep = 0;
            CleanReward *= 2;
            _currentBaseReward = CleanReward;
        }

        private void IncreaseRewardOnStep(int currentUpgradeLevel = -1)
        {
            _currentLevelStep++;

            currentUpgradeLevel = currentUpgradeLevel == -1
                ? ProductionRoomController.UpgradeModule.CurrentUpgradeLevel
                : currentUpgradeLevel;
            
            var upgradeConfig = _roomUpgradeConfigStorage.GetConfig(currentUpgradeLevel);
            CleanReward += (BigInteger) ((double) GetMultipliedStartReward(upgradeConfig) *
                                    GetLevelRewardModifier(_currentLevelStep));
        }

        private BigInteger GetMultipliedStartReward(RoomUpgradeConfig upgradeConfig)
        {
            return _currentBaseReward * upgradeConfig.RewardMultiplier;
        }
        
        private double GetLevelRewardModifier(int level)
        {
            if (level <= 3) return level * .1;
            return .3 + (level - 1) * .05;
        }

        private void InitializeReward()
        {
            CleanReward = RoomConfig.StartReward;
            var currentLevel = 0;
            RoomUpgradeConfig roomUpgradeConfig = _roomUpgradeConfigStorage.GetConfig(currentLevel);
            _currentBaseReward = RoomConfig.StartReward;

            for (int i = 1; i <= ProductionRoomModel.CurrentUpgradeStep; i++)
            {
                if (roomUpgradeConfig.NeededSteps <= i)
                {
                    currentLevel++;
                    roomUpgradeConfig = _roomUpgradeConfigStorage.GetConfig(currentLevel);
                    IncreaseRewardOnUpgrade();
                }
                
                IncreaseRewardOnStep(currentLevel);
            }
        }

        private double GetIncomeBonus()
        {
            double result = 1;
            var bonusCards = ProductionRoomController.CardsModule.GetBonuses<IncomeBonus>();
            
            foreach (var bonusCard in bonusCards)
            {
                result *= bonusCard.Multiplier;
            }

            return result;
        }
    }
}