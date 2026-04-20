using System;
using UnityEngine;
using WaveProject.UI.View;

namespace WaveProject.UI
{
    [Serializable]
    internal class UIViews
    {
        [field: SerializeField] public SettingsView BestiaryUiView { get; private set; }
    }
}