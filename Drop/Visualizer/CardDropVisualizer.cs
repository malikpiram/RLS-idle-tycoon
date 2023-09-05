using RLS.Game.Libraries;
using Zenject;

namespace RLS.Game.Drop
{
    public class CardDropVisualizer : DropVisualizer<BonusCardDrop>
    {
        [Inject] private IconsLibrary _iconsLibrary;
        
        public override void OnVisualize(BonusCardDrop drop)
        {
            DropVisualizerBehaviour.TimerRoot.SetActive(false);
            DropVisualizerBehaviour.AmountText.text = "";
            DropVisualizerBehaviour.Icon.Sprite = _iconsLibrary.CardSprite;
        }
    }
}