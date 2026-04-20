using UnityEngine;

namespace WaveProject.Configs
{
    [CreateAssetMenu(fileName = "InteractionSettings", menuName = "Configs/InteractionSettings", order = 0)]
    public class InteractionSettings : ScriptableObject
    {
        [field:Header("Interactable")]
        [field: SerializeField] public float Sensitivity { get; private set; } = 0.005f;
        
        [field:Header("Camera Fov")]
        [field: SerializeField] public float FovMinValue { get; private set; } = 20;
        [field: SerializeField] public float FovDefaultValue { get; private set; } = 60;
        [field: SerializeField] public float FovChangingSpeed { get; private set; } = 10;
        [field: SerializeField] public float FovSensitivity { get; private set; } = 3;

        [field:Header("Camera Movement")]
        [field: SerializeField] public float CameraMoveSpeed { get; private set; } = 2;
        
        [field:Header("Generator")]
        [field: SerializeField] public float MaxPower { get; private set; } = 100;
        [field: Min(0.01f), SerializeField] public float PowerStep { get; private set; } = 1.5f;
        [field: Min(0.01f), SerializeField] public float FrequencyStep { get; private set; } = 1.5f;

        [field:Header("Receiver")]
        [field: SerializeField] public float MaxReceiverScaleFactor { get; private set; } = 5;
        [field: SerializeField] public float MinZeroOffsetFactor { get; private set; } = -5;
        [field: SerializeField] public float MaxZeroOffsetFactor { get; private set; } = 15;
        [field: SerializeField] public float ReceiverArrowSpeedToTarget { get; private set; } = 2;
        
        // внутрянняя ширина волновода в метрах
        public const float INTERNAL_WAVEGUIDE_WIDTH = 0.023f; 

        // скорость света в метрах == 3 * 10^8
        public const float SPEED_OF_LIGHT = 3 * 100000000F; 
        
        public const float MIH_FREQUENCY = 7500;
        public const float MAX_FREQUENCY = 10500;

        
        private static InteractionSettings _instance;

        public static InteractionSettings Data =>
            _instance ??= Resources.Load<InteractionSettings>("Configs/InteractionSettings");

    }
}