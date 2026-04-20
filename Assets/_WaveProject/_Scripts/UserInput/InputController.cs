using System.Collections.Generic;
using UnityEngine;
using WaveProject.Interaction;
using WaveProject.Services;

namespace WaveProject.UserInput
{
    public class InputController : MonoBehaviour, IService
    {
        private readonly List<IInputSubscriber> _infiniteSubscribers = new();
        
        private CameraDirectionSetter _cameraDirectionSetter;
        
        private IInputSubscriber _currentSubscriber;
        private ISelectable _currentPotentialSubscriber;

        private Vector2 _previousMousePosition;
        private Camera _camera;

        private static Vector2 CurrentMousePosition => Input.mousePosition;
        private static Vector2 Delta => new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        private const float _RAYCAST_DISTANCE = 1000;

        public bool IsUserInputBlocked { get; private set; }
        public bool IsMovementBlocked { get; private set; }

        private void Update()
        {
            CustomUpdate();

            if (IsUserInputBlocked)
                return;
            
            FindOutliner();
            TrySubscribe();
        }

        public void SetCamera(Camera cam)
        {
            _camera = cam;
            Debug.Log("Camera was set");
        }

        public void SetCameraDirectionMover(CameraDirectionSetter cameraDirectionSetter)
        {
            _cameraDirectionSetter = cameraDirectionSetter;
            Subscribe(_cameraDirectionSetter);
        }

        public void SetFovChanger(FovChanger fovChanger)
        {
            _infiniteSubscribers.Add(fovChanger);
        }

        public void SetCameraMover(CameraMover cameraMover)
        {
            _infiniteSubscribers.Add(cameraMover);
        }

        public void ExternSubscribe(IInputSubscriber externSubscriber)
        {
            if (externSubscriber is ISelectable selectable)
                SetOutline(selectable);
            else ClearOutline();
            Debug.Log("Extern Subscribed");
            Subscribe(externSubscriber);
        }

        public void BlockUserInput(bool value)
        {
            IsUserInputBlocked = value;
            Debug.Log("User Input mode changed");
            ReturnToCameraHandler();
            ClearOutline();
        }

        public void BlockMovement(bool value)
        {
            IsMovementBlocked = value;
        }

        private void CustomUpdate()
        {
            _currentSubscriber.CustomUpdate(Delta);
            
            if (IsMovementBlocked)
                return;
            
            _infiniteSubscribers.ForEach(s => s.CustomUpdate(Delta));
        }

        private void FindOutliner()
        {
            if (_currentSubscriber is not CameraDirectionSetter)
                return;
            
            var ray = _camera.ScreenPointToRay(CurrentMousePosition);

            if (Physics.Raycast(ray, out var hit, _RAYCAST_DISTANCE,IInputSubscriber.LayerMask))
            {
                if (hit.collider.TryGetComponent(out ISelectable selectable))
                {
                    SetOutline(selectable);
                }
                else
                {
                    ClearOutline();
                }
            }
            else
            {
                ClearOutline();
            }
        }

        private void ClearOutline()
        {
            _currentPotentialSubscriber?.Deselect();
            _currentPotentialSubscriber = null;
        }

        private void SetOutline(ISelectable selectable)
        {
            _currentPotentialSubscriber?.Deselect();
            _currentPotentialSubscriber = selectable;
            _currentPotentialSubscriber.Select();
        }

        private void TrySubscribe()
        {
            if (Input.GetMouseButtonDown(0) == false) return;
            
            if (_currentPotentialSubscriber is IInputSubscriber subscriber)
            {
                Subscribe(subscriber);
            }
        }

        private void Subscribe(IInputSubscriber subscriber)
        {
            _currentSubscriber?.Disable();
            _currentSubscriber = subscriber;
            _currentSubscriber.Enable();
            
            _currentSubscriber.ChangingFinished += ReturnToCameraHandler;
        }

        private void ReturnToCameraHandler()
        {
            _currentSubscriber.ChangingFinished -= ReturnToCameraHandler;
            Subscribe(_cameraDirectionSetter);
        }
    }
}