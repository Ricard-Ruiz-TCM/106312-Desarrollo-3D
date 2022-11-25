using UnityEngine;

public class Portal : BasicPortal {

    [SerializeField, Header("Main Camera Ctrl:")]
    private Camera m_Camera;
    [SerializeField]
    private float m_OffSetNearPlane;

    [SerializeField, Header("Portal Espejo:")]
    private Transform m_Other;
    public Transform Other() { return m_Other; }
    [SerializeField]
    private Portal m_Mirror;
    public Portal Mirror() { return m_Mirror; }
    public bool MirrorON() { return m_Mirror.gameObject.activeSelf; }

    [SerializeField]
    private Laser m_Laser;
    public Laser Laser() { return m_Laser; }

    // Player
    private Player m_Player;

    // Unity Awake
    void Awake() {
        LoadPoints();
        m_Player = uCore.GameManager.GetPlayer();
        m_Laser = GetComponentInChildren<Laser>();
        m_Laser.gameObject.SetActive(false);
    }

    // Unity Late Update
    void LateUpdate() {
        Vector3 l_worldPosition = m_Player.Camera().transform.position;
        Vector3 l_LocalPosition = m_Other.transform.InverseTransformPoint(l_worldPosition);
        m_Mirror.m_Camera.transform.position = m_Mirror.transform.TransformPoint(l_LocalPosition);

        Vector3 l_wordlDirection = m_Player.Camera().transform.forward;
        Vector3 l_localDirection = m_Other.InverseTransformDirection(l_wordlDirection);
        m_Mirror.m_Camera.transform.forward = m_Mirror.transform.TransformDirection(l_localDirection);

        float l_Distance = Vector3.Distance(m_Mirror.m_Camera.transform.position, m_Mirror.transform.position);
        m_Mirror.m_Camera.nearClipPlane = l_Distance + m_OffSetNearPlane;
    }

}
