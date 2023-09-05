using RLS.Game.Extensions;
using RLS.Game.Gameplay;
using RLS.Game.Libraries;
using RLS.Game.Profile;
using RLS.Utils;
using Zenject;

namespace RLS.Game.Drop
{
    public class TimeJumpDropVisualizer : DropVisualizer<TimeJumpDrop>
    {
        [Inject] private LevelController _levelController; 
        [Inject] private TimeController _timeController;
        [Inject] private ResourcesLibrary _resourcesLibrary;

        public override void OnVisualize(TimeJumpDrop drop)
        {
            DropVisualizerBehaviour.TimerRoot.SetActive(true);
            
            var timerValue =_timeController.SecondsToHMSString(drop.Value, TimeController.TimeFormat.DependsOnTimeClamped);
            DropVisualizerBehaviour.TimerText.SetText(timerValue);

            var coinsAmount = _levelController.GetPaymentPerSecond() * drop.Value;
            DropVisualizerBehaviour.AmountText.SetText($"+{coinsAmount.ToCurrencyString()}");
            
            DropVisualizerBehaviour.Icon.Sprite = _resourcesLibrary.GetSprite(ResourceType.Coin);
        }
    }
}