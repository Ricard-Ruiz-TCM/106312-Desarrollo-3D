using UnityEngine;

public class RefactionPortal : MonoBehaviour {

    [SerializeField]
    private bool m_RefactionEnabled;

    // Portal
    private Portal m_Portal;

    // Unity Awake
    void Awake() {
        m_Portal = GetComponent<Portal>();
    }

    // Unity Start
    void Start() {
        m_RefactionEnabled = false;
    }

    // Unity Update
    void Update() {
        if (m_RefactionEnabled) {
            m_Portal.Mirror().Laser().EnableLaser();
        } else {
            m_Portal.Mirror().Laser().DisableLaser();
        }

        m_RefactionEnabled = false;

    }

    // Método para crear una refracción del laser
    // In: Vector3 hitPos -> Posición de impacto del laser al portal
    // In: Vector3 otherLaserForward -> Dirección del laser que impacta, su forward
    public void CreateRefaction(Vector3 hitPos, Vector3 otherLaserForward) {
        if (m_RefactionEnabled)
            return;

        m_RefactionEnabled = true;
        // Cal de las locals
        Vector3 l_LocalPosition = m_Portal.Other().InverseTransformPoint(hitPos);
        Vector3 l_LocalDirection = m_Portal.Other().InverseTransformDirection(otherLaserForward);
        // Calc a WordlPos
        Vector3 l_WorldPosition = m_Portal.Mirror().transform.TransformPoint(l_LocalPosition);
        Vector3 l_WorldDirection = m_Portal.Mirror().transform.TransformDirection(l_LocalDirection);

        // Calculamos el offset para colocar el rayo
        Plane l_Plane = new Plane(transform.forward, transform.position);
        Ray l_Ray = new Ray(hitPos, otherLaserForward);
        float l_Distance = 0.0f;
        l_Plane.Raycast(l_Ray, out l_Distance);

        // Actualizamos la posición + offset del rayo
        m_Portal.Mirror().Laser().gameObject.SetActive(true);
        m_Portal.Mirror().Laser().SetOffset(l_Distance);
        m_Portal.Mirror().Laser().transform.position = l_WorldPosition;
        m_Portal.Mirror().Laser().transform.forward = l_WorldDirection;

    }
}