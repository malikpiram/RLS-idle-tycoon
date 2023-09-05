using RLS.Game.Configs;

namespace RLS.Game.Drop
{
    public interface IDropFactory
    {
        DropBase Create(DropConfig config);
    }
}