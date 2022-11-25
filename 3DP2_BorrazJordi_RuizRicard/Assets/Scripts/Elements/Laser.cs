using UnityEngine;

public class Laser : MonoBehaviour {

    [SerializeField]
    private bool m_IsEnabled = true;
    public bool IsEnabled() { return m_IsEnabled; }

    [SerializeField]
    private float m_Offset;
    public void SetOffset(float offset) { m_Offset = offset; }
    private Vector3 ForwardOffset() { return this.transform.forward * m_Offset; }

    private LineRenderer m_Laser;
    [SerializeField]
    private LayerMask m_LaserLayer;
    [SerializeField]
    private float m_MaxLaserDistance = 250.0f;

    private PortalButton m_PressedButton;

    // Unity Awake
    void Awake() {
        m_Laser = GetComponent<LineRenderer>();
    }

    // Método para activar el laser
    public void EnableLaser() {
        m_IsEnabled = true;
        RaycastHit l_RaycastHit;
        float l_LaserDistance = m_MaxLaserDistance;
        Ray l_Ray = new Ray(m_Laser.transform.position + ForwardOffset(), m_Laser.transform.forward);
        // Raycast
        if (Physics.Raycast(l_Ray, out l_RaycastHit, m_MaxLaserDistance, m_LaserLayer.value)) {
            l_LaserDistance = Vector3.Distance(m_Laser.transform.position, l_RaycastHit.point);

            // Raycast Collisions
            if (l_RaycastHit.collider.tag == "RCube") {
                l_RaycastHit.collider.GetComponent<RefactionCube>().CreateRefaction();
            }
            if (l_RaycastHit.collider.tag == "Portal") {
                l_RaycastHit.collider.GetComponent<RefactionPortal>().CreateRefaction(l_RaycastHit.point, this.transform.forward);
            }
            if (l_RaycastHit.collider.tag == "Player") {
                l_RaycastHit.collider.GetComponent<PlayerDeath>().Kill();
            }
            if (l_RaycastHit.collider.tag == "Turret") {
                l_RaycastHit.collider.GetComponent<Turret>().DestroyIt();
            }
            if (l_RaycastHit.collider.tag == "Button") {
                m_PressedButton = l_RaycastHit.collider.gameObject.GetComponent<PortalButton>();
                if (m_PressedButton.CanLaserPress()) {
                    m_PressedButton.OnPressed();
                }
            }

        }

        // Set Distance
        m_Laser.SetPosition(0, new Vector3(0.0f, 0.0f, m_Offset));
        m_Laser.SetPosition(1, new Vector3(0.0f, 0.0f, l_LaserDistance));
    }

    // Método para "apagar" el laser
    public void DisableLaser() {
        m_IsEnabled = false;
        m_Laser.SetPosition(0, Vector3.zero);
        m_Laser.SetPosition(1, Vector3.zero);
        if (m_PressedButton != null)
            m_PressedButton.OnReleased();
    }

}
