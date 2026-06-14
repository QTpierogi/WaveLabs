using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveProject
{
    public class Lab4Calculation : Calculator
    {
        [SerializeField] PowerUnit powerUnit;
        public bool reversed;

        private float betta = 0;
        private float phi = 0;
        private float ferriteDiff;

        public override void Init()
        {
            ferriteDiff = Random.Range(10, 20);
        }

        public override float CalculateValue(float PowerFactor, float Frequency)
        {
            betta = powerUnit.GetPolarizationAngle();
            phi = powerUnit.GetRotation();
            float value = 0f;
            if(reversed)
                value = Mathf.Abs(Mathf.Cos(phi - betta)) * Mathf.Pow(10, ferriteDiff / 20) * PowerFactor;
            else value = Mathf.Abs(Mathf.Cos(phi - betta)) * PowerFactor;
            return value * value / 100;
        }
    }
}
