using RLS.Game.Configs;

namespace RLS.Game.Drop
{
    public class DropBase
    {
        public readonly int Id;
        public readonly int Value;
        
        public DropBase(DropConfig dropConfig)
        {
            Value = dropConfig.Value;
            Id = dropConfig.Id;
        }
        
        public virtual void Process(){}
    }
}