using UnityEngine;

namespace RLS.Game.Drop
{
    [RequireComponent(typeof(DropVisualizerBehaviour))]
    public abstract class DropVisualizer<T> : MonoBehaviour, IDropVisualizer where T : DropBase
    {
        protected DropVisualizerBehaviour DropVisualizerBehaviour { get; private set; }
        
        private void Awake()
        {
            DropVisualizerBehaviour = GetComponent<DropVisualizerBehaviour>();
        }
        
        public bool IsMatch(DropBase drop) => drop is T;

        public void Visualize(DropBase drop)
        {
            OnVisualize(drop as T);
        }

        public abstract void OnVisualize(T drop);
    }
}