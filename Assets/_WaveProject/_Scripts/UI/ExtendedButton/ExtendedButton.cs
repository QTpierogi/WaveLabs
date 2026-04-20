using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace WaveProject.UI.ExtendedButton
{
    [RequireComponent(typeof(ExtendedButtonData))]
    public class ExtendedButton : Button
    {
        private ExtendedButtonData _data;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            transition = Transition.None;
        }
#endif

        protected override void Awake()
        {
            base.Awake();
            _data = GetComponent<ExtendedButtonData>();
            transition = Transition.None;
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (!gameObject.activeInHierarchy)
                return;

            if (_data.Edits.Equals(ExtendedButtonData.ExtendedButtonEdits.None))
                return;

            Color imageColor;
            Vector2 imageSize;

            Sprite transitionSprite;

            Color textElementColor;
            Vector2 textElementSize;

            switch (state)
            {
                default:
                case SelectionState.Normal:
                    imageColor = _data.ImageColors.normalColor;
                    imageSize = _data.ImageSizes.NormalSize;
                    transitionSprite = null;

                    textElementColor = _data.TextElementColors.normalColor;
                    textElementSize = _data.ImageSizes.NormalSize;

                    break;

                case SelectionState.Highlighted:
                    imageColor = _data.ImageColors.highlightedColor;
                    imageSize = _data.ImageSizes.HighlightedSize;
                    transitionSprite = _data.ImageSprites.highlightedSprite;

                    textElementSize = _data.ImageSizes.HighlightedSize;
                    textElementColor = _data.TextElementColors.highlightedColor;

                    break;

                case SelectionState.Pressed:
                    imageColor = _data.ImageColors.pressedColor;
                    imageSize = _data.ImageSizes.PressedSize;
                    transitionSprite = _data.ImageSprites.pressedSprite;

                    textElementSize = _data.ImageSizes.PressedSize;
                    textElementColor = _data.TextElementColors.pressedColor;

                    break;

                case SelectionState.Selected:
                    imageColor = _data.ImageColors.selectedColor;
                    imageSize = _data.ImageSizes.SelectedSize;
                    transitionSprite = _data.ImageSprites.selectedSprite;

                    textElementSize = _data.ImageSizes.SelectedSize;
                    textElementColor = _data.TextElementColors.selectedColor;

                    break;

                case SelectionState.Disabled:
                    imageColor = _data.ImageColors.disabledColor;
                    imageSize = _data.ImageSizes.DisabledSize;
                    transitionSprite = _data.ImageSprites.disabledSprite;

                    textElementSize = _data.ImageSizes.DisabledSize;
                    textElementColor = _data.TextElementColors.disabledColor;

                    break;
            }

            _data.Image.DOKill();
            _data.TextElement.DOKill();

            if (_data.Edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.ImageColor))
                _data.Image.DOColor(imageColor, _data.ImageColors.fadeDuration);
            
            if (_data.Edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.ImageSize))
                _data.Image.transform.DOScale(imageSize, _data.ImageSizes.FadeDuration);

            if (_data.Edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.ImageSprite))
                _data.Image.overrideSprite = transitionSprite;

            if (_data.Edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.TextColor))
                _data.TextElement.DOColor(textElementColor, _data.TextElementColors.fadeDuration);

            if (_data.Edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.TextSize))
                _data.TextElement.transform.DOScale(textElementSize, _data.TextElementSizes.FadeDuration);
        }
    }
}