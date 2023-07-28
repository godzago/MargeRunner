﻿using HyperCasualRunner.ScriptableObjects;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace HyperCasualRunner
{
    /// <summary>
    /// It's a general purpose joystick.
    /// </summary>
    public class UIJoystick : MonoBehaviour
    {
        public static UIJoystick İnstance;

        const float TAP_THRESHOLD = 0.2f;

        [SerializeField, Tooltip("background of the joystick gameObject")] CanvasGroup _background;
        [SerializeField, Tooltip("input channel to get the inputs, with this, input values will be distributed across other components")] InputChannelSO _inputChannelSO;
        [SerializeField, Range(0f, 1f), Tooltip("opacity of the gameObject when you press it")] float _pressedOpacity = 0.5f;
        [SerializeField, Range(0f, 1f), Tooltip("Max range the joystick center can go")] float _maxRange = 0.2f;
        [SerializeField, Tooltip("Whether you can control joystick horizontally")] bool _horizontalAxisEnabled = true;
        [SerializeField, Tooltip("Whether you can control joystick vertically")] bool _verticalAxisEnabled = true;
        [SerializeField, Tooltip("If enabled, center follows the player's finger position.")] bool _isCenterDynamic;

        Transform _knobTransform;
        Vector2 _joystickValue;
        float _initialOpacity;
        float _touchTime;

        [SerializeField] TextMeshProUGUI scorText;
        private float coinValue;

        [SerializeField] Transform iconTrasfrom;
        private Camera mainCamera;


        [SerializeField] private GameObject failPanel, succesPanel;

        [SerializeField] Animator animator;
        [SerializeField] Animator animator1;
        [SerializeField] Animator animator2;
        [SerializeField] Animator animator3;

        void Awake()
        {
            İnstance = this;
            _initialOpacity = _background.alpha;
            _knobTransform = _background.transform.GetChild(0);
            _inputChannelSO.SetActiveInput += InputChannelSoOnSetActiveInput;
        }

        private void OnEnable()
        {
            EventManager.levelFailEvent.AddListener(OnFail);
            EventManager.levelSuccessEvent.AddListener(OnSuccess);
            EventManager.FirstFlag.AddListener(Flag1);
            EventManager.AfterFlag.AddListener(flag);
            EventManager.Flag2.AddListener(Flag2);
            EventManager.Flag3.AddListener(Flag3);
        }

        private void OnDisable()
        {
            EventManager.levelFailEvent.RemoveListener(OnFail);
            EventManager.levelSuccessEvent.RemoveListener(OnSuccess);
            EventManager.FirstFlag.RemoveListener(Flag1);
            EventManager.AfterFlag.RemoveListener(flag);
            EventManager.Flag2.RemoveListener(Flag2);
            EventManager.Flag3.RemoveListener(Flag3);
        }

        private void Start()
        {
            mainCamera = Camera.main;
        }
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _inputChannelSO.OnPointerDown();

                SetJoystickPosition();
            }
            else if (Input.GetMouseButton(0))
            {
                CalculateAndUpdateInput();

                _touchTime += Time.deltaTime;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _inputChannelSO.OnPointerUp();

                if (_touchTime <= TAP_THRESHOLD)
                {
                    _inputChannelSO.OnTapped();
                }

                _touchTime = 0f;
                ResetJoystick();
            }

            if (_horizontalAxisEnabled || _verticalAxisEnabled)
            {
                _inputChannelSO.OnJoystickUpdated(_joystickValue);
            }
        }

        void OnDestroy()
        {
            _inputChannelSO.SetActiveInput -= InputChannelSoOnSetActiveInput;
        }

        void InputChannelSoOnSetActiveInput(bool activeness)
        {
            gameObject.SetActive(activeness);
        }

        void SetJoystickPosition()
        {
            _background.transform.position = Input.mousePosition;
            _background.alpha = _pressedOpacity;
        }

        void CalculateAndUpdateInput()
        {
            Vector2 deltaPosition = (Vector2)Input.mousePosition - (Vector2)_background.transform.position;

            if (!_horizontalAxisEnabled)
            {
                deltaPosition.x = 0;
            }

            if (!_verticalAxisEnabled)
            {
                deltaPosition.y = 0;
            }

            _joystickValue.x = EvaluateInputValue(deltaPosition.x);
            _joystickValue.y = EvaluateInputValue(deltaPosition.y);
            _joystickValue = Vector2.ClampMagnitude(_joystickValue, 1f);

            if (_isCenterDynamic)
            {
                _background.transform.position = Input.mousePosition;
            }

            // clamp the knobTransform's position
            int maxRangeInPixel = Mathf.RoundToInt(_maxRange * UnityEngine.Screen.width);
            Vector2 clampedKnobPosition = (Vector2)_background.transform.position + Vector2.ClampMagnitude(deltaPosition, maxRangeInPixel);
            _knobTransform.position = clampedKnobPosition;
        }

        void ResetJoystick()
        {
            _knobTransform.position = _background.transform.position;
            _joystickValue.x = 0f;
            _joystickValue.y = 0f;
            _inputChannelSO.OnJoystickUpdated(_joystickValue);

            _background.alpha = _initialOpacity;
        }

        float EvaluateInputValue(float input)
        {
            return Mathf.InverseLerp(0f, _maxRange, Mathf.Abs(input / UnityEngine.Device.Screen.width)) * Mathf.Sign(input);
        }

        public void AddCountCoins(float amount)
        {
            coinValue += amount;
            scorText.text = coinValue.ToString();
        }
        public Vector3 GetIconPostion(Vector3 target)
        {
            Vector3 uiPos = iconTrasfrom.position;
            uiPos.z = (target - mainCamera.transform.position).z;
            Vector3 result = mainCamera.ScreenToWorldPoint(uiPos);

            return result;
        }

        private void OnFail()
        {
            failPanel.SetActive(true);
        }

        private void OnSuccess()
        {
            succesPanel.SetActive(true);
        }

        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Flag1()
        {
            animator.SetTrigger("flag_On");
        }

        public void flag()
        {
            animator1.SetTrigger("Flag");
        }
        public void Flag2()
        {
            animator2.SetTrigger("Flag2");
        }
        public void Flag3()
        {
            animator3.SetTrigger("Flag3");
        }
    }
}