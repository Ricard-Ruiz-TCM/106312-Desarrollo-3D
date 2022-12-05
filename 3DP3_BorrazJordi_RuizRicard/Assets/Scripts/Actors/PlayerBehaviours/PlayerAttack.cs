using UnityEngine;

public enum TPunchType {
    RIGHT_HAND, LEFT_HAND, KICK
}

public class PlayerAttack : MonoBehaviour {

    [SerializeField, Header("Control:")]
    private bool m_Attacking;
    public bool IsAttacking() { return m_Attacking; }
    public void StartAttack() { m_Attacking = true; }
    public void EndAttack() { m_Attacking = false; }
    public bool CanAttack() { return !IsAttacking(); }

    [SerializeField]
    private int m_Combo;
    public int Combo() { return m_Combo; }

    [SerializeField, Header("Timers:")]
    private float m_ComboTime;
    [SerializeField]
    private int m_MaxCombo;
    // Control de tiempo
    private float m_Time;

    [SerializeField, Header("Daño:")]
    private float m_Damage;
    public float Damage() { return (m_Damage * m_Combo / 2.5f); }

    public PlayerJump m_jump;
    [SerializeField, Header("Colliders:")]
    private Collider m_RHand;
    [SerializeField]
    private Collider m_LHand, m_Kick;

    // Unity Start
    void Start() {
        m_RHand.gameObject.SetActive(false);
        m_LHand.gameObject.SetActive(false);
        m_Kick.gameObject.SetActive(false);
    }

    // Método para activar/desactivar el collider del puñetazo
    public void SetPunchActive(TPunchType PunchType, bool Active) {
        switch (PunchType) {
            case TPunchType.RIGHT_HAND:
                m_RHand.gameObject.SetActive(Active);
                break;
            case TPunchType.LEFT_HAND:
                m_LHand.gameObject.SetActive(Active);
                break;
            case TPunchType.KICK:
                m_Kick.gameObject.SetActive(Active);
                break;
            default: break;
        }
    }

    // Método para golepar
    public void Punch() {

        // Calc del tiempo para el combo
        m_Time = Time.timeSinceLevelLoad - m_Time;

        // Si ha pasado mucho tiempo o hemos hecho el combo entero, reinicmaos
        if ((m_Time > m_ComboTime) || (m_Combo >= m_MaxCombo))
            m_Combo = 0;

        // Incrmenetamos combo
        m_Combo++;

        // Guardamos la útlima vez que dimos golpe
        m_Time = Time.timeSinceLevelLoad;

        AudioAttacks();
    }

    public void AudioAttacks()
    {
        if (m_Combo == 1)
        {
            uCore.Audio.Play2DSFX("punch-1", this.transform.position);
            uCore.Particles.PlayParticlesOnce("Hit", m_RHand.transform.position);
        }
        if (m_Combo == 2)
        {
            uCore.Audio.Play2DSFX("punch-2", this.transform.position);
            uCore.Particles.PlayParticlesOnce("Hit", m_LHand.transform.position);

        }
        if (m_Combo == 3)
        {
            uCore.Audio.Play2DSFX("kick", this.transform.position);
            uCore.Particles.PlayParticlesOnce("Hit", m_Kick.transform.position);

        }
    }

}
