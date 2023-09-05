using DG.Tweening;
using RLS.Game.Extensions;
using RLS.Game.Profile;
using RLS.Game.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RLS.Game.UI
{
    public class ResourceUI : MonoBehaviour
    {
        [Inject] private ResourcesModel _resourcesModel;
        [Inject] private SignalBus _signalBus;

        [SerializeField] private ResourceWidget _resourceWidget;
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private Transform _iconTransform;
        [SerializeField] private ParticleSystem _effect;

        private RectTransform _rect;

        private void Awake()
        {
            _rect = transform as RectTransform;
            _signalBus.Subscribe<ResourceUpdateSignal>(OnResourceUpdate);
        }

        private void Start()
        {
            UpdateValue();
        }

        private void UpdateValue()
        {
            _resourceWidget.SetValue(_resourceType, _resourcesModel.GetResource(_resourceType));
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rect);
        }
        
        private void OnResourceUpdate(ResourceUpdateSignal signal)
        {
            if (!signal.Resource.Equals(_resourceType)) return;
            _iconTransform.DOKill();

            if (signal.Delta < 0)
            {
                _effect.Stop(true);
                _effect.Play(true);
            }

            UpdateValue();
        }
    }
}