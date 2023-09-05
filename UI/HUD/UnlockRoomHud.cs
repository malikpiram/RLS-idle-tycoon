using System.Numerics;
using RLS.Game.Libraries;
using RLS.Game.Profile;
using RLS.Game.Signals;
using RLS.UI;
using UnityEngine;
using Zenject;

namespace RLS.Game.UI
{
    [Hud("unlock_room_hud")]
    public class UnlockRoomHud : RoomHud
    {
        [Inject] private ResourcesModel _resourcesModel;
        [Inject] private MaterialsLibrary _materialsLibrary;

        private BigInteger UnlockCost => ProductionRoomController.UnlockCost;
        
        [SerializeField] private ResourceWidget _resourceWidget;

        protected override void OnAwake()
        {
            SignalBus.Subscribe<ResourceUpdateSignal>(OnUpdateState);
        }

        protected override void OnSetup()
        {
            Button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (!IsUnlockAvailable()) return;
            if (_resourcesModel.TrySubtract(ResourceType.Coin, UnlockCost))
            {
                ProductionRoomController.Unlock();
            }
        }

        protected override void OnUpdateState()
        {
            _resourceWidget.SetValue(ResourceType.Coin, UnlockCost);
            _materialsLibrary.SetGray(Button.image, !IsUnlockAvailable());
        }

        private bool IsUnlockAvailable()
        {
            return _resourcesModel.GetResource(ResourceType.Coin) >= UnlockCost;
        }

        protected override void UnsubscribeAll()
        {
            SignalBus.TryUnsubscribe<ResourceUpdateSignal>(OnUpdateState);
        }
    }
}