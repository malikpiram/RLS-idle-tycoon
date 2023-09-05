using System.Linq;
using System.Numerics;
using RLS.Game.Configs;
using RLS.Game.Signals;
using UnityEngine;
using Zenject;

namespace RLS.Game.Gameplay
{
    public class RoomUpgradeModule : RoomModule
    {
        public BigInteger UpgradeCost { get; private set; }
        public int CurrentUpgradeLevel { get; private set; }
        public int TargetStep { get; private set; }
        
        [Inject] private RoomUpgradeConfigStorage _roomUpgradeConfigStorage;

        protected override void OnInitialize()
        {
            var config = _roomUpgradeConfigStorage.Data.FirstOrDefault(item => item.NeededSteps > ProductionRoomModel.CurrentUpgradeStep);
            CurrentUpgradeLevel = config.Level;
            TargetStep = config.NeededSteps;
            
            InitializeUpgradeCost();
        }

        public void IncrementExperience()
        {
            if (IsMaxLevelReached())
            {
                return;
            }
            
            ProductionRoomModel.CurrentUpgradeStep++;
            
            if (IsExperienceReached())
            {
                CurrentUpgradeLevel++;
                var config = _roomUpgradeConfigStorage.GetConfig(CurrentUpgradeLevel);
                TargetStep = config.NeededSteps;
                SignalBus.Fire(new UpgradeRoomSignal(ProductionRoomModel, config));
            }

            UpdateUpgradeCost();
            SignalBus.Fire(new UpgradeRoomStepSignal(ProductionRoomModel));
            SignalBus.Fire(new RoomUpdateSignal(ProductionRoomController));
        }

        private bool IsExperienceReached()
        {
            return ProductionRoomModel.CurrentUpgradeStep >= TargetStep;
        }

        public void InitializeUpgradeCost()
        {
            var currentConfig =
                _roomUpgradeConfigStorage.Data.FirstOrDefault(item =>
                    item.NeededSteps > ProductionRoomModel.CurrentUpgradeStep);

            currentConfig ??= _roomUpgradeConfigStorage.Data.Last();
            CurrentUpgradeLevel = currentConfig.Level;
            
            UpdateUpgradeCost();
        }

        private void UpdateUpgradeCost()
        {
            var upgradeConfig = _roomUpgradeConfigStorage.GetConfig(CurrentUpgradeLevel);
            UpgradeCost = (BigInteger) ((double)ProductionRoomController.RewardModule.CleanReward * upgradeConfig.UpgradeCostModifier);
        }

        private bool IsMaxLevelReached()
        {
            return CurrentUpgradeLevel == _roomUpgradeConfigStorage.Data.Last().Level - 1;
        }
    }
}