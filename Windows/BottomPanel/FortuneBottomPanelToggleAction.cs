using RLS.UI;
using UI.Windows.WheelOfFortune;
using Zenject;

namespace RLS.Game.UI
{
    public class FortuneBottomPanelToggleAction : BottomPanelToggleAction
    {
        [Inject] private WindowsManager _windowsManager;

        public override void Invoke(bool isOn)
        {
            if (isOn)
            {
                _windowsManager.Show<WheelOfFortuneWindow>();
                return;
            }
            
            _windowsManager.Hide();
        }
    }
}