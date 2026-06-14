using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveProject.Station;
using WaveProject.Station.PlateLogic.Plates;
using WaveProject.Utility;

namespace WaveProject
{
    public class Lab3Calculation : Calculator
    {
        [SerializeField] 
        private ReceivingAntenna _receivingAntenna;
        
        private PhaseShiftPlate _phaseShiftPlate;

        public double value;

        public override void Init()
        {
            _phaseShiftPlate = new EmptyPhaseShiftPlate(0, 0);
        }

        public override float CalculateValue(float PowerFactor, float Frequency)
        {
            var distanceFactor = _receivingAntenna.GetAntennasDistanceFactor();
            var powerFactor = PowerFactor;

            var frequency = Frequency;

            var angleInDegree = _receivingAntenna.GetRotation();
            var angleInRadians = Utils.DegreeToRadians(angleInDegree);

            var variantWavelength = _phaseShiftPlate.GetVariantWavelength(Utils.MHzToHz(frequency));
            var receiverSignalLevel = _phaseShiftPlate.GetReceiverSignalLevel(angleInRadians, variantWavelength);

            value = distanceFactor * powerFactor * receiverSignalLevel;

            return (float)value;
        }

        public void SendFrequency(float frequency)
        {
            _receivingAntenna.SendFrequency(frequency);
        }

        public void SendPowerFactor(float power)
        {
            _receivingAntenna.SendPowerFactor(power);
        }

        public void SetPhaseShiftPlate(PlateType type, float plateLength = 0, float plateThickness = 0, float plateResistance = 0)
        {
            var plateLengthInMeters = Utils.MillimetersToMeters(plateLength);
            var plateThicknessInMeters = Utils.MillimetersToMeters(plateThickness);

            switch (type)
            {
                case PlateType.None:
                    _phaseShiftPlate = new EmptyPhaseShiftPlate(plateLengthInMeters, plateThicknessInMeters);
                    break;

                case PlateType.Metal:
                    _phaseShiftPlate = new MetalPhaseShiftPlate(plateLengthInMeters, plateThicknessInMeters);
                    break;

                case PlateType.Dielectric:
                    _phaseShiftPlate = new DielectricPhaseShiftPlate(plateLengthInMeters, plateThicknessInMeters, plateResistance);
                    break;
            }
        }
    }
}
