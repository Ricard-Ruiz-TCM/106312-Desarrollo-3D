using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Attachable : MonoBehaviour {

    [SerializeField]
    private bool m_Attached;

    // RBody
    private Rigidbody m_Rigidbody;
    [SerializeField, Header("Offset del TP Con el portal:")]
    private float m_OffsetPortalTeleport = 2.0f;

    // ExitPortal
    private Portal m_ExitPortal;

    // Unity Start
    void Start() {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Método para establecer el estado de agarre
    // In: bool attach -> (true -> estoy cogido | false -> no estoy cogido)
    public void SetAttach(bool attach) {
        m_Attached = attach;
    }

    // Método para teletransportar el objeto attached
    // In: Portal portal -> Portal con el que hemos colisionado
    void Teleport(Portal portal) {
        // Comprobamos que podamos teletransportarnos primero
        if ((portal == m_ExitPortal) || (m_Attached))
            return;

        // Si el mirror esta desactivado, nos vamos
        if (!portal.MirrorON())
            return;

        // Transform posición
        Vector3 l_LocalPosition = portal.Other().InverseTransformPoint(transform.position);
        Vector3 l_Direction = portal.Other().transform.InverseTransformDirection(transform.forward);
        // Transform rotación
        Vector3 l_LocalVelocity = portal.Other().transform.InverseTransformDirection(m_Rigidbody.velocity);
        Vector3 l_WorldVelocity = portal.Mirror().transform.TransformDirection(l_LocalVelocity);

        // Teletransportamos
        m_Rigidbody.isKinematic = true;
        transform.forward = portal.Mirror().transform.TransformDirection(l_Direction);

        // Guardamos velocidad del objeto
        Vector3 l_WorldVelocityNormalized = l_WorldVelocity.normalized;
        // Colocamos en el mundo

        transform.position = portal.Mirror().transform.TransformPoint(l_LocalPosition) + l_WorldVelocityNormalized * m_OffsetPortalTeleport;
        float l_Scale = portal.Mirror().CurrentScale();
        transform.localScale = new Vector3(l_Scale, l_Scale, l_Scale);

        // Impulsamos un vez teleransportado
        m_Rigidbody.isKinematic = false;
        m_Rigidbody.velocity = l_WorldVelocity;
        m_ExitPortal = portal.Mirror();
    }

    // * --------------------------------------------------------------------------------- *
    // | - COLLISION --------------------------------------------------------------------- |
    // V --------------------------------------------------------------------------------- V

    // OnTriggerEnter
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Portal") {
            Teleport(other.GetComponent<Portal>());
        }
    }

    // OnTriggerExit
    void OnTriggerExit(Collider other) {
        if (other.tag == "Portal") {
            if (other.GetComponent<Portal>() == m_ExitPortal) {
                m_ExitPortal = null;
            }
        }

    }

    // OnCollisionEnter
    void OnCollisionEnter(Collision other) {
        if (other.collider.tag == "DestroyZone") {
            GameObject.Destroy(this.gameObject);
        }
    }

    // OnCollisionExit
    void OnCollisionExit(Collision other) { }

    // A --------------------------------------------------------------------------------- A

}