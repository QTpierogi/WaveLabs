using TMPro;
using UnityEngine;

namespace WaveProject.BuildVersion
{
    public class BuildVersion : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        
        private void OnValidate() => _text ??= GetComponent<TMP_Text>();
        private void Awake() => _text.text = Application.version;
    }
}