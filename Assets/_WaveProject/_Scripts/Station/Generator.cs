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

        [Header("DB weakening settings")]
        [Min(0), SerializeField] private float _defaultDBWeakening = 30f;
        [SerializeField] private RotateInteractable _DBWeakeningHandle;
        [SerializeField] private TMP_Text _textDBWeakening;

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

        private float _minDBWeakening;
        private float _maxDBWeakening;
        private float _DBWeakeningStep;
        private float _currentDBWeakening;

        private float _maxPower;
        private float _powerStep;
        private float _currentPower;
        private bool _isEnable;
        private bool DBhandleIsActive = false;

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

        public void Init(bool useExtraHandles)
        {
            const bool defaultPower = false;
            
            LoadData();
            
            _frequencyHandle.Init();
            _frequencyHandle.SetDefaultValue(_defaultFrequency, _minFrequency, _maxFrequency);           
            
            
            _powerToggle.Init();
            _powerToggle.SetDefaultToggledState(defaultPower);
            _powerToggle.Toggled.AddListener(ToggleEnabling);
            ToggleEnabling(defaultPower);

            if(useExtraHandles)
            {
                DBhandleIsActive = true;
                _DBWeakeningHandle.Init();
                _DBWeakeningHandle.SetDefaultValue(20, _minDBWeakening, _maxDBWeakening);
            }
            else
            {
                _powerHandle.Init();
                _powerHandle.SetDefaultValue(_defaultPower, 0, _maxPower);
            }
        }

        private void LoadData()
        {
            _maxFrequency = InteractionSettings.MAX_FREQUENCY;
            _minFrequency = InteractionSettings.MIH_FREQUENCY;

            _minDBWeakening = InteractionSettings.MIN_WEAKENING;
            _maxDBWeakening = InteractionSettings.MAX_WEAKENING;

            _maxPower = InteractionSettings.Data.MaxPower;
            _frequencyStep = InteractionSettings.Data.FrequencyStep;
            _DBWeakeningStep = InteractionSettings.Data.WeakeningStep;
            _powerStep = InteractionSettings.Data.PowerStep;
        }

        private void Update()
        {
            _currentFrequency = IsEnable ? Utils.RoundToIncrement(_frequencyHandle.GetValue(), _frequencyStep) : 0;
            _currentPower = IsEnable ? Utils.RoundToIncrement(_powerHandle.GetValue(), _powerStep) : 0;
            _currentDBWeakening = IsEnable ? Utils.RoundToIncrement(_DBWeakeningHandle.GetValue(), _DBWeakeningStep) : 0;
            
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
            _textDBWeakening.text = $"{Mathf.Round(_currentDBWeakening)}";

            _receiver.SendFrequency(_currentFrequency);
            if (DBhandleIsActive)
                _receiver.SendPowerFactor(_currentDBWeakening);
            else _receiver.SendPowerFactor(_currentPower * 2 / (_maxPower));
        }
    }
}