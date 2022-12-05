using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAttack : BasicState {

    [SerializeField, Header("Control:")]
    private bool m_Attacking;
    public bool IsAttacking() { return m_Attacking; }
    private bool m_Rushing;
    public bool IsRushing() { return m_Rushing; }
    private bool m_AttackEnds;
    public bool AttackEnds() { return m_AttackEnds; }

    [SerializeField, Header("Attack Distance:")]
    private float m_Distance;

    [SerializeField, Header("Attack Timers:")]
    private float m_AttackDuration;

    // Player
    private Player m_Player;

    // Agent
    private NavMeshAgent m_Agent;
    [SerializeField, Header("Bull Rush")]
    private float m_ExtraAcc;
    [SerializeField]
    private float m_ExtraSpeed;

    private float m_BaseAcc;
    private float m_BaseSpeed;

    // Unity Awake
    void Awake() {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Player = uCore.GameManager.GetPlayer();
    }

    // Unity Start
    void Start() {
        m_BaseSpeed = m_Agent.speed;
        m_BaseAcc = m_Agent.acceleration;
    }

    // OnEnter
    public override void OnEnter() {
        m_Attacking = true;
        m_AttackEnds = false;
        m_Rushing = false;
        StartAttack();
    }

    // OnExit
    public override void OnExit() {
        m_Agent.speed = m_BaseSpeed;
        m_Agent.acceleration = m_BaseAcc;
        m_Attacking = false;
    }

    // OnUpdate
    public override void OnUpdate() { }

    // Método que empeiza el ataque
    private void StartAttack() {
        m_Rushing = true;
        m_Agent.speed = m_ExtraSpeed;
        m_Agent.acceleration = m_ExtraAcc;
        m_Agent.destination = m_Player.transform.position;
        StartCoroutine(EndAttack());
    }

    // Método que acaba el ataque
    private IEnumerator EndAttack() {
        yield return new WaitForSeconds(m_AttackDuration);
        m_AttackEnds = true;
        m_Rushing = false;
    }

    // Método que comprueba si el player esta dentro de la distancia de ataque
    // Out: bool -> (true -> estamos en distancia de ataque | false -> no estamos en distancia de ataque)
    public bool PlayerOnAttackDistance() {
        return (Vector3.Distance(this.transform.position, m_Player.transform.position) < m_Distance);
    }

}
