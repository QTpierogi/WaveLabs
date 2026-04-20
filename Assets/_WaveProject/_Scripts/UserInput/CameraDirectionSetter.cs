using System;
using Cinemachine;
using UnityEngine;
using WaveProject.Configs;

namespace WaveProject.UserInput
{
    public class CameraDirectionSetter : MonoBehaviour, IInputSubscriber
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        
        private Vector3? _moveTargetPosition;
        private float _moveSpeed;
        private CinemachinePOV _povCamera;

        public event Action ChangingFinished;

        private void Start()
        {
            LoadData();
            _povCamera = _camera.GetCinemachineComponent<CinemachinePOV>();
        }

        private void LoadData() => _moveSpeed = InteractionSettings.Data.CameraMoveSpeed;

        public void Enable(){ }

        public void Disable(){ }

        public void CustomUpdate(Vector2 delta)
        {
            if (Input.GetMouseButton(1))
            {
                _povCamera.m_HorizontalAxis.Value += delta.x * _moveSpeed;
                _povCamera.m_VerticalAxis.Value -= delta.y * _moveSpeed;
            }
        }
    }
}