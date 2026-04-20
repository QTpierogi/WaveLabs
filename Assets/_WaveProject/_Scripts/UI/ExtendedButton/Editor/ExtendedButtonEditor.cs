using UnityEditor;
using UnityEditor.AnimatedValues;

namespace WaveProject.UI.ExtendedButton.Editor
{
    [CustomEditor(typeof(ExtendedButtonData))]
    public sealed class ExtendedButtonEditor : UnityEditor.Editor
    {
        private SerializedProperty _edits;

        private SerializedProperty _image;
        private SerializedProperty _imageColors;
        private SerializedProperty _imageSizes;
        private SerializedProperty _imageSprites;

        private SerializedProperty _textElement;
        private SerializedProperty _textElementColors;
        private SerializedProperty _textElementSizes;

        private readonly AnimBool _editImageColor = new();
        private readonly AnimBool _editImageSize = new();
        private readonly AnimBool _editImageSprite = new();

        private readonly AnimBool _editTextColor = new();
        private readonly AnimBool _editTextSize = new();

        private void OnEnable()
        {
            _edits = serializedObject.FindProperty(nameof(_edits));

            _image = serializedObject.FindProperty(nameof(_image));
            _imageColors = serializedObject.FindProperty(nameof(_imageColors));
            _imageSizes = serializedObject.FindProperty(nameof(_imageSizes));
            _imageSprites = serializedObject.FindProperty(nameof(_imageSprites));

            _textElement = serializedObject.FindProperty(nameof(_textElement));
            _textElementColors = serializedObject.FindProperty(nameof(_textElementColors));
            _textElementSizes = serializedObject.FindProperty(nameof(_textElementSizes));

            var edits = GetButtonEdits(_edits);

            _editImageColor.value = edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.ImageColor);
            _editImageSize.value = edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.ImageSize);
            _editImageSprite.value = edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.ImageSprite);
            _editTextColor.value = edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.TextColor);
            _editTextSize.value = edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.TextSize);

            _editImageColor.valueChanged.AddListener(Repaint);
            _editImageSize.valueChanged.AddListener(Repaint);
            _editImageSprite.valueChanged.AddListener(Repaint);
            _editTextColor.valueChanged.AddListener(Repaint);
            _editTextSize.valueChanged.AddListener(Repaint);
        }

        private void OnDisable()
        {
            _editImageColor.valueChanged.RemoveListener(Repaint);
            _editImageSize.valueChanged.RemoveListener(Repaint);
            _editImageSprite.valueChanged.RemoveListener(Repaint);
            _editTextColor.valueChanged.RemoveListener(Repaint);
            _editTextSize.valueChanged.RemoveListener(Repaint);
        }

        private static ExtendedButtonData.ExtendedButtonEdits GetButtonEdits(SerializedProperty buttonEdits)
        {
            return (ExtendedButtonData.ExtendedButtonEdits)buttonEdits.enumValueFlag;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var edits = GetButtonEdits(_edits);

            _editImageColor.target = !_edits.hasMultipleDifferentValues &&
                                     edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.ImageColor);
            _editImageSize.target = !_edits.hasMultipleDifferentValues &&
                                    edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.ImageSize);
            _editImageSprite.target = !_edits.hasMultipleDifferentValues &&
                                      edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.ImageSprite);
            _editTextColor.target = !_edits.hasMultipleDifferentValues &&
                                    edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.TextColor);
            _editTextSize.target = !_edits.hasMultipleDifferentValues &&
                                   edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.TextSize);

            EditorGUILayout.PropertyField(_edits);
            EditorGUILayout.Space();

            if (edits.Equals(ExtendedButtonData.ExtendedButtonEdits.None))
            {
                serializedObject.ApplyModifiedProperties();
                return;
            }

            ++EditorGUI.indentLevel;
            {
                if (edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.ImageColor) ||
                    edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.ImageSize) ||
                    edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.ImageSprite))
                {
                    EditorGUILayout.LabelField("Image edits", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(_image);
                }

                EditorGUILayout.Space();

                if (EditorGUILayout.BeginFadeGroup(_editImageColor.faded))
                {
                    EditorGUILayout.LabelField("Image colors edits", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(_imageColors);
                }

                EditorGUILayout.EndFadeGroup();
                EditorGUILayout.Space();

                if (EditorGUILayout.BeginFadeGroup(_editImageSize.faded))
                {
                    EditorGUILayout.LabelField("Image size edits", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(_imageSizes);
                }

                EditorGUILayout.EndFadeGroup();
                EditorGUILayout.Space();

                if (EditorGUILayout.BeginFadeGroup(_editImageSprite.faded))
                {
                    EditorGUILayout.LabelField("Image sprites edits", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(_imageSprites);
                }

                EditorGUILayout.EndFadeGroup();
                EditorGUILayout.Space();


                if (edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.TextSize) ||
                    edits.HasFlag(ExtendedButtonData.ExtendedButtonEdits.TextColor))
                {
                    EditorGUILayout.LabelField("Text edits", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(_textElement);
                }

                EditorGUILayout.Space();

                if (EditorGUILayout.BeginFadeGroup(_editTextColor.faded))
                {
                    EditorGUILayout.LabelField("Text colors edits", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(_textElementColors);
                }

                EditorGUILayout.EndFadeGroup();
                EditorGUILayout.Space();

                if (EditorGUILayout.BeginFadeGroup(_editTextSize.faded))
                {
                    EditorGUILayout.LabelField("Text sizes edits", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(_textElementSizes);
                }

                EditorGUILayout.EndFadeGroup();
                EditorGUILayout.Space();
            }
            --EditorGUI.indentLevel;

            serializedObject.ApplyModifiedProperties();
        }
    }
}