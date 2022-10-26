using UnityEngine;

public class Weapon : MonoBehaviour {

    // Icono del HUD -v-
    [SerializeField, Header("Icono:")]
    private Sprite m_Icon;
    public Sprite Icon() { return m_Icon; }

    [SerializeField, Header("Munición:")]
    private int m_CurrentAmmo;
    public int Ammo() { return m_CurrentAmmo; }
    private int m_MagazineCapacity;
    [SerializeField]
    private int m_TotalAmmo;
    public int TotalAmmo() { return m_TotalAmmo; }

    [SerializeField, Header("Info del arma:")]
    private float m_ReloadTime;
    public float ReloadTime() { return m_ReloadTime; }
    [SerializeField]
    private float m_ShootDelay;
    public float ShootDelay() { return m_ShootDelay; }
    [SerializeField]
    private float m_RecoilStr;
    public float RecoilStr() { return m_RecoilStr; }
    [SerializeField]
    private float m_MissFireRate;
    public float MissFireRate() { return m_MissFireRate; }

    [SerializeField, Header("Bullet:")]
    private GameObject m_Bullet;
    public GameObject BulletPrefab() { return m_Bullet; }

    private float m_BulletDistance;
    public float BulletDistance() { return m_BulletDistance; }

    private float m_BulletSpeed;
    public float BulletSpeed() { return m_BulletSpeed; }

    [SerializeField, Header("Arma:")]
    private GameObject m_Weapon;
    public GameObject WeaponObj() { return m_Weapon; }

    // Carga la información de un scriptable object
    // In: WeaponData data -> Informaicón de un arma
    public void LoadWeapon(WeaponData data) {
        m_Icon = data.Icon;
        m_TotalAmmo = data.InitialAmmo;
        m_MagazineCapacity = data.MagazineCapacity;
        m_CurrentAmmo = m_MagazineCapacity;
        m_ReloadTime = data.ReloadTime;
        m_ShootDelay = data.ShootTime;
        m_RecoilStr = data.Recoil;
        m_MissFireRate = data.MissFireRate;
        m_Bullet = data.BulletPrefab;
        m_BulletDistance = data.BulletDistance;
        m_BulletSpeed = data.BulletSpeed;
    }

    // Check si puedo disaprar
    // Out: bool (true -> puedo disparar | false -> no puedo disprar )
    public bool CanShoot() {
        return (m_CurrentAmmo > 0);
    }

    // Check si puedo recargar
    // Out: bool (true -> puedo recargar | false -> no puedo recargar)
    public bool CanReload() {
        return ((m_TotalAmmo > 0) && (m_CurrentAmmo < m_MagazineCapacity));
    }

    // Añade muníción al total ammo
    // In: int amount -> cantidad de balas
    public void AddAmmo(int amount) {
        m_TotalAmmo += amount;
    }

    // Dispara
    public void Shoot() {
        m_CurrentAmmo--;
    }

    // Set de la munición
    // In: int current -> actual, en cargador
    // In: int toatl -> total
    public void SetAmmo(int current, int total) {
        m_CurrentAmmo = current;
        m_TotalAmmo = total;
    }

    // Recarga la munición necesaria
    // Like videojuegos NO REAL LIFE
    public void Reload() {
        int l_ammoNeeded = m_MagazineCapacity - m_CurrentAmmo;
        l_ammoNeeded = Mathf.Clamp(l_ammoNeeded, 0, m_TotalAmmo);
        m_CurrentAmmo += l_ammoNeeded;
        m_TotalAmmo -= l_ammoNeeded;
    }
}
