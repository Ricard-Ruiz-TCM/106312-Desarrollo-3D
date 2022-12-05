using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyChase : BasicState {

    [SerializeField, Header("Detection Distance:")]
    private float m_Distance;

    [SerializeField, Header("Control:")]
    private bool m_Chasing;
    [SerializeField]
    private float m_FacingTime;

    // Player
    private Player m_Player;

    // Agent
    private NavMeshAgent m_Agent;

    // Unity Awake
    void Awake() {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Player = uCore.GameManager.GetPlayer();
    }

    // OnEnter
    public override void OnEnter() {
        m_Chasing = true;
        m_Agent.destination = this.transform.position;
        StartCoroutine(EndFacing());
    }

    // OnExit
    public override void OnExit() { }

    // OnUpdate
    public override void OnUpdate() {
        float x = this.transform.rotation.x;
        this.transform.LookAt(m_Player.transform);
        this.transform.rotation = Quaternion.Euler(x, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
    }

    // M�todo para Coroutine
    IEnumerator EndFacing() {
        yield return new WaitForSeconds(m_FacingTime);
        m_Chasing = false;
    }

    // M�todo que comprueba si el player esta dentro de la distancia de detecci�n
    // Out: bool -> (true -> estamos en distancia de detecci�n | false -> no estamos en distancia de detecci�n)
    public bool DetectionDistance() {
        return (Vector3.Distance(this.transform.position, m_Player.transform.position) < m_Distance);
    }

    // M�todo que comprueba si ya hemos acabado el tiempo
    // Out: bool -> (true -> ya ha terminado el chase | false -> todav�a estamos espearndo)
    public bool ChaseEnded() {
        return !m_Chasing;
    }

}
