using UnityEngine;

public class PlayerCatch : MonoBehaviour {

    [SerializeField, Header("Attach Position:")]
    private Transform m_Position;

    [SerializeField, Header("Attached Object:")]
    private bool m_ObjectAttached;
    public bool IsObjectAttached() { return m_ObjectAttached; }
    [SerializeField]
    private Rigidbody m_Object;

    [SerializeField, Header("Throw Str:")]
    private float m_ThrowStr = 600.0f;

    [SerializeField, Header("Absorción de la portal Gun:")]
    private bool m_Pulling;
    public bool IsPullling() { return m_Pulling; }
    [SerializeField]
    private float m_PullSpeed = 60.0f;
    [SerializeField]
    private float m_PullMaxDistance = 10.0f;

    [SerializeField, Header("Control:")]
    private bool m_CanThrow;
    public bool CanThrow() { return m_CanThrow; }
    [SerializeField]
    private bool m_CanAttach;
    public bool CanAttach() { return m_CanAttach; }

    [SerializeField, Header("Atachable Objs. Layer:")]
    private LayerMask m_LayerMask;

    // Temp Rotation
    Quaternion m_ObjStartRotation;


    // Unity Start
    void Start() {
        m_CanAttach = true;
        m_CanThrow = false;
        m_ObjectAttached = false;
    }

    // Unity Update
    void Update() {
        if (IsPullling()) {
            PullObject();
        }
    }

    // Método para soltar el objeto sin utilizar fuerza
    public void DeAttach() {
        if (IsObjectAttached()) {
            ThrowAttachedObject(0.0f);
        }
    }

    // Método para intentar agarrar un objeto
    // Si no esta en la próximidad correcta, lo "absorve"
    public void Attach() {
        // Intentamos agarrar
        if (CanAttach()) {
            AttachObject();
        }
        // Si tenemos un objeto agarrado, lo lanzamos con fuerxa
        else if (IsObjectAttached()) {
            if (CanThrow()) {
                ThrowAttachedObject(m_ThrowStr);
            }
        }
    }

    // Método para activar la posibilidad de soltar un objeto
    public void EnableThrow() {
        if ((IsObjectAttached()) && (!IsPullling()))
            m_CanThrow = true;
    }

    // Habilita la posibilidad de coger un objeto
    public void EnableAttach() {
        m_CanAttach = true;
    }

    // Método para agarrar un objeto basado en raycast
    private void AttachObject() {
        RaycastHit l_RaycastHit;
        // Rayo desde el centro de la pantalla
        Ray l_Ray = GetComponent<Player>().Camera().ViewportPointToRay(new Vector3(.5f, .5f, 0));
        if (Physics.Raycast(l_Ray, out l_RaycastHit, m_PullMaxDistance, m_LayerMask.value)) {
            if (l_RaycastHit.collider.GetComponent<Attachable>() != null) {
                m_Object = l_RaycastHit.collider.GetComponent<Rigidbody>();
                m_Pulling = true;
                m_CanAttach = false;
                m_Object.isKinematic = true;
                m_Object.GetComponent<Attachable>().SetAttach(true);
                m_ObjStartRotation = l_RaycastHit.collider.transform.rotation;
            }
            if (l_RaycastHit.collider.tag == "Button") {
                PortalButton l_button = l_RaycastHit.collider.gameObject.GetComponent<PortalButton>();
                if (l_button.PlayerCanPress()) {
                    l_button.OnPressed();
                    l_button.OnReleased();
                }

            }
        }
    }

    // Método para hacer el Pull del objeto que hemos decidido agarrar
    private void PullObject() {
        // Si palma el objeto que intentamos agarrar, nos vamos
        if (m_Object == null)
            return;

        // Calculamos movimiento y dirección
        Vector3 l_EulerAngles = m_Position.rotation.eulerAngles;
        Vector3 l_Direction = m_Position.transform.position - m_Object.transform.position;
        float l_Distance = l_Direction.magnitude;
        float l_Movement = m_PullSpeed * Time.deltaTime;

        // Si el movimiento que hará es mayor a la distancia que tiene que recorrer
        // Significa que lo pillariamos
        if (l_Movement >= l_Distance) {
            m_Pulling = false;
            m_ObjectAttached = true;
            m_CanAttach = false;
            m_Object.transform.SetParent(m_Position);
            m_Object.transform.localPosition = Vector3.zero;
            m_Object.transform.localRotation = Quaternion.identity;
            // Solo si lo hemos cogido y no tenemos pulsado/hayamos pulsado recientemente
            // Podemos lanzarlo
            if (!uCore.Input.RMClickPress() && !uCore.Input.LMClickPress()) {
                m_CanThrow = true;
            }
        }
        // Si no, lo movemos en nuestra dirección/rotación
        else {
            l_Direction /= l_Distance;
            m_Object.MovePosition(m_Object.transform.position + l_Direction * l_Movement);
            m_Object.MoveRotation(Quaternion.Lerp(m_ObjStartRotation, Quaternion.Euler(0.0f, l_EulerAngles.y, l_EulerAngles.z), 1.0f - Mathf.Min(l_Distance / 1.5f, 1.0f)));

        }
    }

    // Método para lanzar el objeto agarrado
    // In: float force -> fuerza con la que lo lanzamos
    void ThrowAttachedObject(float force) {
        // Si palma el objeto que intentamos agarrar, nos vamos
        if (m_Object == null)
            return;

        // Lanzamos el objeto
        m_Object.transform.SetParent(uCore.GameManager.EntityContainer());
        m_Object.isKinematic = false;
        m_Object.AddForce(GetComponent<PlayerMovement>().PitchCtrl().forward * force);
        m_Object.GetComponent<Attachable>().SetAttach(false);
        m_Object = null;
        m_ObjectAttached = false;
        m_CanThrow = false;
    }
}
