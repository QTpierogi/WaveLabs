using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WaveProject.Interaction;
using WaveProject.Services;
using WaveProject.UI;
using WaveProject.UserInput;

namespace WaveProject
{
    public class RotatableStation : MonoBehaviour
    {
        [SerializeField] private MoveBetweenPointsInteractable _piston;
        [SerializeField] private TMP_Text _pistonText;
        [SerializeField] private InfiniteRotateInteractable _rotatablePart;
        [SerializeField] private TMP_Text _rotationText;
        [SerializeField] private Lab2Calculation _calculator;

        [Space]
        [SerializeField] private ConnectorChoiceUIVIew _connectorUiView;
        [SerializeField] private InsertInteractable _smoothConnectorButton;
        [SerializeField] private InsertInteractable _steppedConnectorButton;

        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        private InputController _inputController;

        public InsertInteractable _currentInsert;
        private float _moveDuration = 1f;

        private RoutineService _routines;
        private const float _UI_SHOW_WAIT_TIME = 1f;

        public void Init()
        {
            _piston.Init();
            _piston.SetDefaultPosition(0.5f);
            _piston.SetDefaultValue();

            _rotatablePart.Init();

            _connectorUiView.Init(ChooseSmoothConnector, ChooseSteppedConnector, Back);

            _smoothConnectorButton.Init();
            _smoothConnectorButton.SetDefaultValue();
            _smoothConnectorButton.Clicked.AddListener(SelectConnector);

            _steppedConnectorButton.Init();
            _smoothConnectorButton.SetDefaultValue();
            _steppedConnectorButton.Clicked.AddListener(SelectConnector);

            _currentInsert = _smoothConnectorButton;

            if (ServiceManager.TryGetService(out RoutineService routines)) _routines = routines;

            if (ServiceManager.TryGetService(out InputController inputController)) _inputController = inputController;
        }

        private void Update()
        {
            _rotationText.text = $"{MathF.Round(GetRotation())}";
            _pistonText.text = $"{(GetPistonOffset() * 1000).ToString("0.00")}";
        }

        public float GetRotation() => _rotatablePart.GetRotation();

        public float GetPistonOffset() => Mathf.Abs(_piston.transform.position.z) - Mathf.Abs(_piston._leftPoint.position.z);

        private void SelectConnector()
        {
            if(_currentInsert._buttonMode)
            {//_connectorUiView.gameObject.SetActive(true);
                _inputController.BlockUserInput(true);

                _virtualCamera.gameObject.SetActive(true);
                _currentInsert._removing = true;
                _currentInsert._correctPoint = _currentInsert._leftPoint;

                _currentInsert.MoveUp(_moveDuration, OnInsertMoved);
            }
        }

        private void Back()
        {
            _connectorUiView.gameObject.SetActive(false);
            _virtualCamera.gameObject.SetActive(false);
            _inputController.BlockUserInput(false);
        }

        private void ChooseSmoothConnector()
        {
            _smoothConnectorButton.transform.parent.gameObject.SetActive(true);
            _steppedConnectorButton.transform.parent.gameObject.SetActive(false);
            _connectorUiView.gameObject.SetActive(false);
            _calculator._isConnectionSmooth = true;

            _currentInsert = _smoothConnectorButton;
            InsertingConnectorSetup(_currentInsert);
        }

        private void ChooseSteppedConnector()
        {
            _steppedConnectorButton.transform.parent.gameObject.SetActive(true);
            _smoothConnectorButton.transform.parent.gameObject.SetActive(false);
            _connectorUiView.gameObject.SetActive(false);
            _calculator._isConnectionSmooth = false;

            _currentInsert = _steppedConnectorButton;
            InsertingConnectorSetup(_currentInsert);
        }

        private void InsertingConnectorSetup(InsertInteractable connector)
        {
            connector._removing = false;
            connector._buttonMode = false;
            connector.transform.position = connector._leftPoint.position;
            connector._correctPoint = _currentInsert._rightPoint;
            _inputController.ExternSubscribe(_currentInsert);
            connector.ChangingFinished += FinishConnectorInserting;
        }

        private void OnInsertMoved()
        {
            _inputController.ExternSubscribe(_currentInsert);

            _currentInsert.ChangingFinished += FinishConnectorRemoving;
        }

        private void FinishConnectorInserting()
        {
            if (_currentInsert != null)
                _currentInsert.ChangingFinished -= FinishConnectorInserting;

            Debug.Log("Connector inserted");
            _currentInsert.MoveDown(_moveDuration);
            _virtualCamera.gameObject.SetActive(false);
            _currentInsert._buttonMode = true;
            _inputController.BlockUserInput(false);
        }

        private void FinishConnectorRemoving()
        {
            if (_currentInsert != null)
                _currentInsert.ChangingFinished -= FinishConnectorRemoving;
            Debug.Log("Connector removed");
            _currentInsert._removing = false;
            _connectorUiView.gameObject.SetActive(true);
        }
    }
}
