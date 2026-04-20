using System;
using WaveProject.Utility;

namespace WaveProject.Station.PlateLogic.Plates
{
    internal class EmptyPhaseShiftPlate : PhaseShiftPlate
    {
        public EmptyPhaseShiftPlate(float plateLength, float plateThickness) : base(plateLength, plateThickness)
        {
        }

        public override double GetReceiverSignalLevel(float angleInRadians, double variantWavelength)
        {
            var betta = Utils.DegreeToRadians(0);
            const float r = 0.0f;

            var cosOfAngle = Math.Cos(angleInRadians - betta);
            return Math.Pow(Math.Abs(cosOfAngle) * (1 - r) + r, 2) * 100;
        }
    }
}