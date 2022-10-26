using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : BasicState {

    [SerializeField, Header("Patrol State:")]
    private GameObject m_PatrolID;
    [SerializeField, Header("Patrol Route:")]
    private bool m_OneWayRoute;
    private bool m_Forward;
    [SerializeField]
    private int m_DestinyID;
    [SerializeField]
    private List<Transform> m_Points;
    private Transform m_Destiny;

    [SerializeField]
    private float m_HearDistance = 12.0f;

    // NavMeshAgent 
    private NavMeshAgent m_Agent;

    [SerializeField]
    private EnemyEye m_Eye;

    // OnEnable
    void OnEnable() {
        RespawnButton.OnPlayerRespawn += () => { m_DestinyID = 0; SetDestination(); };
    }

    // OnDisable
    void OnDisable() {
        RespawnButton.OnPlayerRespawn -= () => { m_DestinyID = 0; SetDestination(); };
    }

    // Unity Awake
    void Awake() {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    // Unity Start
    void Start() {
        m_Points = new List<Transform>();
        foreach (Transform c in m_PatrolID.transform)
            m_Points.Add(c);

        m_DestinyID = 0;
        m_Forward = true;
    }

    // OnEnter
    public override void OnEnter() {
        m_Eye.ChangeEyeColor(Color.white);
        if (m_Destiny == null)
            return;

        m_Agent.destination = m_Destiny.position;
        this.transform.LookAt(m_Destiny);
    }

    // OnExit
    public override void OnExit() {
        m_Destiny.position = m_Agent.destination;
    }

    // OnUpdate
    public override void OnUpdate() {
        // Check si llega al destino y cambia de punto
        if (PatrolTargetPositionArrived())
            MoveToNextPatrolPosition();
    }

    // Método que comprueba si se escucha al player
    // Out: bool (true -> lo escuda | false -> no lo escucha)
    public bool HearsPlayer() {
        bool l_dis = Vector3.Distance(uCore.GameManager.GetPlayer().transform.position, this.transform.position) < m_HearDistance;
        return (l_dis && uCore.GameManager.GetPlayer().Moving());
    }

    // Método para selecionar el siguietne punto destino del recorrido
    private void MoveToNextPatrolPosition() {
        // Comprueba si es OneWayRoute (ruta donde va y viene, no es ciclica)
        if (m_OneWayRoute) {
            if (m_Forward) {
                m_DestinyID++;
                if (m_DestinyID == m_Points.Count - 1) {
                    m_Forward = false;
                }
            } else {
                m_DestinyID--;
                if (m_DestinyID == 0) {
                    m_Forward = true;
                }
            }
        } else {
            // Ruta ciclica
            m_DestinyID++;
            m_DestinyID %= m_Points.Count;
        }

        SetDestination();
    }

    // Método para establecer el destino segun m_DestinyID
    private void SetDestination() {
        if (!this.gameObject.activeSelf)
            return;
        // Selecciona destino de forma interna y al Agent
        m_DestinyID = Mathf.Clamp(m_DestinyID, 0, m_Points.Count - 1);
        m_Destiny = m_Points[m_DestinyID];
        m_Agent.destination = m_Destiny.position;
    }

    // Comprueba si ha llegado al destino
    // Out: bool (true -> Ha llegdo al destino | false -> no ha llegado al destino)
    private bool PatrolTargetPositionArrived() {
        return !m_Agent.hasPath && !m_Agent.pathPending && m_Agent.pathStatus == NavMeshPathStatus.PathComplete;
    }

}
