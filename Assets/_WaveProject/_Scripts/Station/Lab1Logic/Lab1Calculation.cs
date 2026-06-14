using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using WaveProject.Utility;

namespace WaveProject
{
    public class Lab1Calculation : Calculator
    {
        [SerializeField] private CarriageStation _carriageStation;

        [Space]
        [SerializeField] private float waveSpeed;
        [SerializeField] public float waveLength;
        [SerializeField] public float closedWaveLength;


        private float xDistance;
        private float zDistance;

        public override void Init()
        {
            
        }

        public override float CalculateValue(float PowerFactor, float Frequency)
        {
            waveLength = waveSpeed / Utils.MHzToHz(Frequency);

            closedWaveLength = waveLength / Mathf.Sqrt(1 - Mathf.Pow((waveLength / (2f * _carriageStation.ConstStandWidth)), 2f));

            var fX = 0f;
            var fZ = 0f;

            if (_carriageStation.crossInsert)
            {
                fZ = Mathf.Abs(Mathf.Sin(zDistance * 2 * Mathf.PI / closedWaveLength));
                if (_carriageStation.loopInsert)
                {
                    fX = Mathf.Abs(Mathf.Cos(xDistance * Mathf.PI / _carriageStation.ConstStandWidth));
                }
                else
                {
                    fX = Mathf.Abs(Mathf.Sin(xDistance * Mathf.PI / _carriageStation.ConstStandWidth));
                }
            }
            else if (_carriageStation.longInsert)
            {
                fX = 1;
                if (_carriageStation.loopInsert)
                {
                    fZ = Mathf.Abs(Mathf.Cos(zDistance * 2 * Mathf.PI / closedWaveLength));
                }
                else
                {
                    fZ = Mathf.Abs(Mathf.Sin(zDistance * 2 * Mathf.PI / closedWaveLength));
                }
            }

            var value = PowerFactor * fX * fZ;

            return value * value;
        }

        public void SendX(float x)
        {
            xDistance = x;
        }

        public void SendZ(float z)
        {
            zDistance = z;
        }
    }
}
