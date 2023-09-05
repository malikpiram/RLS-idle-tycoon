using System.Collections;
using DG.Tweening;
using RLS.Game.UI;
using RLS.UI;
using UnityEngine;
using Zenject;

namespace RLS.Game.Gameplay
{
    public class AdButtonHudController : MonoBehaviour
    {
        [Inject] private HudReference<AdButtonHud> _adButtonHudReference;

        [SerializeField] private HudTarget _hudTarget;
        [SerializeField] private Transform _startTarget;
        [SerializeField] private Transform _endTarget;

        private void Start()
        {
            var hud = _adButtonHudReference.Show();
            hud.SetTarget(_hudTarget);
            StartCoroutine(Moving());
        }
        
        private IEnumerator Moving()
        {
            while (true)
            {
                _hudTarget.transform.position = _startTarget.position;
                yield return new WaitForSeconds(2f);
                yield return _hudTarget.transform.DOMove(_endTarget.position, 2f).WaitForCompletion();
            }
        }
    }
}