using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Weapon")]
public class WeaponData : ScriptableObject {

    // Icono del HUD
    public Sprite Icon;

    // Prefabs
    public GameObject WeaponPrefab;
    public GameObject BulletPrefab;

    // Nombre del arma
    public string WeaponName;

    // Info del arma y cargador, muncición, etc
    public int InitialAmmo;
    public int MagazineCapacity;
    public float ReloadTime;
    public float ShootTime;
    public float Recoil;
    public float MissFireRate;

    // Bullet
    public float BulletDistance;
    public float BulletSpeed;

}
