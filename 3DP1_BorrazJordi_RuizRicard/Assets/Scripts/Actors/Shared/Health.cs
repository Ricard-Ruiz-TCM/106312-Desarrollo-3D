using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour {

    // Observer para el evento de cambio de vida
    public static event Action<GameObject> OnHealthChanged;
    public static event Action<GameObject> OnDie;

    [SerializeField, Header("Health:")]
    private bool m_Dead;
    public bool IsDead() { return m_Dead; }
    [SerializeField]
    private bool m_Invencible;
    public bool IsInvencible() { return m_Invencible; }
    [SerializeField]
    private float m_InvencibleDuration;
    [SerializeField]
    private float m_CurrentHealth;
    public float CurrentHealth() { return m_CurrentHealth; }
    private const float m_MaxHealth = 1.0f;
    public float MaxHealth() { return m_MaxHealth; }

    // Unity Start
    void Start() {
        m_CurrentHealth = m_MaxHealth;
    }

    // Set del cosas b�sicas
    // In: float time -> Valor del tiempo de invencibildiad
    public void SetData(float time) {
        m_InvencibleDuration = time;
    }

    // Set de vida
    // In: int health -> vida
    public void SetHealt(float health) {
        m_CurrentHealth = health;
        CheckDeath();
    }

    // M�todo para cuararse
    // In: Cantidad a curar (0.0f - 1.0f)
    public void Heal(float amount) {
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth + amount, 0f, m_MaxHealth);
        OnHealthChanged?.Invoke(this.gameObject);
        CheckDeath();
    }

    // M�todo para recibir da�o
    // In: Cantidad de da�o (0.0f - 1.0f)
    public void TakeDamage(float amount) {
        if ((m_Invencible) || (m_Dead))
            return;
        // Clamp de la vida + Check de muerte
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth - amount, 0f, m_MaxHealth);
        OnHealthChanged?.Invoke(this.gameObject);
        CheckDeath();
    }

    // M�todo para matar al jugador
    // Haciendole recibir el MaxHealth
    public void Kill() {
        TakeDamage(m_MaxHealth);
    }

    // M�todo para hacerse invencible
    public void MakeInvencible() {
        m_Invencible = true;
        StartCoroutine(DisableInvencible(m_InvencibleDuration));
    }

    // M�todo de Caroutine para desactivar la invencibilidad
    // In: float time -> Duraci�n de la invencibilidad
    private IEnumerator DisableInvencible(float time) {
        yield return new WaitForSeconds(time);
        m_Invencible = false;
    }

    // M�todo que comprueba si seguimos vivos y hace call de OnDie
    private void CheckDeath() {
        if (m_CurrentHealth <= 0f)
            m_Dead = true;
        else
            m_Dead = false;

        if (m_Dead)
            OnDie?.Invoke(this.gameObject);
    }

}