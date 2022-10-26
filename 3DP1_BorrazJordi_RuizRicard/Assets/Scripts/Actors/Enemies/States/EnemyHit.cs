using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class EnemyHit : BasicState {

    [SerializeField, Header("Enemy Vitals:")]
    private Health m_Health;
    public Health EnemyHealth() { return m_Health; }
    [SerializeField]
    private bool m_GotHit;
    [SerializeField]
    private bool m_IsDying;
    [SerializeField]
    private float m_DeathTime;

    [SerializeField, Header("UI:")]
    private Image m_LifeBarImage;
    [SerializeField]
    private Transform m_LifeBarAnchorPosition;
    [SerializeField]
    private RectTransform m_LifeBarRectTransform;
    public GameObject m_dronParticlesPosition;
    // Enemy Alaert
    private EnemyAlert m_Alert;

    [SerializeField]
    private GameObject m_Drone;

    // Unity OnEnable
    void OnEnable() {
        Health.OnHealthChanged += OnHealthChanged;
        RespawnButton.OnPlayerRespawn += () => { m_Health.Heal(1.0f); };
    }

    // Unity OnDisable
    void OnDisable() {
        Health.OnHealthChanged -= OnHealthChanged;
        RespawnButton.OnPlayerRespawn -= () => { m_Health.Heal(1.0f); };
    }

    // Unity Awake
    void Awake() {
        m_Health = GetComponent<Health>();

        m_Alert = GetComponent<EnemyAlert>();
    }

    // Unity Update
    void Update() {
        UpdateLifeBarPosition();
    }

    // OnEnter
    public override void OnEnter() {
        m_IsDying = true;
        m_GotHit = false;
        StartCoroutine(EndHitAnimation());
    }

    // OnExit
    public override void OnExit() { }

    // OnUpdate
    public override void OnUpdate() { }

    // On HealChanged Obsever from Health
    private void OnHealthChanged(GameObject obj) {
        if (obj != this.gameObject)
            return;

        // Update de la vida
        m_LifeBarImage.fillAmount = m_Health.CurrentHealth();
    }

    private IEnumerator EndHitAnimation() {
        yield return new WaitForSeconds(m_DeathTime);
        m_IsDying = false;
    }

    // Método para recibir el balazo je 
    // In: float dmg -> Daño de la bala
    public void Hit(float dmg) {
        m_GotHit = true;
        uCore.Audio.PlaySFX("dron_hit", this.transform.position);

        uCore.Particles.PlayParticlesOnce("Hit_02", m_dronParticlesPosition.transform.position);
        m_Health.TakeDamage(dmg);
    }

    // Compruba si he recibido un golpecito de nada
    // Out: bool -> (true -> he recbiido un balazo | false -> no he re3cibido ningún balazo)
    public bool GotHit() {
        return m_GotHit;
    }

    // Comprueba si la animación ha acabado
    // Out: bool -> (true -> la animación ha acabado | false -> la animación no ha acabado)
    public bool HitAnimEnds() {
        return !m_IsDying;
    }

    void UpdateLifeBarPosition() { //Updates LifeBar in canvas2d so its up in drone
        Vector3 l_Position = Camera.main.WorldToViewportPoint(m_LifeBarAnchorPosition.position);
        m_LifeBarRectTransform.anchoredPosition = new Vector3(l_Position.x * 1920.0f, -(1080.0f - l_Position.y * 1080), 0);
        m_LifeBarRectTransform.gameObject.SetActive(l_Position.z > 0.0f);
        bool l_viable = m_Alert.PlayerOnDistance(3.5f);
        Ray l_ray = new Ray(m_Drone.transform.position, uCore.GameManager.GetPlayer().transform.position);
        l_viable &= Physics.Raycast(l_ray, m_Alert.SighDistance(3.5f), LayerMask.GetMask("Enemy"));

        if (m_Health.IsDead())
            l_viable = false;

        m_LifeBarRectTransform.gameObject.SetActive(l_viable);
    }

}
