using System;
using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    // Observer para el cambio de munción/disparo/recargar
    public static event Action OnAmmoChanged;
    // Observer para el cmabio de arma
    public static event Action OnWeaponChange;

    [SerializeField, Header("Arma:")]
    private Weapon m_CurrentWeapon;
    public Weapon CurrentWeapon() { return m_CurrentWeapon; }
    private WeaponData m_Data;
    public WeaponData WeaponData() { return m_Data; }
    [SerializeField]
    private GameObject m_WeaponContainer;

    [SerializeField, Header("Control Disparo:")]
    private bool m_Shooting;
    public bool IsShooting() { return m_Shooting; }
    private float m_NoShootingTime;
    [SerializeField]
    private bool m_Reloading;
    public bool IsReloading() { return m_Reloading; }

    [SerializeField, Header("MissFire:")]
    private float m_MissFireRate;
    private float m_MissFireResetTime = 0.8f;

    // Unity Start
    void Start() {
        m_Shooting = false;
        m_NoShootingTime = 0.0f;
        m_Reloading = false;
    }

    // Unity Update
    void Update() {
        m_NoShootingTime += Time.deltaTime;
    }

    // Set de la info necesaria
    // In: PlayerData data -> info
    public void SetData(PlayerData data) {
        m_Data = data.InnitialWeapon;
        LoadWeapon(data.InnitialWeapon);
    }

    // Instnacia un arma de trinka
    // In: WeaponData data -> Informaicón básica del arma
    public void LoadWeapon(WeaponData data) {
        if (m_CurrentWeapon != null)
            GameObject.Destroy(m_CurrentWeapon.gameObject);

        m_CurrentWeapon = GameObject.Instantiate(data.WeaponPrefab, m_WeaponContainer.transform).GetComponent<Weapon>();
        m_CurrentWeapon.gameObject.name = "Weapon";
        m_CurrentWeapon.LoadWeapon(data);
        m_MissFireRate = m_CurrentWeapon.MissFireRate();
        OnWeaponChange?.Invoke();
    }

    // Check si puedo disaprar
    // Out: bool (true -> puedo disparar | false -> no puedo disprar)
    public bool CanShoot() {
        return ((m_CurrentWeapon.CanShoot()) && (!m_Reloading));
    }

    // Método para disparar, hace muchas cosas je
    // Out: resultado (true -> Disparo | false -> no disparo)
    public bool Shoot() {

        // NO WEAPON, NO PARTY
        if (m_CurrentWeapon == null)
            return false;

        if (m_Shooting || m_Reloading)
            return false;

        if (!CanShoot())
            return false;

        m_Shooting = true;
        m_CurrentWeapon.Shoot();
        OnAmmoChanged?.Invoke();
        StartCoroutine(EndShoot(m_CurrentWeapon.ShootDelay()));

        // Recoil
        float l_recoil = m_CurrentWeapon.RecoilStr();
        // TODO // Aplicar el recoil a la posición/rotación del jugador

        // Missfire
        if (m_NoShootingTime <= m_MissFireResetTime) {
            m_MissFireRate += m_CurrentWeapon.MissFireRate() / 100.0f;
        } else {
            m_MissFireRate = 0.0f;
        }
        m_NoShootingTime = 0.0f;

        // Instantiate de la bala
        Vector3 l_center = new Vector3(0.5f, 0.5f, 0.0f);
        l_center.x += m_MissFireRate * UnityEngine.Random.Range(-1.0f, 1.0f);
        l_center.y += m_MissFireRate * UnityEngine.Random.Range(-1.0f, 1.0f);

        Ray l_ray = Camera.main.ViewportPointToRay(l_center);
        RaycastHit l_raycastHit;
        Physics.Raycast(l_ray, out l_raycastHit, m_CurrentWeapon.BulletDistance());
        Vector3 l_forward = l_raycastHit.point - m_CurrentWeapon.gameObject.transform.position;
        l_forward.Normalize();

        InstantiateBullet(l_forward);

        // Particles
        m_CurrentWeapon.gameObject.GetComponentInChildren<ParticleSystem>().Play();

        // Audio SFX
        uCore.Audio.PlaySFX("Shot", m_CurrentWeapon.transform.position);

        return true;
    }

    // Crea una balita 
    // In: Vector3 forward -> Dirección
    // In: flaot speed -> Velociadd de la bala
    private void InstantiateBullet(Vector3 forward) {
        GameObject l_bullet = GameObject.Instantiate(m_CurrentWeapon.BulletPrefab(), uCore.GameManager.ActorContainer());
        l_bullet.transform.position = m_CurrentWeapon.gameObject.transform.position + forward * 0.05f;
        l_bullet.transform.rotation = m_CurrentWeapon.gameObject.transform.rotation;
        l_bullet.transform.Rotate(new Vector3(0.0f, 0.0f, -90.0f));
        l_bullet.GetComponent<Bullet>().SetBulletDirection(forward, m_CurrentWeapon.BulletSpeed());
    }

    // Caroutine Acabar disparo
    private IEnumerator EndShoot(float time) {
        yield return new WaitForSeconds(time);
        m_Shooting = false;
    }

    // Check si puedo recargar
    // Out: bool (true -> puedo | false -> no puedo)
    public bool CanReload() {
        return ((m_CurrentWeapon.CanReload()) && (!m_Shooting));
    }

    // Método para recargar, hace muchas cosas, creo
    // Out: resultado (false -> no work | true -> si work)
    public bool Reload() {

        // NO WEAPON, NO PARTY
        if (m_CurrentWeapon == null)
            return false;

        if (m_Shooting || m_Reloading)
            return false;

        if (!CanReload())
            return false;

        m_Reloading = true;
        uCore.Audio.PlaySFX("Reload", m_CurrentWeapon.transform);
        m_CurrentWeapon.Reload();
        OnAmmoChanged?.Invoke();
        StartCoroutine(EndReload(m_CurrentWeapon.ReloadTime()));
        return true;
    }

    // Caroutine Acabar recargar
    private IEnumerator EndReload(float time) {
        yield return new WaitForSeconds(time);
        m_Reloading = false;
    }

    // Check si tengo un arma
    // Out: bool (true -> tengo | false -> no tengo)
    public bool HaveWeapon() {
        return (m_CurrentWeapon != null);
    }

    // Añade munición al arma actual
    // In: int amount -> cantidad de balas
    public void AddAmmo(int amount) {
        m_CurrentWeapon.AddAmmo(amount);
        OnAmmoChanged?.Invoke();
    }

    // Set de la munición
    // In: int current -> actual, en cargador
    // In: int toatl -> total
    public void SetAmmo(int current, int total) {
        m_CurrentWeapon.SetAmmo(current, total);
        OnAmmoChanged?.Invoke();
    }

}
