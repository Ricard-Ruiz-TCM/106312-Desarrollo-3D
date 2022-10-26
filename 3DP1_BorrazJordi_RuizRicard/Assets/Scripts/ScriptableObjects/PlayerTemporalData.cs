using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Temporal Player", menuName = "ScriptableObjects/TemporalPlayer")]
public class PlayerTemporalData : ScriptableObject {

    // Trasnform
    public Vector3 Position;
    public Quaternion Rotation;

    // Control
    public bool ValidInfo = false;

    // Vitals
    public float Health;
    public float Shield;

    // Shooting
    public WeaponData Weapon;
    public int Ammunition;
    public int TotalAmmunition;

    // Key Ring
    public List<int> Keys;

}
