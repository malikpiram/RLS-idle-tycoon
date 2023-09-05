using DG.Tweening;
using RLS.Game.Drop;
using RLS.UI;
using TMPro;
using UnityEngine;

namespace UI.Windows
{
    [Window("DropRewardWindow/drop_reward_window")]
    public class DropRewardWindow : Window
    {
        private Args DropWindowArguments => (Args) Arguments;
        
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private DropVisualizerBehaviour _dropVisualizerBehaviour;
        
        private void Awake()
        {
            TextFading(_text);
        }

        public override void UpdateWindow()
        {
            _dropVisualizerBehaviour.SetDrop(DropWindowArguments.Drop);
        }

        private void TextFading(TextMeshProUGUI text)
        {
            var sequence = DOTween.Sequence().SetLoops(-1);
            sequence.Append(text.DOFade(0.5f, 0.5f));
            sequence.Append(text.DOFade(1f, 0.5f));
        }

        public class Args : IWindowArgs
        {
            public readonly DropBase Drop;
            
            public Args(DropBase drop)
            {
                Drop = drop;
            }
        }
    }
}