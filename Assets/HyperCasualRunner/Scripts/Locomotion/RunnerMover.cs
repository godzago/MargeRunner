using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasualRunner.Locomotion
{
    /// <summary>
    /// Customizable object mover class. Specifically designed for runner games.
    /// </summary>
    public class RunnerMover : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] float _horizontalMoveSpeed = 3f;
        [SerializeField] float _forwardMoveSpeed = 15f;
        [SerializeField] bool _canControlForwardMovement;
        [SerializeField, Tooltip("If it's true, object will rotate itself so it's up is aligned with the ground's up direction")] bool _shouldOrientUpDirectionToGround;
        [SerializeField, ShowIf(nameof(_shouldOrientUpDirectionToGround)), Tooltip("Detection range of the ground when orienting itself")] float _orientUpDirectionRange = 2f;
        [SerializeField, Tooltip("Lower the number, higher the gravity power")] float _gravityPower = -8f;
        [SerializeField, Range(0f, 1f), Tooltip("0 for tight control")] 
        float _horizontalSmoothingTime = 0.13f;
        [SerializeField, Range(0f, 1f), Tooltip("0 for tight control")] 
        float _forwardSmoothingTime = 0.17f;

        [Header("Dependencies")] 
        [SerializeField] CharacterController _characterController;
        [SerializeField] bool _turnToMovingDirection;
        [SerializeField, ShowIf(nameof(_turnToMovingDirection)), Required] 
        GameObject _gameObjectToTurn;
        [SerializeField, ShowIf(nameof(_turnToMovingDirection))] float _maxRotatingLimit = 15f;
        [SerializeField, ShowIf(nameof(_turnToMovingDirection))] float _rotationSpeed = 9f;

        [Header("Movement Constrain")]
        [SerializeField] bool _shouldConstrainMovement;
        [SerializeField, ShowIf(nameof(_shouldConstrainMovement))] MovementConstrainerBase _movementConstrainer;
        
        int _canGoForward; // 1 yes, 0 no
        float _gravitationalVelocity;
        Vector3 _horizontalMovement;
        Vector3 _forwardMovement;
        Vector3 _horizontalVelocity;
        Vector3 _forwardVelocity;
        Vector3 _initialForwardDirection;
        Vector3 _groundNormal = Vector3.up;

        [SerializeField] Slider slider;
        [SerializeField] float countdownTime = 2f;
        private float currentTime;

        private float speedXValue = 3f;

        public float ForwardMoveSpeed { get => _forwardMoveSpeed; set => _forwardMoveSpeed = value; }
        void Start()
        {
            currentTime = countdownTime;
            slider.maxValue = countdownTime;
            slider.value = currentTime;
        }
        public void Initialize()
        {
            _movementConstrainer.Initialize(gameObject);
            _initialForwardDirection = transform.forward;
        }

        public void OnDestroying()
        {
            _movementConstrainer.OnDestroying();
        }

        public void TryStartMovement()
        {
            _canGoForward = 1;
            EventManager.FirstFlag.Invoke();
        }

        public bool TryStopMovement()
        {
            if (_canControlForwardMovement) _canGoForward = 0;
            return _canControlForwardMovement;
        }

        /// <summary>
        /// If you feed this method with Vector2, it will move the RunnerMover towards that.
        /// </summary>
        /// <param name="moveDirection">Move Direction to go. It has to be normalized. It will be multiplied by the movement speed.</param>

        public void Move(Vector2 moveDirection)
        {
            if (!enabled)
            {
                return;
            }
                var trans = transform;
                Vector3 horizontalMovementRaw = trans.right * moveDirection.x;
                Vector3 forwardMovementRaw = trans.forward * _forwardMoveSpeed * speedXValue;
                _horizontalMovement = Vector3.SmoothDamp(_horizontalMovement, horizontalMovementRaw * _horizontalMoveSpeed, ref _horizontalVelocity, _horizontalSmoothingTime);
                _forwardMovement = Vector3.SmoothDamp(_forwardMovement, forwardMovementRaw * _canGoForward, ref _forwardVelocity, _forwardSmoothingTime);
                Vector3 totalInputMovement = _horizontalMovement + _forwardMovement;

                if (_characterController.isGrounded)
                {
                    _gravitationalVelocity = _gravityPower * Time.deltaTime;
                }
                else
                {
                    _gravitationalVelocity += _gravityPower * Time.deltaTime;
                }

                Vector3 finalMovement = (totalInputMovement + transform.up * _gravitationalVelocity) * Time.deltaTime;

                if (_shouldConstrainMovement)
                {
                    finalMovement = _movementConstrainer.GetConstrainedMotion(finalMovement, transform.position);
                }

                _characterController.Move(finalMovement);

                if (_shouldOrientUpDirectionToGround)
                {
                    bool isNearGround = Physics.Raycast(transform.position, Vector3.down * _orientUpDirectionRange, out RaycastHit hitInfo, 2f);
                    if (isNearGround) _groundNormal = hitInfo.normal;
                }

                if (_turnToMovingDirection)
                {
                    // TODO: This part might cause problem when rotating
                    Quaternion targetRotation = Quaternion.LookRotation(_initialForwardDirection + horizontalMovementRaw * _maxRotatingLimit, _groundNormal);

                    _gameObjectToTurn.transform.rotation = Quaternion.Slerp(
                        _gameObjectToTurn.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                }
            
        }
        private void Update()
        {
            SliderBarSettigs();
        }

        void Reset()
        {
            _characterController = GetComponent<CharacterController>();
        }
        void SliderBarSettigs()
        {
            if (currentTime > 0 && _canGoForward == 1)
            {
                currentTime -= Time.deltaTime;
                slider.value = currentTime;
            }

            if (currentTime < 0)
            {
                speedXValue = 0f;
                EventManager.levelFailEvent.Invoke();
            }           
        }
    }
}