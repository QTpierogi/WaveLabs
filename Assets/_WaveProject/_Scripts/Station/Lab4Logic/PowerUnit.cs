using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using WaveProject.Configs;
using WaveProject.Interaction;
using WaveProject.Services;
using WaveProject.UserInput;
using WaveProject.Utility;

namespace WaveProject
{
    public class PowerUnit : MonoBehaviour
    {
        [SerializeField] private InfiniteRotateInteractable _rotatableAntenna;

        [SerializeField] private Toggle _powerToggle;
        [SerializeField] private MeshRenderer _powerMeshRenderer;
        [SerializeField] private RotateInteractable _powerHandle;

        [Min(0), SerializeField] private float _defaultPower = 0;
        [Min(0), SerializeField] private float _maxPower = 1;
        [Min(0), SerializeField] private float _minPower = 0;

        [Space]
        [SerializeField] private TMP_Text _powerText;

        [Space]
        [SerializeField] private float _arrowAngleRange = 70;
        [SerializeField] private Transform _arrow;
        [SerializeField] private int _arrowSpeedFactor = 1;
        [SerializeField] private float _arrowSpeedToTarget = 1;

        [Space]
        [SerializeField] private InteractableButton _wireChanger;
        [SerializeField] private ChangeWiresUIView _changeWireUiView;
        [SerializeField] private Lab4Calculation calculator;

        [Space]
        [SerializeField] public float animationTime = 2f;
        [SerializeField] public List<GameObject> GeneratorWirePath;
        [SerializeField] public List<GameObject> ReceiverWirePath;
        [SerializeField] public GameObject GeneratorWire;
        [SerializeField] public GameObject ReceiverWire;

        private Sequence _animationSequence;
        private bool _inProcess;

        private float _currentPower;

        private bool _isEnable;        

        public bool IsEnable => _isEnable;

        private float startPolarizationAngle;
        private float polarizationAngle;

        private InputController _inputController;

        public void Init()
        {
            const bool defaultPower = false;

            LoadData();

            startPolarizationAngle = Random.Range(110, 130);

            _rotatableAntenna.Init();
            
            var angle = Random.Range(-90, 90);
            _rotatableAntenna.SetDefaultRotation(angle);

            _wireChanger.Init();
            _wireChanger.Clicked.AddListener(OpenChangeWiresMenu);

            _changeWireUiView.Init(ChangeWires, Cancel);

            _powerHandle.Init();
            _powerHandle.SetDefaultValue(_defaultPower, 0f, _maxPower);

            _powerToggle.Init();
            _powerToggle.SetDefaultToggledState(defaultPower);
            _powerToggle.Toggled.AddListener(ToggleEnabling);
            ToggleEnabling(defaultPower);

            if (ServiceManager.TryGetService(out InputController inputController)) _inputController = inputController;
            StartCoroutine(AimForResultValue());

            GeneratorWirePath.Reverse();
            ReceiverWirePath.Reverse();
        }

        private void LoadData()
        {
            _maxPower = InteractionSettings.Data.MaxElectricPower;
        }

        public void Update()
        {
            _arrowSpeedFactor = _isEnable ? 1 : 2;
            _currentPower = IsEnable ? _powerHandle.GetValue() : 0;

            polarizationAngle = startPolarizationAngle + 100 * Mathf.Pow((_currentPower/(_currentPower + 0.15f)), 1.25f);

            _powerText.text = $"{_currentPower.ToString("0.00")}";
        }

        public void ToggleEnabling(bool value)
        {
            _isEnable = value;

            if (IsEnable)
                _powerMeshRenderer.material.EnableKeyword("_EMISSION");
            else _powerMeshRenderer.material.DisableKeyword("_EMISSION");
        }

        public float GetRotation() => _rotatableAntenna.GetRotation();

        public float GetPolarizationAngle() => polarizationAngle;

        private IEnumerator AimForResultValue()
        {
            float currentValue = 0;

            while (true)
            {
                var currentTarget = _currentPower;

                currentValue = Mathf.Lerp(currentValue, currentTarget, _arrowSpeedFactor * _arrowSpeedToTarget * Time.deltaTime);

                _arrow.rotation = Utils.GetRotationInRange(currentValue, _minPower, _maxPower, -_arrowAngleRange + _minPower, _arrowAngleRange, Vector3.right);

                yield return null;
            }
        }

        public void OpenChangeWiresMenu()
        {
            _changeWireUiView.gameObject.SetActive(true);
        }

        private void ChangeWires()
        {
            if (_inProcess) return;
            _changeWireUiView.gameObject.SetActive(false);
            _inProcess = true;

            calculator.reversed = !calculator.reversed;
            GeneratorWirePath.Reverse();
            ReceiverWirePath.Reverse();

            _animationSequence?.Kill();
            _animationSequence = DOTween.Sequence();

            var generatorPath = GeneratorWirePath.Select(a => a.transform.position).ToArray();
            var receiverPath = ReceiverWirePath.Select(a => a.transform.position).ToArray();
            _animationSequence.Insert(0, GeneratorWire.transform.DOPath(generatorPath ,animationTime, PathType.CatmullRom, PathMode.Full3D));
            _animationSequence.Insert(0, ReceiverWire.transform.DOPath(receiverPath, animationTime, PathType.CatmullRom, PathMode.Full3D));

            _animationSequence.OnKill(() => _inProcess = false);
        }

        private void Cancel()
        {
            _changeWireUiView.gameObject.SetActive(false);
            _inputController.BlockUserInput(false);
        }
    }
}
