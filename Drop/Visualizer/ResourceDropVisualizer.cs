using RLS.Game.Libraries;
using UnityEngine;
using Zenject;

namespace RLS.Game.Drop
{
    public class ResourceDropVisualizer : DropVisualizer<ResourceDrop>
    {
        [Inject] private ResourcesLibrary _resourcesLibrary;

        public override void OnVisualize(ResourceDrop drop)
        {
            DropVisualizerBehaviour.TimerRoot.SetActive(false);
            DropVisualizerBehaviour.Icon.Sprite = _resourcesLibrary.GetSprite(drop.ResourceType);
            DropVisualizerBehaviour.AmountText.SetText($"+{drop.Value}"); 
        }
    }
}