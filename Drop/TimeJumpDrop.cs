using RLS.Game.Configs;
using RLS.Game.Extensions;
using RLS.Game.Gameplay;
using RLS.Game.Profile;
using UnityEngine;
using Zenject;

namespace RLS.Game.Drop
{
    public class TimeJumpDrop : DropBase
    {
        [Inject] private LevelController _levelController;
        [Inject] private ResourcesModel _resourcesModel;

        public TimeJumpDrop(DropConfig dropConfig) : base(dropConfig){}

        public override void Process()
        {
            var rewardValue = _levelController.GetPaymentPerSecond() * Value;
            _resourcesModel.AddResources(ResourceType.Coin, rewardValue);
        }
    }
}