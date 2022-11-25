using UnityEngine;

public class Turret : MonoBehaviour {

    [SerializeField]
    private bool m_LaserAvaliable;

    private Laser m_Laser;
    [SerializeField]
    private float m_MaxRotAngle = 25.0f;

    // Unity Awake
    void Awake() {
        m_Laser = GetComponentInChildren<Laser>();
    }

    // Unity Start
    void Start() {
        m_LaserAvaliable = true;
    }

    // Unity Update
    void Update() {
        // Si no tenemos laser, no comprobamos para activarlo
        if (!m_LaserAvaliable)
            return;

        if (Vector3.Dot(transform.up, Vector3.up) > Mathf.Cos(m_MaxRotAngle * Mathf.Deg2Rad)) {
            m_Laser.EnableLaser();
        } else {
            m_Laser.DisableLaser();
        }
    }

    // Método para destuir la torreta por el lsaer
    public void DestroyIt() {
        GameObject.Destroy(this.gameObject);
        uCore.Audio.Play2DSFX("turret", this.transform.position);
    }

    // Método para desactivar el laser
    public void DisableLaser() {
        m_LaserAvaliable = false;
        m_Laser.DisableLaser();
    }

    // * --------------------------------------------------------------------------------- *
    // | - COLLISION --------------------------------------------------------------------- |
    // V --------------------------------------------------------------------------------- V

    private void OnCollisionEnter(Collision collision) {
        // Collision Turret
        if (collision.collider.tag == "Turret") {
            DisableLaser();
        }
        // Collision Companion
        else if (collision.collider.tag == "Companion") {
            DisableLaser();
        }
        // Collision con DstroySurface
        else if (collision.collider.tag == "DestroyZone") {
            DestroyIt();
        }
    }

    private void OnTriggerExit(Collider other) { }

    // A --------------------------------------------------------------------------------- A
}
