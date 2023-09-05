using System.Collections.Generic;
using System.Linq;
using RLS.Game.Configs;
using UnityEngine;
using Zenject;

namespace RLS.Game.Drop
{
    public class DropProcessor : MonoBehaviour
    {
        [Inject] private IDropFactory _dropFactory;
        [Inject] private DropConfigStorage _dropConfigStorage;

        public DropBase GetDrop(int dropPackId)
        {
            return _dropFactory.Create(_dropConfigStorage.GetDropByRule(dropPackId)[0]);
        }
        
        public void ProcessDrop(int dropPackId)
        {
            var dropConfigs = _dropConfigStorage.GetDropByRule(dropPackId);
            ProcessDrop(dropConfigs);
        }

        public void ProcessDrop(List<DropConfig> dropConfigs)
        {
            var drops = dropConfigs.Select(item => _dropFactory.Create(item)).ToList();
            ProcessDrop(drops);
        }
        
        public void ProcessDrop(DropConfig dropConfig)
        {
            var drop = _dropFactory.Create(dropConfig);
            ProcessDrop(new List<DropBase>{drop});
        }
        
        public void ProcessDrop(DropBase drop)
        {
            ProcessDrop(new List<DropBase>{drop});
        }
        
        public void ProcessDrop(List<DropBase> drops)
        {
            foreach (var drop in drops)
            {
                drop.Process();
            }
        }
    }
}