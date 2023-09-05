namespace RLS.Game.Drop
{
    public interface IDropVisualizer
    {
        public bool IsMatch(DropBase drop);
        public void Visualize(DropBase drop);
    }
}