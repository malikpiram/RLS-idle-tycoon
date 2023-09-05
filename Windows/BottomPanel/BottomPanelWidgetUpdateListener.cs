using System;
using System.Collections.Generic;
using System.Linq;
using RLS.UI;
using UI.Windows;
using UI.Windows.CardGettingWindow;
using UnityEngine;

namespace RLS.Game.UI
{
    public class BottomPanelWidgetUpdateListener : IWidgetUpdateListener
    {
        private BottomPanelWidget _bottomPanelWidget;

        private List<Type> _fullyHideWindows = new()
        {
            typeof(DialogueWindow),
            typeof(DropRewardWindow),
            typeof(CardsSelectionWindow),
            typeof(CardGettingWindow),
        };
        
        public BottomPanelWidgetUpdateListener(BottomPanelWidget bottomPanelWidget)
        {
            _bottomPanelWidget = bottomPanelWidget;
        }
        
        public void OnWindowShown(Window window)
        {
            if (_fullyHideWindows.Any(item => item == window.GetType()))
            {
                _bottomPanelWidget.HidePanel();
                return;
            }
            
            _bottomPanelWidget.ShowPanel();
        }
    }
}