using RLS.UI;
using Zenject;

namespace RLS.Game.UI
{
    public class ShopBottomPanelToggleAction : BottomPanelToggleAction
    {
        [Inject] private WindowsManager _windowsManager;
        
        public override void Invoke(bool isOn)
        {
            if (isOn)
            {
                _windowsManager.Show<ShopWindow>();
                return;
            }
            
            _windowsManager.Hide();
        }
    }
}