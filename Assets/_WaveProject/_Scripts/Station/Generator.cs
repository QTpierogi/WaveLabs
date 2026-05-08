using TMPro;
using UnityEngine;
using WaveProject.Configs;
using WaveProject.Interaction;
using WaveProject.Utility;

namespace WaveProject.Station
{
    public class Generator : MonoBehaviour
    {
        [Header("Frequency settings")] 
        [Min(0), SerializeField] private float _defaultFrequency = 8000f;
        [SerializeField] private RotateInteractable _frequencyHandle;
        [SerializeField] private TMP_Text _textFrequency;

        [Header("Power settings")] 
        [Min(0), SerializeField] private float _defaultPower = 10;
        [SerializeField] private Toggle _powerToggle;
        [SerializeField] private MeshRenderer _powerMeshRenderer;
        [SerializeField] private RotateInteractable _powerHandle;

        [Space]
        [SerializeField] private Receiver _receiver;
        
        private float _minFrequency;
        private float _maxFrequency;
        private float _frequencyStep;
        private float _currentFrequency;
        
        private float _maxPower;
        private float _powerStep;
        private float _currentPower;
        private bool _isEnable;

        public bool IsEnable => _isEnable;

        private void OnValidate()
        {
            if (_defaultFrequency > InteractionSettings.MAX_FREQUENCY)
            {
                _defaultFrequency = InteractionSettings.MAX_FREQUENCY;
            }

            if (_defaultPower > InteractionSettings.Data.MaxPower)
            {
                _defaultPower = InteractionSettings.Data.MaxPower;
            }
        }

        public void Init()
        {
            const bool defaultPower = false;
            
            LoadData();
            
            _frequencyHandle.Init();
            _frequencyHandle.SetDefaultValue(_defaultFrequency, _minFrequency, _maxFrequency);
            
            _powerHandle.Init();
            _powerHandle.SetDefaultValue(_defaultPower, 0, _maxPower);
            
            _powerToggle.Init();
            _powerToggle.SetDefaultToggledState(defaultPower);
            _powerToggle.Toggled.AddListener(ToggleEnabling);
            ToggleEnabling(defaultPower);
        }

        private void LoadData()
        {
            _maxFrequency = InteractionSettings.MAX_FREQUENCY;
            _minFrequency = InteractionSettings.MIH_FREQUENCY;
            
            _maxPower = InteractionSettings.Data.MaxPower;
            _frequencyStep = InteractionSettings.Data.FrequencyStep;
            _powerStep = InteractionSettings.Data.PowerStep;
        }

        private void Update()
        {
            _currentFrequency = IsEnable ? Utils.RoundToIncrement(_frequencyHandle.GetValue(), _frequencyStep) : 0;
            _currentPower = IsEnable ? Utils.RoundToIncrement(_powerHandle.GetValue(), _powerStep) : 0;
            
            SendData();
        }
        
        public void ToggleEnabling(bool value)
        {
            _isEnable = value;
            
            if (IsEnable) 
                _powerMeshRenderer.material.EnableKeyword("_EMISSION");
            else _powerMeshRenderer.material.DisableKeyword("_EMISSION");
        }

        private void SendData()
        {
            _textFrequency.text = $"{Mathf.Round(_currentFrequency)}";

            _receiver.SendFrequency(_currentFrequency);
            _receiver.SendPowerFactor(_currentPower / (_maxPower * 0.5f));
        }
    }
}