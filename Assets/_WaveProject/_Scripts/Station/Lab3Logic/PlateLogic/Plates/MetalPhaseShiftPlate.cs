using System;
using System.Numerics;
using UnityEngine;

namespace WaveProject.Station.PlateLogic.Plates
{
    class MetalPhaseShiftPlate : PhaseShiftPlate
    {
        public MetalPhaseShiftPlate(float plateLength, float plateThickness) : base(plateLength, plateThickness) { }

        public override double GetReceiverSignalLevel(float angleInRadians, double variantWavelength)
        {
            var betta = GetPolarizationCharacteristicInclinationAngle(variantWavelength);
            var r = GetEllipticityCoefficient(variantWavelength);

            var cosOfAngle = Math.Cos(angleInRadians - betta);
            return Math.Pow(Math.Abs(cosOfAngle) * (1 - r) + r, 2) * 100;
        }

        private double GetWavelength10(double variantWavelength)
        {
            var a = INTERNAL_WAVEGUIDE_WIDTH;

            var pow = Math.Pow(variantWavelength / (2 * a), 2);
            var sqrt = Math.Sqrt(1 - pow);
            return variantWavelength / sqrt;
        }

        // private double GetMinIdealPlateLength(double variantWavelength)
        // {
        //     var l10 = GetWavelength10(variantWavelength);
        //     var l01 = GetWavelength01(variantWavelength);
        //     return (l10*l01) / 4 * Math.Abs(l10 - l01);
        // }

        private double GetPolarizationCharacteristicInclinationAngle(double variantWavelength)
        {
            var a = GetA(variantWavelength);
            var shift = GetPhaseShift(variantWavelength);
                
            return 0.5d * Math.Atan((2 * a) / (a*a - 1) * Math.Cos(shift));
        }

        private double GetEllipticityCoefficient(double variantWavelength)
        {
            var a = GetA(variantWavelength);
            
            var shift = GetPhaseShift(variantWavelength);
            
            var absSinOfShift = Math.Abs(Math.Sin(shift));
            var powSinOfShift = Math.Pow(Math.Cos(shift), 2);

            var aPow4 = Math.Pow(a, 4);

            return (2*a * absSinOfShift) /
                   (a*a + 1 + Math.Sqrt(aPow4 - 2 * a*a + 1 + 4*a*a * powSinOfShift));
        }

        private double GetPhaseShift(double variantWavelength)
        {
            const double pi = Mathf.PI;
            var wavelength10 = GetWavelength10(variantWavelength);
            var wavelength01 = GetWavelength01(variantWavelength);

            return 2 * pi / wavelength10 * PlateLength - 2 * pi / wavelength01 * PlateLength;
        }

        private double GetA(double variantWavelength)
        {
            var g = GetG(variantWavelength);
            
            return Math.Sqrt(1 - Math.Pow(Complex.Abs(g), 2));
        }

        private double GetWavelength01(double variantWavelength)
        {
            var a = INTERNAL_WAVEGUIDE_WIDTH;

            var pow = Math.Pow(variantWavelength / 2 * (a - PlateThickness), 2);
            
            pow = Math.Clamp(pow, 0, .99999999999d); //TODO: may be wrong
            
            var sqrt = Math.Sqrt(1 - pow);
            return variantWavelength / sqrt;
        }
        
        private Complex GetG(double variantWavelength)
        {
            var z10 = GetZ10(variantWavelength);
            var zIn = GetZIn(variantWavelength);

            return
                (z10 - zIn) /
                (z10 + zIn);
        }

        private Complex GetZIn(double variantWavelength)
        {
            const double pi = Mathf.PI;

            var z01 = GetZ01(variantWavelength);
            var z10 = GetZ10(variantWavelength);

            var lambda01 = GetWavelength01(variantWavelength);

            var tanArg = 2 * pi * PlateLength / lambda01;

            var j = Complex.ImaginaryOne;
            
            return z01 * 
                   (z10 + j * z01 * Math.Tan(tanArg)) / 
                   (z01 + j * z10 * Math.Tan(tanArg));
        }

        private double GetZ10(double variantWavelength)
        {
            const double pi = Mathf.PI;
            var a = INTERNAL_WAVEGUIDE_WIDTH;

            var pow = Math.Pow(variantWavelength / (2 * a), 2);
            var sqrt = Math.Sqrt(1 - pow);
            
            return 120 * pi / sqrt;
        }

        private double GetZ01(double variantWavelength)
        {
            const double pi = Mathf.PI;
            var a = INTERNAL_WAVEGUIDE_WIDTH;

            var pow = Math.Pow(variantWavelength / (2 * (a - PlateThickness)), 2);

            pow = Math.Clamp(pow, 0, .99999999999d); //TODO: may be wrong
            
            var sqrt = Math.Sqrt(1 - pow);
            
            return 120 * pi / sqrt;
        }
    }
}