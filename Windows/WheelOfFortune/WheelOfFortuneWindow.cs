using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using RLS.Game.Configs;
using RLS.Game.Drop;
using RLS.Game.Gameplay;
using RLS.UI;
using RLS.Utils.Pool;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows.WheelOfFortune
{
    [Window("WheelOfFortuneWindow/wheel_of_fortune_window")]
    public class WheelOfFortuneWindow : Window
    {
        [Inject] private DropProcessor _dropProcessor;
        [Inject] private IDropFactory _dropFactory;
        [Inject] private DropConfigStorage _dropConfigStorage;
        [Inject] private GeneralConfigStorage _generalConfigStorage;
        [Inject] private WindowsManager _windowsManager;

        private const string DropIdKey = "wheel_of_fortune_drop_id";
        
        [SerializeField] private UIListPool<WheelOfFortuneItem> _pool;
        [SerializeField] private Image _wheel;
        [SerializeField] private Button _button;

        private const float IdleSpinDuration = 20f;
        private const float StartSpinDuration = .5f;
        private const float MiddleSpinDuration = .3f;
        private const float FinishSpinDuration = 1f;
        private const float AfterSpinDelay = 1f;
        private const float CircleAngle = 360f;
        private const int SpinsCount = 8;
        
        private int _itemsCount;
        private int _wheelOfFortuneDropId;

        private DropBase _resultDrop;

        private void Start()
        {
            _button.onClick.AddListener(WheelSpin);
            
            _wheelOfFortuneDropId = _generalConfigStorage.GetParameter(DropIdKey)?.IntValue ?? 1;
            var potentialDropConfigs = GetPotentialDrop();
            _itemsCount = potentialDropConfigs.Count;
            
            for (int i = 0; i < potentialDropConfigs.Count; i++)
            {
                var drop = _dropFactory.Create(potentialDropConfigs[i]);
                var dropPreviewItem = _pool.GetNext();
                dropPreviewItem.SetDrop(drop);
                dropPreviewItem.transform.localEulerAngles += Vector3.forward * GetAngle(i);
            }

            StartIdle();
        }

        private void StartIdle(float delay = 0)
        {
            _wheel.transform.DORotate(Vector3.forward * CircleAngle, IdleSpinDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1)
                .SetDelay(delay);
        }
        
        [Inject]
        private void Construct(GameObjectFactory factory)
        {
            _pool.SetupFactory(factory);
        }

        private void WheelSpin()
        {
            _resultDrop = GetDrop();

            _wheel.transform.DOKill();

            var resultItem = _pool.GetActiveElements().FirstOrDefault(item => item.DropId == _resultDrop.Id);
            var targetAngle = Vector3.forward * 360 - resultItem.transform.localEulerAngles;

            var sequence = DOTween.Sequence();
            sequence.Append(_wheel.transform
                .DORotate(Vector3.forward * CircleAngle, StartSpinDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.InQuart));
            sequence.Append(_wheel.transform.DORotate(Vector3.forward * CircleAngle, MiddleSpinDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(SpinsCount));
            sequence.Append(_wheel.transform
                .DORotate(targetAngle, FinishSpinDuration, RotateMode.LocalAxisAdd)
                .SetEase(Ease.OutQuart));
            sequence.AppendCallback(() =>
            {
                _windowsManager.Show<DropRewardWindow>(new DropRewardWindow.Args(_resultDrop));
                GetReward();
                StartIdle(AfterSpinDelay);
            });
        }

        private void GetReward()
        {
            if (_resultDrop == null) return;
            _dropProcessor.ProcessDrop(_resultDrop);
            _resultDrop = null;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus) return;
            GetReward();
        }

        public List<DropConfig> GetPotentialDrop()
        {
            return _dropConfigStorage.GetDrop(_wheelOfFortuneDropId);
        }

        public DropBase GetDrop()
        {
            var dropConfigs = _dropConfigStorage.GetDropByRule(_wheelOfFortuneDropId);
            return _dropFactory.Create(dropConfigs[0]);
        }

        private float GetAngle(int itemIndex)
        {
            var angularOffset = 360 / _itemsCount;
            var startAngularOffset = angularOffset / 2;
            return startAngularOffset + angularOffset * itemIndex;
        }
    }
}