using RLS.Game.Gameplay;
using RLS.Game.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RLS.Game.UI
{
    public abstract class RoomHud : RLS.UI.Hud, IAutomation
    {
        [Inject] protected SignalBus SignalBus { get; private set; }
        
        [field:SerializeField] protected Button Button { get; private set; }
        [field:SerializeField] protected RoomAutomationType TargetAutomation { get; private set; }
        
        protected ProductionRoomController ProductionRoomController { get; private set; }
        protected RoomAutomation RoomAutomation => ProductionRoomController.RoomAutomation;
        
        private void Awake()
        {
            SignalBus.Subscribe<RoomUpdateSignal>(OnUpdateSignal);
            OnAwake();
        }

        private void OnUpdateSignal(RoomUpdateSignal signal)
        {
            if (!signal.RoomController.Equals(ProductionRoomController)) return;
            UpdateState();
        }

        private void OnDestroy()
        {
            SignalBus.TryUnsubscribe<RoomUpdateSignal>(OnUpdateState);
            UnsubscribeAll();
        }

        public void Setup(ProductionRoomController productionRoom)
        {
            ProductionRoomController = productionRoom;
            OnSetup();
            OnUpdateState();
        }
        

        private void UpdateState()
        {
            if (Button != null)
            {
                Button.gameObject.SetActive(!ProductionRoomController.RoomAutomation.IsAutomationActive(TargetAutomation));
            }

            OnUpdateState();
        }
        
        public virtual bool IsConditionsMet() => false;
        public virtual void DoAutoAction() {}
        
        protected virtual void UnsubscribeAll(){}
        protected virtual void OnAwake(){}
        protected virtual void OnSetup(){}
        
        protected abstract void OnUpdateState();
    }
}