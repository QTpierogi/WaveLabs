using System;
using UnityEngine;

namespace WaveProject.UI.ExtendedButton
{
    [Serializable]
    public struct SizeBlock : IEquatable<SizeBlock>
    {
        [field: SerializeField] public Vector2 NormalSize { get; private set; }
        [field: SerializeField] public Vector2 HighlightedSize { get; private set; }
        [field: SerializeField] public Vector2 PressedSize { get; private set; }
        [field: SerializeField] public Vector2 SelectedSize { get; private set; }
        [field: SerializeField] public Vector2 DisabledSize { get; private set; }

        [field: SerializeField] public float FadeDuration { get; private set; }


        public static SizeBlock DefaultSizeBlock;

        static SizeBlock()
        {
            DefaultSizeBlock = new SizeBlock
            {
                NormalSize = Vector2.one,
                HighlightedSize = Vector2.one * 1.1f,
                PressedSize = Vector2.one * 1.2f,
                SelectedSize = Vector2.one,
                DisabledSize = Vector2.one,
                FadeDuration = 0.1f
            };
        }

        public override bool Equals(object obj)
        {
            return obj is SizeBlock other && Equals(other);
        }

        public bool Equals(SizeBlock other)
        {
            return NormalSize.Equals(other.NormalSize) && 
                   HighlightedSize.Equals(other.HighlightedSize) &&
                   PressedSize.Equals(other.PressedSize) && 
                   SelectedSize.Equals(other.SelectedSize) &&
                   DisabledSize.Equals(other.DisabledSize) && 
                   FadeDuration.Equals(other.FadeDuration);
        }
        
        public static bool operator==(SizeBlock point1, SizeBlock point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator!=(SizeBlock point1, SizeBlock point2)
        {
            return !point1.Equals(point2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NormalSize, HighlightedSize, PressedSize, SelectedSize, DisabledSize, FadeDuration);
        }
    }
}