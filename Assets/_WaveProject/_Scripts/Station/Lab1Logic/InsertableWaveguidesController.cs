using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveProject.Interaction;
using WaveProject.Services;
using WaveProject.Station.PlateLogic.Plates;
using WaveProject.Station.PlateLogic;
using WaveProject.Station;
using WaveProject.UI;
using WaveProject.UserInput;

namespace WaveProject
{
    public class InsertableWaveguidesController : MonoBehaviour
    {
        [SerializeField] private Receiver _receiver;
        [SerializeField] private CarriageStation _carriageStation; 
        [SerializeField] private InsertUIView _insertUiView;
        [SerializeField] private RemoveInsertUIView _removeInsertUIView;

        [Space]
        [SerializeField] private Transform _insertPosition;
        [SerializeField] private GameObject _movingBody;

        [Space]
        [SerializeField] private InteractableButton _selectWaveguidesButton;


        [SerializeField] private float _moveDuration = 1;

        private RoutineService _routines;
        private const float _UI_SHOW_WAIT_TIME = 1f;

        private InputController _inputController;
        

        private Vector3 currentPosition;
        private bool inserted = false;

        public void Init()
        {
            _insertUiView.Init(InsertIntoLong, InsertIntoCross, Back);
            _removeInsertUIView.AddListener(RemoveInsert);
            
            _selectWaveguidesButton.Init();
            _selectWaveguidesButton.Clicked.AddListener(SelectInsert);


            if (ServiceManager.TryGetService(out RoutineService routines)) _routines = routines;
            if (ServiceManager.TryGetService(out InputController inputController)) _inputController = inputController;
        }
        private void RemoveInsert()
        {
            _movingBody.transform.Rotate(Vector3.left, -90);
            SetPosition(currentPosition, _insertPosition.position);
            _movingBody.transform.SetParent(gameObject.transform);
            _selectWaveguidesButton.Clicked.AddListener(SelectInsert);
            _removeInsertUIView.gameObject.SetActive(false);
            _carriageStation.crossInsert = false;
            _carriageStation.longInsert = false;
        }

        private void SelectInsert()
        {
            _insertUiView.gameObject.SetActive(true);
        }

        private void InsertIntoLong()
        {
            currentPosition = _carriageStation._longCarriage.transform.position - new Vector3(0.0f, -0.67f, -0.3f);
            _movingBody.transform.Rotate(Vector3.left, 90);
            SetPosition(_insertPosition.position, currentPosition);
            _movingBody.transform.SetParent(_carriageStation._longCarriage.transform);
            _selectWaveguidesButton.Clicked.RemoveAllListeners();
            _insertUiView.gameObject.SetActive(false);
            _removeInsertUIView.gameObject.SetActive(true);
            _carriageStation.longInsert = true;
        }

        private void InsertIntoCross()
        {
            currentPosition = _carriageStation._crossCarriage.transform.position - new Vector3(0.0f, -0.67f, 0.94f);
            _movingBody.transform.Rotate(Vector3.left, 90);
            SetPosition(_insertPosition.position, currentPosition);
            _movingBody.transform.SetParent(_carriageStation._crossCarriage.transform);
            _selectWaveguidesButton.Clicked.RemoveAllListeners();
            _insertUiView.gameObject.SetActive(false);
            _removeInsertUIView.gameObject.SetActive(true);
            _carriageStation.crossInsert = true;
        }


        private void Back()
        {
            _insertUiView.gameObject.SetActive(false);
            _inputController.BlockUserInput(false);
        }

        protected virtual void SetPosition(Vector3 start, Vector3 end)
        {
            _movingBody.transform.position = Vector3.Slerp(start, end, _moveDuration);
        }
    }
}
