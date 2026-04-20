using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveProject
{
    public abstract class Calculator: MonoBehaviour
    {
        public abstract void Init();

        public abstract float CalculateValue(float defaultScaleFactor, float PowerFactor, float Frequency);
    }
}
