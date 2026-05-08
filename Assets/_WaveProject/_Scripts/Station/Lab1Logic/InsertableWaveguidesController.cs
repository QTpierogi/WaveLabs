using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Animations;
using WaveProject.Interaction;
using WaveProject.Services;
using WaveProject.Station;
using WaveProject.Station.PlateLogic;
using WaveProject.Station.PlateLogic.Plates;
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

        [Space]
        [SerializeField] private float _moveDuration = 1;
        [SerializeField] private float _rotateDuration = 1;

        [Space]
        [SerializeField] private Transform _midpointA;
        [SerializeField] private Transform _midpointB;

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
            StartCoroutine(RotateInsert(-90));
            StartCoroutine(CurvedMovementAnimation(currentPosition, _midpointB.position, _midpointA.position, _insertPosition.position, 
                gameObject.transform, false, false));
        }

        private void SelectInsert()
        {
            _insertUiView.gameObject.SetActive(true);
        }

        private void InsertIntoLong()
        {
            currentPosition = _carriageStation._longCarriage.transform.position - new Vector3(0.0f, -0.67f, -0.3f);
            StartCoroutine(RotateInsert(90));
            StartCoroutine(CurvedMovementAnimation(_insertPosition.position, _midpointA.position, _midpointB.position, currentPosition, 
                _carriageStation._longCarriage.transform, true, false));
        }

        private void InsertIntoCross()
        {
            currentPosition = _carriageStation._crossCarriage.transform.position - new Vector3(0.0f, -0.67f, 0.94f);
            StartCoroutine(RotateInsert(90));
            StartCoroutine(CurvedMovementAnimation(_insertPosition.position, _midpointA.position, _midpointB.position, currentPosition,
                _carriageStation._crossCarriage.transform, true, true));
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

        private Vector3 QuadraticLerp(Vector3 start, Vector3 midpoint, Vector3 end, float interpolation)
        {
            Vector3 ab = Vector3.Lerp(start, midpoint, interpolation);
            Vector3 bc = Vector3.Lerp(midpoint, end, interpolation);

            return Vector3.Lerp(ab, bc, interpolation);
        }

        private Vector3 CubicLerp(Vector3 start, Vector3 midpointA, Vector3 midpointB, Vector3 end, float interpolation)
        {
            Vector3 ab = QuadraticLerp(start, midpointA, midpointB, interpolation);
            Vector3 bc = QuadraticLerp(midpointA, midpointB, end, interpolation);

            return Vector3.Lerp(ab, bc, interpolation);
        }

        private IEnumerator CurvedMovementAnimation(Vector3 Start, Vector3 midpointA, Vector3 midpointB, Vector3 End, Transform newParent,
            bool inserting, bool crossSection)
        {
            if(inserting)
                _insertUiView.gameObject.SetActive(false);
            else
            {
                _removeInsertUIView.gameObject.SetActive(false);
                _carriageStation.crossInsert = false;
                _carriageStation.longInsert = false;
            }
            float elapsedTime = 0;
            float progress = 0;
            while(progress <=1)
            {
                _movingBody.transform.position = CubicLerp(Start, midpointA, midpointB, End, progress);
                elapsedTime += Time.unscaledDeltaTime;
                progress = elapsedTime / _moveDuration;
                yield return null;
            }
            _movingBody.transform.position = End;
            _inputController.BlockUserInput(false);
            _movingBody.transform.parent = newParent;
            if(inserting)
            {
                _selectWaveguidesButton.Clicked.RemoveAllListeners();
                _removeInsertUIView.gameObject.SetActive(true);
                if (crossSection)
                    _carriageStation.crossInsert = true;
                else _carriageStation.longInsert = true;
            }
            else _selectWaveguidesButton.Clicked.AddListener(SelectInsert);
        }

        private IEnumerator RotateInsert(float angle)
        {
            Quaternion startRotation = _movingBody.transform.rotation;
            float elapsedTime = 0;
            float progress = 0;
            float waitTime = _moveDuration * 0.2f;
            yield return new WaitForSeconds(waitTime);
            while (progress <= 1)
            {
                Vector3 newEulerOffset = Vector3.left * angle * progress;
                _movingBody.transform.rotation = Quaternion.Euler(newEulerOffset) * startRotation;
                elapsedTime += Time.unscaledDeltaTime;
                progress = elapsedTime / _rotateDuration;
                yield return null;
            }
        }
    }
}
