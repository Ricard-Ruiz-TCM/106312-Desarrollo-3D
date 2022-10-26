using System;
using UnityEngine;

public class Shield : MonoBehaviour {

    // Observer para el evento de cambiar escudo
    public static event Action<GameObject> OnShieldChanged;

    [SerializeField, Header("Shield:")]
    private float m_CurrentShield;
    public float CurrentShield() { return m_CurrentShield; }
    private const float m_MaxShield = 1.0f;
    public float MaxShield() { return m_MaxShield; }

    // Unity Start
    void Start() {
        m_CurrentShield = 0.0f;
    }

    // Set del CurrentShield
    // In: float shield -> valor de escudo a set
    public void SetShield(float shield) {
        m_CurrentShield = shield;
    }

    // Método para incrementar escudo
    // In: Cantidad a incrementar (0.0f - 1.0f)
    public void IncreaseShield(float amount) {
        m_CurrentShield = Mathf.Clamp(m_CurrentShield + amount, 0f, m_MaxShield);
        OnShieldChanged?.Invoke(this.gameObject);
    }

    // Método para decrementar escudo
    // In: Cantidad a decrementar (0.0f - 1.0f)
    public void DecreaseShield(float amount) {
        if (!HaveShield())
            return;
        // Clamp del escudo
        m_CurrentShield = Mathf.Clamp(m_CurrentShield - amount, 0f, m_MaxShield);
        OnShieldChanged?.Invoke(this.gameObject);
    }

    // Método para destruir el escudo
    // Reciviendo el MaxShield como decremento
    public void DestroyShield() {
        DecreaseShield(m_MaxShield);
    }

    // M�todo comprobar si tengo escudo
    // Out: bool (true -> tengo escudo | false -> no tengo escudo)
    public bool HaveShield() {
        return (m_CurrentShield > 0.0f);
    }
}