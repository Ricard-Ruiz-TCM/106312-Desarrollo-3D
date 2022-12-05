using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyPatrol : BasicState {

    [SerializeField, Header("Ruta")]
    private Transform m_Route;
    private List<Transform> m_Points;

    private int m_Destiny;

    // Agent
    private NavMeshAgent m_Agent;

    // Unity Awake
    void Awake() {
        m_Agent = GetComponent<NavMeshAgent>();
    }
    
    // Unity Start
    void Start() {
        m_Destiny = 0;
        m_Points = new List<Transform>();
        foreach(Transform t in m_Route) {
            m_Points.Add(t);
        }
    }

    // OnEnter
    public override void OnEnter() {
        m_Destiny = Mathf.Clamp(m_Destiny, 0, m_Points.Count);
        m_Agent.destination = (m_Points[m_Destiny].position);
    }

    // OnExit
    public override void OnExit() {
        m_Agent.destination = this.transform.position;
    }

    // OnUpdate
    public override void OnUpdate() {
        if (PatrolTargetPositionArrived()) {
            NextPoint();
        }
    }

    // Método para avanzar hacía el siguiente punto
    private void NextPoint() {
        m_Destiny++;
        m_Destiny %= m_Points.Count;

        // Set destination
        m_Destiny = Mathf.Clamp(m_Destiny, 0, m_Points.Count - 1);
        m_Agent.destination = m_Points[m_Destiny].position;
    }

    // Método que comprueba si ha llegado al destino
    // Out: bool (true -> Ha llegdo al destino | false -> no ha llegado al destino)
    public bool PatrolTargetPositionArrived() {
        return !m_Agent.hasPath && !m_Agent.pathPending && m_Agent.pathStatus == NavMeshPathStatus.PathComplete;
    }

}
