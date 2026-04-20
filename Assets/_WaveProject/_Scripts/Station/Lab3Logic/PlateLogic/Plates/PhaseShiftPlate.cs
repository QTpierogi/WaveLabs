using WaveProject.Configs;

namespace WaveProject.Station.PlateLogic.Plates
{
    public abstract class PhaseShiftPlate
    {
        protected readonly float PlateLength;
        protected readonly float PlateThickness;
        
        protected const double INTERNAL_WAVEGUIDE_WIDTH = InteractionSettings.INTERNAL_WAVEGUIDE_WIDTH;
        private const double _SPEED_OF_LIGHT = InteractionSettings.SPEED_OF_LIGHT;

        protected PhaseShiftPlate(float plateLength, float plateThickness)
        {
            PlateLength = plateLength;
            PlateThickness = plateThickness;
        }

        public double GetVariantWavelength(double frequency)
        {
            return _SPEED_OF_LIGHT / frequency;
        }

        public abstract double GetReceiverSignalLevel(float angleInRadians, double variantWavelength);
    }
}