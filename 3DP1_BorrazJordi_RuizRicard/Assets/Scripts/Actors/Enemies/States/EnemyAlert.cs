using UnityEngine;
using UnityEngine.AI;

public class EnemyAlert : BasicState {

    [SerializeField, Header("Alert State:")]
    private float m_TotalRotated;
    [SerializeField]
    private float m_RotationSpeed;
    private const float m_DestinyRotation = 359.9f;

    [SerializeField]
    private bool m_Rotating;

    [SerializeField, Header("Dron \"Sees\":")]
    private float m_EyesHeight = 1.2f;
    [SerializeField]
    private float m_PlayerEyesHeight = 1.8f;

    [SerializeField]
    private float m_SightDistance = 10.0f;
    public float SighDistance(float multi = 1.0f) { return m_SightDistance * multi; }
    [SerializeField]
    private float m_VisualCosAngle = 60.0f;
    [SerializeField]
    private LayerMask m_SightLayerMask;

    private bool m_Distance, m_Dot, m_Raycast;

    // Player
    private Player m_Player;
    // NavMeshAgent 
    private NavMeshAgent m_Agent;

    [SerializeField]
    private EnemyEye m_Eye;

    [SerializeField]
    private GameObject m_Drone;

    // Unity Awake
    void Awake() {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Player = uCore.GameManager.GetPlayer();
    }

    // OnEnter
    public override void OnEnter() {
        m_Rotating = true;
        m_TotalRotated = 0.0f;
        m_Eye.ChangeEyeColor(Color.yellow);
        m_Agent.destination = transform.position;
    }

    // OnExit
    public override void OnExit() { }

    // OnUpdate
    public override void OnUpdate() {
        // Get de la rotación
        float l_CurrentRotation = this.transform.eulerAngles.y;
        // Check si hemos acabado la "animación" de rotar
        if (m_TotalRotated >= m_DestinyRotation) {
            m_Rotating = false;
            return;
        }

        // Calculamos la siguiente rotación
        Vector3 rotation = Vector3.zero;
        rotation.y += l_CurrentRotation + m_RotationSpeed * Time.deltaTime;
        m_TotalRotated += Mathf.Abs(l_CurrentRotation - rotation.y);
        // Rotamos
        this.transform.rotation = Quaternion.Euler(rotation);
    }

    // Check si no estamos rotando
    // Out: bool (true -> hemos acabado la rotación | false -> no hemos acabado al rotación)
    public bool AlertAnimEnds() {
        return !m_Rotating;
    }

    // MNétodo para comprobamos si hacemos eye contact con el player
    // Out: bool -> (true -> estamos haciendo eye contact | false -> no estamos haciendo eye contact)
    public bool SeesPlayer() {
        // Get pos del player
        Vector3 l_PlayerPosition = m_Player.transform.position;
        // Get nustro forward
        Vector3 l_ForwardXZ = this.transform.forward;
        l_ForwardXZ.y = 0.0f;
        l_ForwardXZ.Normalize();

        // Calc de la direción con el player
        Vector3 l_DirectionToEnemyXZ = l_PlayerPosition - this.transform.position;
        l_DirectionToEnemyXZ.y = 0.0f;
        l_DirectionToEnemyXZ.Normalize();

        // Reposicionamos la "mirada" y posicón de los ojos del dron y del player
        Vector3 l_EyesPosition = this.transform.position + Vector3.up * m_EyesHeight;
        Vector3 l_PlayerEyesPosition = l_PlayerPosition + Vector3.up * m_PlayerEyesHeight;

        // Cal de la neuva dirección basada en la posición de los ojos
        Vector3 l_Direction = l_PlayerEyesPosition - l_EyesPosition;
        float l_lenght = l_Direction.magnitude;
        l_Direction /= l_lenght;
        Ray l_Ray = new Ray(l_EyesPosition, l_Direction);

        // Comprobamos distancia
        m_Distance = Vector3.Distance(l_PlayerPosition, this.transform.position) < m_SightDistance;
        // Comprobamos angulo
        m_Dot = Vector3.Dot(l_ForwardXZ, l_DirectionToEnemyXZ) > Mathf.Cos(m_VisualCosAngle * Mathf.Deg2Rad / 2.0f);
        // Comprobamos cualquier obstaculo
        m_Raycast = Physics.Raycast(l_Ray, l_lenght * 2.0f, m_SightLayerMask.value);

        // Solo lo verá, si esta en distancia, en angulo y no hay obstaculos
        return m_Distance && m_Dot && !m_Raycast;

    }

    // método para comprobar si el player esta dentro de la distancia de visión del dron
    // In: float multi -> Multiplicador a la distancia
    // Out: bool -> (true -> está dentro | false -> no está dentro)
    public bool PlayerOnDistance(float multi = 1.0f) {
        return Vector3.Distance(m_Player.transform.position, transform.position) < m_SightDistance * multi;
    }

    // método para comprobar si el player esta dentro de la distancia de visión del dron
    // Out: bool -> (true -> está dentro | false -> no está dentro)
    public bool PlayerOnRaycast() {
        SeesPlayer();
        return m_Raycast;
    }

}
