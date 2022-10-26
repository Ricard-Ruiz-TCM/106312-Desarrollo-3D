using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "ScriptableObjects/Player")]
public class PlayerData : ScriptableObject {

    // PlayerMovement
    public float YawRotationSpeed;
    public float PitchRotationSpeed;
    public float MaxPitch;
    public float MinPitch;
    public float SpeedBase;
    public float SpeedMultiplier;

    // PlayerMovement Camera
    public float SmoothFOV;
    public float FastSpeedFOV;
    public float NormalSpeedFOV;

    // PlayerShooting
    public WeaponData InnitialWeapon;

    // PlayerJump
    public float JumptStr;
    public float CoyoteTime;

    // PlayerVitals
    public float HealthDR;
    public float ShieldDR;
    public float Invencible;

}
