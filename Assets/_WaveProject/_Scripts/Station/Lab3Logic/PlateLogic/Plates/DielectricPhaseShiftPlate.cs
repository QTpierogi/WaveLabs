using System;
using System.Numerics;
using UnityEngine;

namespace WaveProject.Station.PlateLogic.Plates
{
    internal class DielectricPhaseShiftPlate : PhaseShiftPlate
    {
        private readonly double _resistance;

        public DielectricPhaseShiftPlate(float plateLength, float plateThickness, double resistance) : base(plateLength, plateThickness)
        {
            _resistance = resistance;
        }

        public override double GetReceiverSignalLevel(float angleInRadians, double variantWavelength)
        {
            var betta = GetPolarizationCharacteristicInclinationAngle(variantWavelength);
            var r = GetEllipticityCoefficient(variantWavelength);

            var cosOfAngle = Math.Cos(angleInRadians - betta);
            return Math.Pow(Math.Abs(cosOfAngle) * (1 - r) + r, 2) * 100;
        }
        
        private double GetWavelength01(double variantWavelength)
        {
            var a = INTERNAL_WAVEGUIDE_WIDTH;

            var pow = Math.Pow(variantWavelength / (2 * a), 2);
            var sqrt = Math.Sqrt(1 - pow);
            return variantWavelength / sqrt;
        }
        
        private double GetWavelength10(double variantWavelength)
        {
            var a = INTERNAL_WAVEGUIDE_WIDTH;
            var t = PlateThickness;
            var l01 = GetWavelength01(variantWavelength);
            var l = variantWavelength;

            return 1 / 
                   ((_resistance - 1) * ((t * l01) / (a * l*l)) + (1 / l01));
        }

        // private double GetMinIdealPlateLength(double variantWavelength)
        // {
        //     var l10 = GetWavelength10(variantWavelength);
        //     var l01 = GetWavelength01(variantWavelength);
        //     return (l10*l01) / 4 * Math.Abs(l10 - l01);
        // }

        private double GetPolarizationCharacteristicInclinationAngle(double variantWavelength)
        {
            var a = GetA(variantWavelength, PlateLength, PlateThickness);
            var wavelength10 = GetWavelength10(variantWavelength);
            var wavelength01 = GetWavelength01(variantWavelength);
            
            var shift = GetPhaseShift(PlateLength, wavelength10, wavelength01);
                
            return 0.5d * Math.Atan((2 * a) / (a*a - 1) * Math.Cos(shift));
        }

        private double GetEllipticityCoefficient(double variantWavelength)
        {
            var a = GetA(variantWavelength, PlateLength, PlateThickness);
            
            var wavelength10 = GetWavelength10(variantWavelength);
            var wavelength01 = GetWavelength01(variantWavelength);
            
            var shift = GetPhaseShift(PlateLength, wavelength10, wavelength01);
            
            var absSinOfShift = Math.Abs(Math.Sin(shift));
            var powSinOfShift = Math.Pow(Math.Cos(shift), 2);

            var aPow4 = Math.Pow(a, 4);

            return (2*a * absSinOfShift) /
                   (a*a + 1 + Math.Sqrt(aPow4 - 2 * a*a + 1 + 4*a*a * powSinOfShift));
        }

        private double GetPhaseShift(double plateLength, double wavelength10, double wavelength01)
        {
            const double pi = Mathf.PI;

            return 2 * pi / wavelength10 * plateLength - 2 * pi / wavelength01 * plateLength;
        }

        private double GetA(double variantWavelength, double plateLength, double plateThickness)
        {
            var g = GetG(variantWavelength, plateLength, plateThickness);
            
            return Math.Sqrt(1 - Math.Pow(Complex.Abs(g), 2));
        }
        
        private Complex GetG(double variantWavelength, double plateLength, double plateThickness)
        {
            var z01 = GetZ01(variantWavelength);
            var zIn = GetZIn(variantWavelength, plateLength, plateThickness);

            return
                (z01 - zIn) /
                (z01 + zIn);
        }

        private Complex GetZIn(double variantWavelength, double plateLength, double plateThickness)
        {
            const double pi = Mathf.PI;

            var z01 = GetZ01(variantWavelength);
            var z10 = GetZ10(variantWavelength);

            var lambda10 = GetWavelength10(variantWavelength);

            var tanArg = 2 * pi * plateLength / lambda10;

            var j = Complex.ImaginaryOne;
            
            return z10 * 
                   (z01 + j * z10 * Math.Tan(tanArg)) / 
                   (z10 + j * z01 * Math.Tan(tanArg));
        }

        private double GetZ10(double variantWavelength)
        {
            var l10 = GetWavelength10(variantWavelength);
            var l01 = GetWavelength01(variantWavelength);
            var z01 = GetZ01(variantWavelength);

            return l10 / l01 * z01;
        }

        private double GetZ01(double variantWavelength)
        {
            const double pi = Mathf.PI;
            var a = INTERNAL_WAVEGUIDE_WIDTH;

            var pow = Math.Pow(variantWavelength / (2 * a), 2);
            var sqrt = Math.Sqrt(1 - pow);
            
            return 120 * pi / sqrt;
        }
    }
}