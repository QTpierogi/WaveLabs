using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using WaveProject.Configs;
using WaveProject.Utility;

namespace WaveProject.UserInput
{
    public class FovChanger : MonoBehaviour, IInputSubscriber
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        
        private float _defaultFov;
        private float _fovChangingSpeed;
        private float _fovMinValue;
        
        private float _fovTarget;
        private float _fovSensitivity;

        public event Action ChangingFinished;

        private void Start()
        {
            LoadData();
            
            _camera.m_Lens.FieldOfView = _defaultFov;
            _fovTarget = _defaultFov;
            
            StartCoroutine(ChangeFovRoutine());
        }

        private void LoadData()
        {
            _fovMinValue = InteractionSettings.Data.FovMinValue;
            _defaultFov = InteractionSettings.Data.FovDefaultValue;
            _fovSensitivity = InteractionSettings.Data.FovSensitivity;
            
            _fovChangingSpeed = InteractionSettings.Data.FovChangingSpeed;
        }

        public void Enable()
        {
        }

        public void Disable()
        {
        }

        public void CustomUpdate(Vector2 _)
        {
            if (Input.mouseScrollDelta.y == 0) return;

            _fovTarget -= Input.mouseScrollDelta.y * _fovSensitivity;
            _fovTarget = Mathf.Clamp(_fovTarget, _fovMinValue, _defaultFov);
        }

        private IEnumerator ChangeFovRoutine()
        {
            yield return new WaitUntil(() => Utils.IsAlmostEqual(_fovTarget, _defaultFov, .01));
            
            while (true)
            {
                var newFieldOfView = Mathf.Lerp(_camera.m_Lens.FieldOfView, _fovTarget,
                    Easings.OutCirc(Time.deltaTime * _fovChangingSpeed));

                _camera.m_Lens.FieldOfView = newFieldOfView;

                if (Utils.IsAlmostEqual(_camera.m_Lens.FieldOfView, _fovTarget, .01))
                {
                    _camera.m_Lens.FieldOfView = _fovTarget;

                    var currentFovTarget = _fovTarget;
                    yield return new WaitUntil(() => Utils.IsAlmostEqual(_fovTarget, currentFovTarget, .01));
                }

                yield return null;
            }
        }
    }
}