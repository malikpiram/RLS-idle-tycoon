using RLS.Game.Configs;
using RLS.Game.Profile;
using UnityEngine;
using Zenject;

namespace RLS.Game.Drop
{
    public class ResourceDrop : DropBase
    {
        [Inject] private ResourcesModel _resourcesModel;
        
        public readonly ResourceType ResourceType;
        
        public ResourceDrop(DropConfig config) : base(config)
        {
            switch (config.DropType)
            {
                case DropType.Egg:
                    ResourceType = ResourceType.Egg;
                    break;
                case DropType.Ticket:
                    ResourceType = ResourceType.Ticket;
                    break;
                default:
                    Debug.LogError($"Wrong DropType: {config.DropType}. No matching ResourceType");
                    break;
            }
        }

        public override void Process()
        {
            _resourcesModel.AddResources(ResourceType, Value);
        }
    }
}