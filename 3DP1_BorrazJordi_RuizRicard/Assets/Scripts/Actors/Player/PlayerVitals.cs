using System;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(Shield))]
public class PlayerVitals : MonoBehaviour {

    // Observer para actualizar el HUD del player
    public static event Action OnVitalsChanged;

    // Health
    private Health m_Health;
    public Health PlayerHealth() { return m_Health; }

    // Shield
    private Shield m_Shield;
    public Shield Playershield() { return m_Shield; }

    // Reducciones de daño para el player, COSAS DE DISEÑO, IDK
    private float m_HealthDR;
    private float m_ShieldDR;

    // Unity OnEnable
    void OnEnable() {
        Health.OnHealthChanged += VitalsChanged;
        Shield.OnShieldChanged += VitalsChanged;
    }

    // Unity OnDisable
    void OnDisable() {
        Health.OnHealthChanged -= VitalsChanged;
        Shield.OnShieldChanged -= VitalsChanged;
    }

    // Unity Awake
    void Awake() {
        m_Health = GetComponent<Health>();
        m_Shield = GetComponent<Shield>();
    }

    // Set de la info necesaria desde un PlayerData
    // In: PlayerData data -> Información
    public void SetData(PlayerData data) {
        m_Health.SetData(data.Invencible);

        m_HealthDR = data.HealthDR;
        m_ShieldDR = data.ShieldDR;
    }

    // Take Damage mejorado y necesario
    // In: float amount -> Daño recibido
    public void TakeDamage(float amount) {

        float l_damage2Shield = amount * m_ShieldDR;
        float l_damage2Health = amount * m_HealthDR;

        float l_shieldAmount = m_Shield.CurrentShield();

        // No tengo escudo
        if (!m_Shield.HaveShield()) {
            m_Health.TakeDamage(amount);
            uCore.Audio.PlaySFX("grunt", this.transform.position);

        }

        // El escudo cubre el daño
        else if (l_shieldAmount >= l_damage2Shield) {
            m_Shield.DecreaseShield(l_damage2Shield);
            m_Health.TakeDamage(l_damage2Health);
            uCore.Audio.PlaySFX("grunt", this.transform.position);
            // El escudo no cubre todo el daño
        } else {
            m_Shield.DestroyShield();
            m_Health.TakeDamage(l_damage2Health + (l_damage2Shield - l_shieldAmount));
            uCore.Audio.PlaySFX("grunt", this.transform.position);

        }

    }

    // Check si puedo curarme
    // Out: bool (true -> puedo curarme | false -> no puedo curarme)
    public bool CanHeal() {
        return (m_Health.CurrentHealth() < m_Health.MaxHealth());
    }

    // Check si puedo incrementar escudo
    // Out: bool (true -> puedo incrementar | false -> no puedo incrementar)
    public bool CanIncreaseShield() {
        return (m_Shield.CurrentShield() < m_Shield.MaxShield());
    }

    // On Heal/Shield Changed Obsever from Health/Shield
    private void VitalsChanged(GameObject obj) {
        if (obj != this.gameObject)
            return;

        OnVitalsChanged?.Invoke();
    }

}
