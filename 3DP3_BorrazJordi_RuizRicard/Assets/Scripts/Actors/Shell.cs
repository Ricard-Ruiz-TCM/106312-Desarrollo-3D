using UnityEngine;
using UnityEngine.AI;

public class Shell : MonoBehaviour {

    // Contorl
    [SerializeField, Header("Control:")]
    private bool m_Moving;
    [SerializeField]
    private float m_Distance;
    [SerializeField]
    private LayerMask m_Layers;

    [SerializeField]
    private GameObject m_RayPoint;

    // Nav Mesh Agent
    private NavMeshAgent m_Agent;

    // Unity Awake
    void Awake() {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    // Unity Start
    void Start() {
        m_Moving = false;
    }

    // Unity Update
    void Update() {
        // Si no estamos "Activos" no hacmeos nada del movimiento
        if (!m_Moving)
            return;

        
        // Control de rotaci�n
        // Raycast frontal, para comprobar paredes y movidas as�
        if (Physics.Raycast(new Ray(m_RayPoint.transform.position, this.transform.forward), m_Distance / 2.5f, m_Layers)) {
            RotateShell();
        }
        // Raycast inferior, comprueba que no vayamos a precipitarnos
        if (!Physics.Raycast(new Ray(m_RayPoint.transform.position, -this.transform.up), m_Distance, m_Layers)) {
            RotateShell();
        }

        // Movimiento
        m_Agent.destination = m_RayPoint.transform.position;

    }

    // M�todo para rotar la shell
    private void RotateShell() {
        m_Agent.enabled = false;

        this.transform.Rotate(new Vector3(0.0f, 180.0f + Random.Range(-25.0f, 25.0f), 0.0f));

        m_Agent.enabled = true;
    }

    // * --------------------------------------------------------------------------------- *
    // | - COLLISION --------------------------------------------------------------------- |
    // V --------------------------------------------------------------------------------- V

    // OnTriggerEnter
    private void OnTriggerEnter(Collider other) {
        // Coll enemy
        if (other.tag == "Enemy") {
            other.GetComponent<Enemy>().GotHit();
            m_Moving = true;
        }
        // Col Player
        else if (other.tag == "Player") {
            if (m_Moving) {
                other.GetComponent<Player>().TakeDamage();
            } else {
                m_Moving = true;
                m_Agent.enabled = false;
                this.transform.rotation = Quaternion.Euler(other.transform.forward);
                m_Agent.enabled = true;
            }
            
        }
    }

    // A --------------------------------------------------------------------------------- A

}
