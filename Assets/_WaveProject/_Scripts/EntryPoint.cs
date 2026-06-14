using UnityEngine;
using WaveProject.Extensions;
using WaveProject.Services;
using WaveProject.Station;
using WaveProject.Station.PlateLogic;
using WaveProject.UserInput;

namespace WaveProject
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private CameraDirectionSetter _directionSetter;
        [SerializeField] private FovChanger _fovChanger;
        [SerializeField] private CameraMover _cameraMover;

        [SerializeField] private Generator _generator;
        [SerializeField] private Receiver _receiver;
        [SerializeField] private ReceivingAntenna _receivingAntenna;
        [SerializeField] private PlateGenerator _plateGenerator;
        [SerializeField] private CarriageStation _carriageStation;
        [SerializeField] private InsertableWaveguidesController _insertableWaveguidesController;
        [SerializeField] private RotatableStation _rotatableStation;
        [SerializeField] private PowerUnit _powerUnit;

        private bool useExtraHandles = false;

        private void Awake()
        {
            var routineService = this.Get<RoutineService>();
            ServiceManager.TryAddService(routineService);

            var input = this.Get<InputController>();
            input.SetCamera(Camera.main);
            input.SetCameraDirectionMover(_directionSetter);
            input.SetFovChanger(_fovChanger);
            input.SetCameraMover(_cameraMover);
            
            ServiceManager.TryAddService(input);

            if (_powerUnit != null)
            {
                _powerUnit.Init();
                useExtraHandles = true;

            }
            else useExtraHandles = false;
            _generator.Init(useExtraHandles);
            _receiver.Init(useExtraHandles);
            if(_receivingAntenna!=null)_receivingAntenna.Init();
            if(_plateGenerator!=null)_plateGenerator.Init();
            if(_carriageStation!=null)_carriageStation.Init();
            if(_insertableWaveguidesController!=null)_insertableWaveguidesController.Init();
            if(_rotatableStation!=null)_rotatableStation.Init();    
        }

        private void OnDestroy()
        {
            ServiceManager.ClearServices();
        }
    }
}