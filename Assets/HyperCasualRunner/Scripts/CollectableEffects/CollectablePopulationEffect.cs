using DG.Tweening;
using HyperCasualRunner.PopulationManagers;
using NaughtyAttributes;
using UnityEngine;

namespace HyperCasualRunner.CollectableEffects
{
    /// <summary>
    /// Use when you want to have a bouncing collectables, like when you hit obstacle in Coffee Stack. It has a Throw method that allows you to throw it.
    /// </summary>
    [DisallowMultipleComponent]
    public class CollectablePopulationEffect : CollectableEffectBase
    {
        [SerializeField] bool _useMultiply;
        [SerializeField, HideIf(nameof(_useMultiply))] int _increaseAmount = 1;
        [SerializeField, ShowIf(nameof(_useMultiply)), Min(0f)] float _multiplyRatio = 1.5f;
        
        Tween _jumpTween;
        Tween _destroyTween;

        public void Throw(Vector3 relativePosition)
        {
            _jumpTween = transform.DOJump(relativePosition, 2f, 1, 2.5f).SetRelative().SetEase(Ease.OutBack);
            _destroyTween = DOVirtual.DelayedCall(10f, () => Destroy(gameObject), false);
        }

        public override void ApplyEffect(PopulationManagerBase manager)
        {
            _jumpTween.Kill();
            _destroyTween.Kill();
            if (_useMultiply)
            {
                if (!Mathf.Approximately(_multiplyRatio, 0f))
                {
                    manager.MultiplyPopulation(_multiplyRatio);
                }
            }
            else
            {
                manager.AddPopulation(_increaseAmount);
            }
        }
    }
}
