using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveProject.Utility;

namespace WaveProject
{
    public class Lab2Calculation : Calculator
    {
        [SerializeField] private RotatableStation _rotatableStation;

        [Space]
        [SerializeField] private float waveSpeed;
        [SerializeField] private float waveSupression;
        [SerializeField] private float waveguideRadius;
        [SerializeField] private float basePistonOffset;

        [Space]
        [SerializeField] public float waveLength;
        [SerializeField] public float secondaryWaveLength;
        [SerializeField] public float closedWaveLength;

        public bool _isConnectionSmooth;

        public override void Init()
        {
            _isConnectionSmooth = true;
        }

        public override float CalculateValue(float PowerFactor, float Frequency)
        {
            waveLength = waveSpeed / Utils.MHzToHz(Frequency);

            closedWaveLength = waveLength / Mathf.Sqrt(1 - Mathf.Pow((waveLength / (3.413f * waveguideRadius)), 2));

            secondaryWaveLength = waveLength / Mathf.Sqrt(1 - Mathf.Pow((waveLength / (2.613f * waveguideRadius)), 2));
            
            var zOffset = _rotatableStation.GetPistonOffset() + basePistonOffset;
            var f = Mathf.Sin(_rotatableStation.GetRotation() * Mathf.Deg2Rad) * Mathf.Sin(zOffset * 2 * Mathf.PI / closedWaveLength);

            if (!_isConnectionSmooth)
                f = f * waveSupression + Mathf.Sin(zOffset * 2 * Mathf.PI / secondaryWaveLength);

            var value = PowerFactor * Mathf.Abs(f);
            return value * value;
        }
    }
}
