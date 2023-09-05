using System;
using System.Collections.Generic;
using System.Numerics;
using RLS.Game.Extensions;
using RLS.Game.Profile;
using RLS.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RLS.UI
{
    [Window("ReturnWindow/return_window", WindowType.Popup)]
    public class ReturnWindow : Window, IAutoRebuildingContent
    {
        [Inject] private TimeController _timeController;
        [Inject] private AnalyticsModel _analyticsModel;
        [Inject] private LocalizationManager _localizationManager;
        [Inject] private ResourcesModel _resourcesModel;
        [Inject] private WindowsManager _windowsManager;
        [Inject] public ContainerRebuilder ContainerRebuilder { get; set; }
        
        private int TimeOffline => _timeController.Timestamp - _analyticsModel.ExitTimestamp;

        private BigInteger Reward => TimeOffline * ReturnWindowArguments.PaymentPerSecond;


        private const int DefaultRewardModifier = 2;
        private const int AdRewardModifier = 10;

        [SerializeField] private TextMeshProUGUI _valueAmount;
        [SerializeField] private TextMeshProUGUI _advValueAmount;
        [SerializeField] private TextMeshProUGUI _timeOffline;

        [SerializeField] private Button _claimButton;
        [SerializeField] private Button _adClaimButton;
        
        [field:SerializeField] public List<RectTransform> RebuildingRects { get; private set; }
        public Action<IAutoRebuildingContent> RebuildRequested { get; set; }

        private const string ValueKey = "<<return_window_offline_earnings>>";
        private const string AdvValueKey = "<<return_window_ad_earnings>>";
        private const string TimerKey = "<<return_window_time_offline>>";

        private BigInteger _adEarningsAmount;

        private ReturnWindowArguments ReturnWindowArguments => (ReturnWindowArguments) Arguments;

        private void Awake()
        {
            (this as IAutoRebuildingContent).AppendRebuildingContent();
            _claimButton.onClick.AddListener(OnClaimClick);
            _adClaimButton.onClick.AddListener(OnAdClaimClick);
        }

        private void OnClaimClick()
        {
            _resourcesModel.AddResources(ResourceType.Coin, Reward * DefaultRewardModifier);
            _windowsManager.Hide();
        }
        
        private void OnAdClaimClick()
        {
            _resourcesModel.AddResources(ResourceType.Coin, Reward * AdRewardModifier);
            _windowsManager.Hide();
        }

        public override void UpdateWindow()
        {
            WindowContentUpdate();
        }

        private void WindowContentUpdate()
        {
            _valueAmount.text = _localizationManager.Localize(ValueKey, $"{(Reward * DefaultRewardModifier).ToCurrencyString()}");
            _advValueAmount.text = _localizationManager.Localize(AdvValueKey, $"{(Reward * AdRewardModifier).ToCurrencyString()}");
            
            _timeOffline.text = _localizationManager.Localize(TimerKey, _timeController.SecondsToHMSString(TimeOffline));
            RebuildRequested?.Invoke(this);
        }
    }

    public class ReturnWindowArguments : IWindowArgs
    {
        public readonly BigInteger PaymentPerSecond;

        public ReturnWindowArguments(BigInteger paymentPerSecond)
        {
            PaymentPerSecond = paymentPerSecond;
        }
    }
}