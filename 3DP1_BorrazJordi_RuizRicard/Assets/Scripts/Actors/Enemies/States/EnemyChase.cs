using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : BasicState {

    [SerializeField, Header("Chase State:")]
    private float m_MaxDistance;
    [SerializeField]
    private float m_MinDistance;

    // Player
    private Transform m_Player;
    // NavMeshAgent 
    private NavMeshAgent m_Agent;

    private EnemyAttack m_Attack;

    [SerializeField]
    private EnemyEye m_Eye;

    [SerializeField]
    private Transform m_Drone;

    // Unity Awake
    void Awake() {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Attack = GetComponent<EnemyAttack>();
        m_Player = uCore.GameManager.GetPlayer().transform;
    }

    // OnEnter
    public override void OnEnter() {
        m_Eye.ChangeEyeColor(Color.red);
    }

    // OnExit
    public override void OnExit() {
        m_Agent.destination = this.transform.position;
    }

    // OnUpdate
    public override void OnUpdate() {
        m_Drone.transform.LookAt(m_Player.transform.position + new Vector3(0.0f, 1.8f, 0.0f));
        // Set del destination
        if (Vector3.Distance(this.transform.position, m_Player.position) > m_Attack.AttackDistance())
            m_Agent.destination = m_Player.transform.position;
    }

    public bool InsideMaxDistance() {
        bool l_onDistance = Vector3.Distance(m_Player.transform.position, this.transform.position) < m_MaxDistance;
        l_onDistance &= Vector3.Distance(m_Player.transform.position, this.transform.position) > m_MinDistance;
        return l_onDistance;
    }

    // Método para comprobar si el player ha conseguido huir mientas los chaseabamos
    // Out: bool -> (true -> ha conseguido huir | false -> no hac onsguido huir)
    public bool PlayerFlee() {
        return (Vector3.Distance(this.transform.position, m_Player.position) > m_MaxDistance);
    }

}
