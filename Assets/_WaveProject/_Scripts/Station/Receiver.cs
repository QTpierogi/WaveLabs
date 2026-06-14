using System;
using System.Collections;
using TMPro;
using UnityEngine;
using WaveProject.Configs;
using WaveProject.Interaction;
using WaveProject.Station.PlateLogic.Plates;
using WaveProject.Utility;
using Random = UnityEngine.Random;

namespace WaveProject.Station
{
    public class Receiver : MonoBehaviour
    {
        [Min(0), SerializeField] public float _defaultScaleFactor = 1;
        [Min(0), SerializeField] private float _defaultMultiplierFactor = 1;
        [SerializeField] private StateRotateInteractable _scaleMultiplierHandle;
        [SerializeField] private RotateInteractable _scaleFactorHandle;
        [SerializeField] private RotateInteractable _zeroOffsetHandle;
        [SerializeField] private Switcher _zeroSwitcher;
        [SerializeField] private Switcher _powerSwitcher;
        [SerializeField] private MeshRenderer _powerMeshRenderer;
        [SerializeField] private MeshRenderer _powerZeroOffsetMeshRenderer;

        [Space]
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _arrowAngleRange = 70;
        [SerializeField] private Transform _arrow;

        [Space]
        [SerializeField] private Calculator calculator;

        private int _speedFactor;
        private float _speedToTarget;

        private float _maxScaleFactor;

        private float _maxMultiplierFactor;
        public float PowerFactor { get; private set; }
        public float Frequency { get; private set; }

        private float _startZeroOffsetFactor;
        private float _minZeroOffsetFactor;
        private float _maxZeroOffsetFactor;

        private bool _isEnable;
        private bool _multiplierIsEnabled;

        private bool _isZeroOffset;
        private float _zeroOffset;

        private float _result;
        private float CurrentTarget => Math.Clamp(_isEnable ? _result + _zeroOffset : 0, _minZeroOffsetFactor, 100);

        private void OnValidate()
        {
            if (_defaultScaleFactor > InteractionSettings.Data.MaxReceiverScaleFactor)
            {
                _defaultScaleFactor = InteractionSettings.Data.MaxReceiverScaleFactor;
            }

            if (_defaultMultiplierFactor > InteractionSettings.Data.MaxReceiverMultiplierFactor)
            {
                _defaultMultiplierFactor = InteractionSettings.Data.MaxReceiverMultiplierFactor;
            }
        }

        public void Init(bool useExtraHandles)
        {
            const bool defaultZeroSetter = false;
            const bool defaultPower = false;

            LoadData();
            StartCoroutine(AimForResultValue());

            _zeroOffset = Random.Range(_startZeroOffsetFactor, _maxZeroOffsetFactor);

            _scaleFactorHandle.Init();
            _scaleFactorHandle.SetDefaultValue(_defaultScaleFactor, 0, _maxScaleFactor);

            if(useExtraHandles)
            {
                _multiplierIsEnabled = true;
                _scaleMultiplierHandle.Init();
                _scaleMultiplierHandle.SetDefaultValue(_defaultMultiplierFactor);
            }

            _zeroOffsetHandle.Init();
            _zeroOffsetHandle.SetDefaultValue(_zeroOffset, _minZeroOffsetFactor, _maxZeroOffsetFactor);

            _zeroSwitcher.Init();
            _zeroSwitcher.SetDefaultToggledState(defaultZeroSetter);
            _zeroSwitcher.Toggled.AddListener(ToggleZeroSetter);
            ToggleZeroSetter(defaultZeroSetter);

            _powerSwitcher.Init();
            _powerSwitcher.SetDefaultToggledState(defaultPower);
            _powerSwitcher.Toggled.AddListener(TogglePower);
            TogglePower(defaultPower);

            calculator.Init();
        }

        private void LoadData()
        {
            _maxScaleFactor = InteractionSettings.Data.MaxReceiverScaleFactor;
            _startZeroOffsetFactor = InteractionSettings.Data.StartZeroOffsetFactor;
            _minZeroOffsetFactor = InteractionSettings.Data.MinZeroOffsetFactor;
            _maxZeroOffsetFactor = InteractionSettings.Data.MaxZeroOffsetFactor;
            _maxMultiplierFactor = InteractionSettings.Data.MaxReceiverMultiplierFactor;
            _speedToTarget = InteractionSettings.Data.ReceiverArrowSpeedToTarget;
        }

        public void TogglePower(bool value)
        {
            _isEnable = value;

            if (_isEnable)
                _powerMeshRenderer.material.EnableKeyword("_EMISSION");
            else _powerMeshRenderer.material.DisableKeyword("_EMISSION");

            if (_isEnable && _isZeroOffset)
                _powerZeroOffsetMeshRenderer.material.EnableKeyword("_EMISSION");
            else _powerZeroOffsetMeshRenderer.material.DisableKeyword("_EMISSION");
        }

        public void ToggleZeroSetter(bool value)
        {
            _isZeroOffset = value;

            if (_isEnable && _isZeroOffset)
                _powerZeroOffsetMeshRenderer.material.EnableKeyword("_EMISSION");
            else _powerZeroOffsetMeshRenderer.material.DisableKeyword("_EMISSION");
        }

        public void SendPowerFactor(float power)
        {
            PowerFactor = power;
        }

        public void SendFrequency(float frequency)
        {
            Frequency = frequency;
        }

        private void Update()
        {
            _speedFactor = _isEnable ? 1 : 2;
            _speedFactor = _isZeroOffset ? 2 : 1;

            _zeroOffset = _isEnable ? _zeroOffsetHandle.GetValue() : 0;

            if (_isEnable == false || Frequency == 0) return;
            
            _defaultScaleFactor = _scaleFactorHandle.GetValue();
            if (_multiplierIsEnabled)
                _defaultScaleFactor /= _scaleMultiplierHandle.GetValue();
            var value = calculator.CalculateValue(PowerFactor, Frequency);
            _result = value * _defaultScaleFactor * 100;
        }

        private IEnumerator AimForResultValue()
        {
            float currentValue = 0;
            const float maxValue = 100f;
            var minValue = _minZeroOffsetFactor;

            while (true)
            {
                var currentTarget = _isZeroOffset ? _zeroOffset : CurrentTarget;

                currentValue = Mathf.Lerp(currentValue, currentTarget, _speedFactor * _speedToTarget * Time.deltaTime);

                _text.text = $"{Math.Clamp(Math.Round(currentValue), minValue, maxValue)}";

                _arrow.rotation = Utils.GetRotationInRange(currentValue, minValue, maxValue, -_arrowAngleRange + minValue, _arrowAngleRange, Vector3.right);

                yield return null;
            }
        }
    }
}