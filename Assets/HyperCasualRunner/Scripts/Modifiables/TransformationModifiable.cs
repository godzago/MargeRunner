using System;
using DG.Tweening;
using HyperCasualRunner.Interfaces;
using HyperCasualRunner.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace HyperCasualRunner.Modifiables
{
    /// <summary>
    /// Controls transformation of the PopulatedEntities. It also informs other ITransformator listeners for the updated child gameObject.
    /// </summary>
    [DisallowMultipleComponent]
    public class TransformationModifiable : MonoBehaviour, ITransformator
    {
        [SerializeField] bool _shouldChangeGameObjects = true;
        [SerializeField, ShowIf(nameof(_shouldChangeGameObjects))] GameObject[] _renderersByLevel;
        [SerializeField, HideIf(nameof(_shouldChangeGameObjects))] UnityEvent<int> _onLevelChanged;
        [SerializeField] bool _useTransformationParticle;
        [SerializeField, ShowIf(nameof(_useTransformationParticle))] ParticleSystem _transformationParticle;

        Tweener _deactivateTween;
        Tween _activateTween;
        readonly float _smoothingDuration = 0.5f;
        int _currentLevel;
        int _defaultLevel;
        
        public int MaxLevel => _renderersByLevel.Length;
        public int DefaultLevel
        {
            get
            {
                return _defaultLevel;
            }
            set
            {
                _defaultLevel = value;
                _currentLevel = DefaultLevel;
            }
        }

        public Action<GameObject> Transformed { get; set; }

        void OnDisable()
        {
            SetLevel(DefaultLevel);
        }

        void OnDestroy()
        {
            _deactivateTween.Kill();
            _activateTween.Kill();
        }

        public void SetLevelAdditive(int value)
        {
            int add = Mathf.Clamp(_currentLevel + value, 0, MaxLevel - 1);
            SetLevel(add);
        }
        
        public void SetLevel(int level)
        {
            if (_currentLevel == level)
            {
                return;
            }

            if (_shouldChangeGameObjects)
            {
                ChangeGameObject(level);
            }
            else
            {
                _onLevelChanged.Invoke(level);
            }
            _currentLevel = level;

            if (_useTransformationParticle)
            {
                _transformationParticle.Play(true);
            }
        }
        void ChangeGameObject(int level)
        {
            _deactivateTween = _renderersByLevel[_currentLevel].transform.DeactivateSlowly(_smoothingDuration);
            _activateTween = _renderersByLevel[level].transform.ShowSmoothly(_smoothingDuration);
            Transformed?.Invoke(_renderersByLevel[level]);
        }
    }
}