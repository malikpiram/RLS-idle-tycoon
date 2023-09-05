using RLS.Data;
using RLS.Game.Configs;
using RLS.Game.Data;
using RLS.Game.Profile;
using RLS.UI;
using RLS.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RLS.Game.UI
{
    [Hud("room_upgrade_hud")]
    public class RoomUpgradeHud : RoomHud
    {
        [Inject] private ResourcesModel _resourcesModel;
        [Inject] private RoomUpgradeConfigStorage _roomUpgradeConfigStorage;
        [Inject] private LocalizationManager _localizationManager;
        [Inject] private SaveManager _saveManager;

        private const string LevelKey = "<<upgrade_hud_level>>";
        private const string NameKeyTemplate = "<<room_name_{0}>>";

        [SerializeField] private TextMeshProUGUI _currentUpgradeLevel;
        [SerializeField] private TextMeshProUGUI _nameLabel;
        [SerializeField] private ResourceWidget _rewardWidget;
        [SerializeField] private ResourceWidget _costWidget;
        [SerializeField] private Slider _experienceValue;

        private RoomHud _roomHudImplementation;

        protected override void OnSetup()
        {
            Button.onClick.AddListener(OnClick);

            var nameKey = string.Format(NameKeyTemplate, ProductionRoomController.Id);
            _nameLabel.SetText(_localizationManager.Localize(nameKey));
        }

        private void OnClick()
        {
            if (_resourcesModel.TrySubtract(ResourceType.Coin, ProductionRoomController.UpgradeModule.UpgradeCost))
            {
                ProductionRoomController.UpgradeModule.IncrementExperience();
                _saveManager.Save();
            }
        }

        protected override void OnUpdateState()
        {
            UpdateText();
            UpdateProgressBar();
        }

        private void UpdateText()
        {
            _currentUpgradeLevel.text =
                _localizationManager.Localize(LevelKey, ProductionRoomController.CurrentStep.ToString());
            _rewardWidget.SetValue(ResourceType.Coin, ProductionRoomController.RewardModule.ResultReward);
            _costWidget.SetValue(ResourceType.Coin, ProductionRoomController.UpgradeModule.UpgradeCost);
        }

        private void UpdateProgressBar()
        {
            var previousUpgradeConfig =
                _roomUpgradeConfigStorage.GetConfig(ProductionRoomController.UpgradeModule.CurrentUpgradeLevel - 1);

            var currentStep = ProductionRoomController.CurrentStep;
            var targetStep = ProductionRoomController.UpgradeModule.TargetStep;

            if (previousUpgradeConfig != null)
            {
                currentStep -= previousUpgradeConfig.NeededSteps;
                targetStep -= previousUpgradeConfig.NeededSteps;
            }

            _experienceValue.value = (float) currentStep / targetStep;
        }
    }
}
