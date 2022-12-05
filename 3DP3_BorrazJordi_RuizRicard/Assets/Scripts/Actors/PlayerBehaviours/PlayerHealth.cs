using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    // Observer para indicar que el player a muerto
    public static event Action OnDie;
    // Observer para indicar que el player acaba de hacer respawn;
    public static event Action OnRespawn;
    // Observer para el evento de cambio de vida
    public static event Action OnHealthChanged;

    private const float LIFE_PORTION = 0.125f;

    [SerializeField]
    private bool m_Alive;
    public bool IsAlive() { return m_Alive; }
    public bool IsDead() { return !m_Alive; }

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

    [SerializeField]
    private int m_Lifes;
    [SerializeField]
    private int m_MaxLifes;
    public int Lifes() { return m_Lifes; }

    // Unity Start
    void Start() {
        m_Alive = true;
        m_CurrentHealth = m_MaxHealth;
    }

    // Set de vida
    // In: int health -> vida
    public void SetHealt(float health) {
        m_CurrentHealth = health;
        CheckDeath();
    }

    // Método para cuararse
    // In: Cantidad a curar (0.0f - 1.0f)
    public void Heal(float amount) {
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth + amount, 0f, m_MaxHealth);
        OnHealthChanged?.Invoke();
        CheckDeath();
    }

    // Método para restablecer el sistema de Vida
    public void RestoreFull() {
        m_Lifes = m_MaxLifes;
        m_CurrentHealth = m_MaxHealth;
    }

    // Método para recibir daño
    // In: Cantidad de daño (0.0f - 1.0f)
    public void TakeDamage(float amount = LIFE_PORTION) {
        if ((m_Invencible) || (!m_Alive))
            return;
        // Clamp de la vida + Check de muerte
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth - amount, 0f, m_MaxHealth);
        OnHealthChanged?.Invoke();
        CheckDeath();
    }

    // Método para matar al jugador
    // Haciendole recibir el MaxHealth
    public void Kill() {
        TakeDamage(m_MaxHealth);
    }

    // Método para hacerse invencible
    public void MakeInvencible() {
        m_Invencible = true;
        StartCoroutine(DisableInvencible(m_InvencibleDuration));
    }

    // Método de Caroutine para desactivar la invencibilidad
    // In: float time -> Duración de la invencibilidad
    private IEnumerator DisableInvencible(float time) {
        yield return new WaitForSeconds(time);
        m_Invencible = false;
    }

    // Método que comprueba si seguimos vivos y hace call de OnDie
    private void CheckDeath() {
        if (m_CurrentHealth <= 0f) {
            m_Alive = false;
            m_Lifes--;
        } else {
            m_Alive = true;
        }

        //Si hemos muerto
        if (!m_Alive)
            OnDie?.Invoke();
    }

    // Método para revivir al player
    public void Revive() {
        if (m_Alive)
            return;

        m_Alive = true;
        Heal(m_MaxHealth);
        OnRespawn?.Invoke();
    }

}
