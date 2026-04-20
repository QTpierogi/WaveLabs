using System;
using Cinemachine;
using UnityEngine;

namespace WaveProject.UserInput
{
    public class CameraMover : MonoBehaviour, IInputSubscriber
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private Transform _rotatePoint;

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _maxAngle = 160;
        
        private float _currentAngle;
        private CinemachinePOV _povCamera;
        
        private (float min, float max) _cameraValueRange;
        private (float min, float max) _defaultValueRange;

        public event Action ChangingFinished;

        private void Start()
        {
            LoadData();

            _povCamera = _camera.GetCinemachineComponent<CinemachinePOV>();
            _cameraValueRange = (_povCamera.m_HorizontalAxis.m_MinValue, _povCamera.m_HorizontalAxis.m_MaxValue);
            _defaultValueRange = _cameraValueRange;
        }

        private void LoadData()
        {
        }

        public void Enable()
        {
        }

        public void Disable()
        {
        }

        public void CustomUpdate(Vector2 _)
        {
            var direction = 0f;

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) 
                direction = -1f;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) 
                direction = 1f;

            var angleToRotate = direction * _moveSpeed * Time.deltaTime;
            var newAngle = _currentAngle + angleToRotate;

            if (Mathf.Abs(newAngle) <= _maxAngle)
            {
                _camera.transform.RotateAround(_rotatePoint.position, Vector3.up, angleToRotate);
                _currentAngle = newAngle;

                var angleTime = (_currentAngle / _maxAngle + 1) / 2;
                
                var valueOffset = Mathf.Lerp(-_maxAngle, _maxAngle, angleTime);
                _cameraValueRange = (_defaultValueRange.min + valueOffset, _defaultValueRange.max + valueOffset);

                _povCamera.m_HorizontalAxis.m_MinValue = _cameraValueRange.min;
                _povCamera.m_HorizontalAxis.m_MaxValue = _cameraValueRange.max;
            }
        }
    }
}