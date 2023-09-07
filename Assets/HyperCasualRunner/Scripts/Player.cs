using System;
using HyperCasualRunner.GenericModifiers;
using HyperCasualRunner.Interfaces;
using HyperCasualRunner.Locomotion;
using HyperCasualRunner.PopulationManagers;
using HyperCasualRunner.ScriptableObjects;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

namespace HyperCasualRunner
{
    /// <summary>
    /// Brain of the player object, it acts like a composition root for other components like RunnerMover, PopulationManagerBase. It controls managers, enables or disables them, initializes them,
    /// ticks all the tickables, etc. So it's the most crucial piece when you create player controlled character.
    /// </summary>
    [DisallowMultipleComponent]
    public class Player : MonoBehaviour, IInteractor
    {
        private static Player instance;
        public static Player Instance { get { return instance; } }

        [SerializeField, Required] RunnerMover _runnerMover;
        [SerializeField, Required] PopulationManagerBase _populationManagerBase;
        [SerializeField, Required] InputChannelSO _inputChannelSO;
        [SerializeField] int _startingEntityCount;

        [SerializeField, HideIf(nameof(_useAnimation))] bool _useAnimator;
        [SerializeField, HideIf(nameof(_useAnimator))] bool _useAnimation;
        [SerializeField, ShowIf(nameof(_useAnimator))] AnimatorModifier _animatorModifier;
        [SerializeField, ShowIf(nameof(_useAnimation))] AnimationModifier _animationModifier;

        //private float CoinValue = 100f;
        //private float ObstacleValue = -470f;

        ITickable[] _tickables;

        //float Charge;

        //[SerializeField] private float duraiton;
        //[SerializeField] private float strenght;
        //[SerializeField] private int vibrato;
        //[SerializeField] private float randomness;

        //bool isShakeing;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            OnInteractionEnded();

            _runnerMover.Initialize();
            _populationManagerBase.Initialize();

            foreach (IInitializable initializable in GetComponents<IInitializable>())
            {
                initializable.Initialize(_populationManagerBase);
            }

            _tickables = GetComponents<ITickable>();
        }

        void OnEnable()
        {
            _inputChannelSO.JoystickUpdated += OnJoystickUpdate;
            _inputChannelSO.PointerDown += OnTouchDown;
            _inputChannelSO.PointerUp += OnTouchUp;
        }

        void OnDisable()
        {
            _inputChannelSO.JoystickUpdated -= OnJoystickUpdate;
            _inputChannelSO.PointerDown -= OnTouchDown;
            _inputChannelSO.PointerUp -= OnTouchUp;
        }

        void OnDestroy()
        {
            _runnerMover.OnDestroying();
        }

        void Start()
        {
            _populationManagerBase.AddPopulation(_startingEntityCount);
        }

        void Update()
        {
            foreach (ITickable tickable in _tickables)
            {
                tickable.Tick();
            }
        }

        public void OnInteractionBegin()
        {
            _runnerMover.enabled = false;
            enabled = false;
        }

        public void OnInteractionEnded()
        {
            _runnerMover.enabled = true;
            enabled = true;
        }

        void OnJoystickUpdate(Vector2 obj)
        {
            _runnerMover.Move(obj);
        }

        void OnTouchDown()
        {
            _runnerMover.TryStartMovement();
            if (_useAnimation)
            {
                _animationModifier.PlayLocomotion(1f);
            }
            else if (_useAnimator)
            {
                _animatorModifier.PlayLocomotion(1f);
            }
        }

        void OnTouchUp()
        {
            bool isMovementStopped = _runnerMover.TryStopMovement();
            if (!isMovementStopped)
            {
                return;
            }
            if (_useAnimation)
            {
                _animationModifier.PlayLocomotion(0f);
            }
            else if (_useAnimator)
            {
                _animatorModifier.PlayLocomotion(0f);
            }
        }
    }
}