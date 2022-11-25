using System;
using UnityEngine;

public class PlayerDeath : MonoBehaviour {

    // Observer para indicar que el player a muerto
    public static event Action OnDie;
    // Observer para indicar que el player acaba de hacer respawn;
    public static event Action OnRespawn;

    private PlayerCheckPoint m_CheckPoint;

    [SerializeField]
    private bool m_Alive;
    public bool IsAlive() { return m_Alive; }

    // Unity Awake
    void Awake() {
        m_CheckPoint = GetComponent<PlayerCheckPoint>();
    }

    // Unity Start
    void Start() {
        m_Alive = true;
    }

    // Otra forma de morir, AVEMUS MATADO
    public void Kill() {
        Die();

    }

    // Método para matar al player
    public void Die() {
        if (!m_Alive)
            return;

        m_Alive = false;
        uCore.Audio.Play2DSFX("die", this.transform.position);
        OnDie?.Invoke();
    }

    // Método para revivir al player
    public void Revive() {
        m_Alive = true;
    }

    // Método para hacer respawn del player
    public void Respawn() {
        Revive();
        m_CheckPoint.RestoreData();
        OnRespawn?.Invoke();
    }

}
