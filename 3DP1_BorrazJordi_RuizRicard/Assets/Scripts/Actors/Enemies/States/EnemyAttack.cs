using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : BasicState {

    [SerializeField, Header("Attack State:")]
    private bool m_Attacking;
    [SerializeField]
    private float m_Damage;
    [SerializeField]
    private float m_AttackDistance;
    public float AttackDistance() { return m_AttackDistance; }
    [SerializeField]
    private float m_AttackDelay;

    // Player
    private Player m_Player;

    [SerializeField]
    private GameObject m_Drone;

    [SerializeField]
    private EnemyEye m_Eye;
    [SerializeField]
    private Transform m_Cannon;

    // Unity Awake
    void Awake() {
        m_Player = uCore.GameManager.GetPlayer();
    }

    // Unity Start
    void Start() {
        // Un daño aleatorio, así xke sí je
        m_Damage = Random.Range(0.1f, 0.2f);
        // Un delay basádo en el daño, + daño + tiempo | - daño - tiempo
        m_AttackDelay = m_Damage * 20.0f;
    }

    // OnEnter
    public override void OnEnter() {
        m_Attacking = true;
        m_Eye.ChangeEyeColor(Color.red);
        m_Player.Vitals().TakeDamage(m_Damage);
        uCore.Particles.PlayParticlesOnce("Hit_01", m_Cannon.transform.position);
        // Iniciamos "la animación"
        StartCoroutine(EndAttack());
    }

    // OnExit
    public override void OnExit() {
        GetComponent<NavMeshAgent>().destination = this.transform.position;
    }

    // OnUpdate
    public override void OnUpdate() {
        m_Drone.transform.LookAt(m_Player.transform.position + new Vector3(0.0f, 1.8f, 0.0f));
    }

    // Coroutine para acabar el ataque
    private IEnumerator EndAttack() {
        yield return new WaitForSeconds(m_AttackDelay);
        m_Attacking = false;
    }

    // Check que ha acabado la animación de atacar
    // Out: bool -> (true -> esta atacando | false -> ya no está atacando)
    public bool AttackAnimEnd() {
        return !m_Attacking;
    }

    // Comprueba que estamos en rango de ataque
    // Out: bol -> (true -> estamos en rango de ataque | false -> no estámos edn rango de ataque)
    public bool InShootRange() {
        return (Vector3.Distance(this.transform.position, m_Player.transform.position) <= m_AttackDistance);
    }

}
