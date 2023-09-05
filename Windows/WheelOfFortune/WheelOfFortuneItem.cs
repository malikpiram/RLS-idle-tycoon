using RLS.Game.Drop;
using UnityEngine;

namespace RLS.Game.Gameplay
{
    public class WheelOfFortuneItem : MonoBehaviour
    {
        public int DropId { get; private set; }

        [SerializeField] private DropVisualizerBehaviour _dropVisualizer;
        
        public void SetDrop(DropBase drop)
        {
            DropId = drop.Id;
            _dropVisualizer.SetDrop(drop);
        }
    }
}