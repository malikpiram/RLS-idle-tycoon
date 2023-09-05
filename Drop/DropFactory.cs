using System;
using RLS.Game.Configs;
using UnityEngine;
using Zenject;

namespace RLS.Game.Drop
{
    public class DropFactory : IDropFactory
    {
        private Action<object> _inject;

        [Inject]
        private void Construct(DiContainer container)
        {
            _inject = container.Inject;
        }
        
        public DropBase Create(DropConfig config)
        {
            DropBase drop = null;
            
            switch (config.DropType)
            {
                case DropType.Egg:
                case DropType.Ticket:
                    drop = new ResourceDrop(config);
                    break;
                case DropType.TimeJump:
                    drop = new TimeJumpDrop(config);
                    break;
                case DropType.ProductionBoost:
                    drop = new ProductionBoostDrop(config);
                    break;
                case DropType.BonusCard:
                    drop = new BonusCardDrop(config);
                    break;
                default:
                    Debug.LogError($"Can't create Drop for {config.DropType}");
                    break;
            }

            if (drop == null) return null;

            _inject(drop);
            return drop;
        }
    }
}