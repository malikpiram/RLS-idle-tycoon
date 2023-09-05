using System.Collections.Generic;
using System.Linq;
using RLS.UI;
using TMPro;
using UnityEngine;

namespace RLS.Game.Drop
{
    public class DropVisualizerBehaviour : MonoBehaviour
    {
        private List<IDropVisualizer> _dropVisualizers;

        [field:SerializeField] public TextMeshProUGUI TimerText;
        [field:SerializeField] public TextMeshProUGUI AmountText;
        [field:SerializeField] public GameObject TimerRoot;
        [field:SerializeField] public BoundedImage Icon;
        
        private void Awake()
        {
            _dropVisualizers = GetComponents<IDropVisualizer>().ToList();
        }
        
        public void SetDrop(DropBase drop)
        {
            foreach (var dropVisualizer in _dropVisualizers)
            {
                if (!dropVisualizer.IsMatch(drop)) continue;
                dropVisualizer.Visualize(drop);
            }
        }
    }
}