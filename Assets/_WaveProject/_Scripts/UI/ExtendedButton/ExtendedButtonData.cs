using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WaveProject.UI.ExtendedButton
{
    [Serializable]
    public class ExtendedButtonData : MonoBehaviour
    {
        [SerializeField] private ExtendedButtonEdits _edits;

        [SerializeField] private Image _image;
        [SerializeField] private ColorBlock _imageColors = ColorBlock.defaultColorBlock;
        [SerializeField] private SizeBlock _imageSizes = SizeBlock.DefaultSizeBlock;
        [SerializeField] private SpriteState _imageSprites;

        [SerializeField] private TextMeshProUGUI _textElement;
        [SerializeField] private ColorBlock _textElementColors = ColorBlock.defaultColorBlock;
        [SerializeField] private SizeBlock _textElementSizes = SizeBlock.DefaultSizeBlock;

        public ExtendedButtonEdits Edits
        {
            get => _edits;
            private set => _edits = value;
        }

        public Image Image
        {
            get => _image;
            private set => _image = value;
        }


        public ColorBlock ImageColors
        {
            get => _imageColors;
            private set => _imageColors = value;
        }


        public SizeBlock ImageSizes
        {
            get => _imageSizes;
            private set => _imageSizes = value;
        }


        public SpriteState ImageSprites
        {
            get => _imageSprites;
            private set => _imageSprites = value;
        }

        public TextMeshProUGUI TextElement
        {
            get => _textElement;
            private set => _textElement = value;
        }

        public ColorBlock TextElementColors
        {
            get => _textElementColors;
            private set => _textElementColors = value;
        }

        public SizeBlock TextElementSizes
        {
            get => _textElementSizes;
            private set => _textElementSizes = value;
        }

        protected void OnValidate()
        {
            Image = GetComponent<Image>();
            TextElement = GetComponentInChildren<TextMeshProUGUI>();
        }

        [Flags]
        public enum ExtendedButtonEdits
        {
            None = 0,
            ImageColor = 1 << 0,
            ImageSize = 1 << 1,
            ImageSprite = 1 << 2,
            TextColor = 1 << 3,
            TextSize = 1 << 4
        }
    }
}