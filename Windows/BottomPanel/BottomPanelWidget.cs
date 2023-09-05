using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using RLS.UI;
using UnityEngine;

namespace RLS.Game.UI
{
    [Window("BottomPanelWidget/bottom_panel_widget", WindowType.Widget, WindowPriority.Widget)]
    public class BottomPanelWidget : Widget
    {
        [SerializeField] private RectTransform _panelRoot;
        [SerializeField] private List<BottomPanelToggle> _toggles;

        private const float TransitionDuration = .3f;
        private const float Offset = 400f;
        
        private float _startPositionY;

        private void OnValidate()
        {
            _toggles = GetComponentsInChildren<BottomPanelToggle>().ToList();
        }

        private void Awake()
        {
            _toggles[2].ForceSelect();
        }

        public override void InitializeWidgetUpdateListener()
        {
            WidgetUpdateListener = new BottomPanelWidgetUpdateListener(this);
        }

        public void HidePanel()
        {
            _panelRoot.DOKill();
            _panelRoot.DOAnchorPosY(-Offset, TransitionDuration);
        }

        public void ShowPanel()
        {
            _panelRoot.DOKill();
            _panelRoot.DOAnchorPosY(0, TransitionDuration);
        }
    }
}