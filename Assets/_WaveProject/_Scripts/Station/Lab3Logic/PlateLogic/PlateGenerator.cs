using Cinemachine;
using UnityEngine;
using WaveProject.Interaction;
using WaveProject.Services;
using WaveProject.Station.PlateLogic.Plates;
using WaveProject.UI;
using WaveProject.UserInput;

namespace WaveProject.Station.PlateLogic
{
    public class PlateGenerator : MonoBehaviour
    {
        [SerializeField] private Lab3Calculation _calculator;
        [SerializeField] private PlateUiView _plateUiView;
        [SerializeField] private RemovePlateUIView _removePlateUIView;
        [SerializeField] private Transform _moveAntenn;

        [Space] 
        [SerializeField] private InteractableButton _selectMetalButton;
        [SerializeField] private InteractableButton _selectDielectricButton;

        [Space] 
        [SerializeField] private float _lengthMinValue;
        [SerializeField] private float _lengthMaxValue;

        [SerializeField] private float _thicknessMinValue;
        [SerializeField] private float _thicknessMaxValue;

        [SerializeField] private float _resistanceMinValue;
        [SerializeField] private float _resistanceMaxValue;

        [Space] 
        [SerializeField] private Plate _metalPlate;
        [SerializeField] private Plate _dielectricPlate;

        [SerializeField] private float _moveDuration = 1;

        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        private float _length;
        private float _thickness;
        private float _resistance;

        private PlateType _plateType;
        private Plate _currentPlate;
        
        private RoutineService _routines;
        private const float _UI_SHOW_WAIT_TIME = 1f;

        private InputController _inputController;

        public void Init()
        {
            _plateUiView.Init(Show, Back,
                _lengthMinValue,
                _lengthMaxValue,
                _thicknessMinValue,
                _thicknessMaxValue,
                _resistanceMinValue,
                _resistanceMaxValue);

            _removePlateUIView.AddListener(RemovePlate);
            RemovePlate();
            
            _metalPlate.transform.parent = _moveAntenn;
            _dielectricPlate.transform.parent = _moveAntenn;

            _plateUiView.LengthChanged += OnLengthChanged;
            _plateUiView.ThicknessChanged += OnThicknessChanged;
            _plateUiView.ResistanceChanged += OnResistanceChanged;
            
            _selectMetalButton.Init();
            _selectDielectricButton.Init();

            _selectMetalButton.Clicked.AddListener(SelectMetalPlate);
            _selectDielectricButton.Clicked.AddListener(SelectDielectricPlate);

            if (ServiceManager.TryGetService(out RoutineService routines)) _routines = routines;
            if (ServiceManager.TryGetService(out InputController inputController)) _inputController = inputController;
        }

        private void OnDestroy()
        {
            _plateUiView.LengthChanged -= OnLengthChanged;
            _plateUiView.ThicknessChanged -= OnThicknessChanged;
            _plateUiView.ResistanceChanged -= OnResistanceChanged;
        }

        private void OnLengthChanged(float value) => _length = value;
        private void OnThicknessChanged(float value) => _thickness = value;
        private void OnResistanceChanged(float value) => _resistance = value;

        private void RemovePlate()
        {
            if (_currentPlate != null) 
                _currentPlate.Hide();
            
            _calculator.SetPhaseShiftPlate(PlateType.None);
            _removePlateUIView.gameObject.SetActive(false);
        }

        private void SelectMetalPlate()
        {
            RemovePlate();
            
            _inputController.BlockUserInput(true);
            
            _virtualCamera.gameObject.SetActive(true);
            
            _plateType = PlateType.Metal;
            
            _routines.WaitTime(_UI_SHOW_WAIT_TIME, this, () =>
            {
                _plateUiView.gameObject.SetActive(true);
                _plateUiView.SelectMetalPlate();
            });
        }

        private void SelectDielectricPlate()
        {
            RemovePlate();
            
            _inputController.BlockUserInput(true);
            
            _virtualCamera.gameObject.SetActive(true);
            
            _plateType = PlateType.Dielectric;
            
            _routines.WaitTime(_UI_SHOW_WAIT_TIME, this, () =>
            {
                _plateUiView.gameObject.SetActive(true);
                _plateUiView.SelectDielectricPlate();
            });
        }

        private void Show()
        {
            _calculator.SetPhaseShiftPlate(_plateType, _length, _thickness, _resistance);
            
            _currentPlate = _plateType == PlateType.Metal ? _metalPlate : _dielectricPlate;
            _currentPlate.gameObject.SetActive(true);

            _currentPlate.MoveToAntenna(_moveDuration, OnPlateMoved);
            
            _plateUiView.gameObject.SetActive(false);
        }

        private void OnPlateMoved()
        {
            _currentPlate.Init();
            _inputController.ExternSubscribe(_currentPlate.MovementInteractable);

            _currentPlate.MovementInteractable.ChangingFinished += FinishPlateInserting;
        }

        private void Back()
        {
            _plateUiView.gameObject.SetActive(false);
            _virtualCamera.gameObject.SetActive(false);
            _inputController.BlockUserInput(false);
        }

        private void FinishPlateInserting()
        {
            Debug.Log("Plate moved");
            if (_currentPlate != null) 
                _currentPlate.MovementInteractable.ChangingFinished -= FinishPlateInserting;

            _virtualCamera.gameObject.SetActive(false);
            _inputController.BlockUserInput(false);
            
            _removePlateUIView.gameObject.SetActive(true);
        }
    }
}