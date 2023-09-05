using UnityEngine;

namespace RLS.Game.UI
{
    public class HudUpgradeLevel : MonoBehaviour
    {
        public int Level;
        
        private void LevelUp(bool levelUp)
        {
            if (levelUp)
            {
                Level++;
            }
        }
    }
}