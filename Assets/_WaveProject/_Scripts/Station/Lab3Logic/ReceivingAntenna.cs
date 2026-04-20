using System;
using TMPro;
using UnityEngine;
using WaveProject.Interaction;
using Random = UnityEngine.Random;

namespace WaveProject.Station
{
    public class ReceivingAntenna : MonoBehaviour
    {
        [SerializeField] private float _baseDistance = 4;
        [SerializeField] private MoveBetweenPointsInteractable _secondAntenna;
        
        [SerializeField] private Transform _thisAntennaDistancePoint;
        [SerializeField] private Transform _secondAntennaDistancePoint;
        
        [SerializeField] private InfiniteRotateInteractable _rotatePart;
        [SerializeField] private TMP_Text _rotationText;
        
        public float PowerFactor { get; private set; }
        public float Frequency { get; private set; }
        public bool GeneratorEnabled => Frequency != 0;

        private void OnValidate()
        {
            _baseDistance = GetPointsDistance();
        }

        public void Init()
        {
            _secondAntenna.Init();
            _secondAntenna.SetDefaultPosition(Random.Range(0, 1f));
            _secondAntenna.SetDefaultValue();
            
            _rotatePart.Init();
            
            var angle = Random.Range(-90, 90);
            _rotatePart.SetDefaultRotation(angle);
        }

        private void Update()
        {
            _rotationText.text = $"{MathF.Round(GetRotation())}";
        }

        public float GetAntennasDistanceFactor()
        {
          return 1 * _baseDistance / Mathf.Pow(GetPointsDistance(), 2);
        }

        private float GetPointsDistance()
        {
            return Vector3.Distance(_thisAntennaDistancePoint.position, _secondAntennaDistancePoint.position);
        }

        public float GetRotation() => _rotatePart.GetRotation();

        public void SendPowerFactor(float power)
        {
            PowerFactor = power;
        }

        public void SendFrequency(float frequency)
        {
            Frequency = frequency;
        }
    }
}