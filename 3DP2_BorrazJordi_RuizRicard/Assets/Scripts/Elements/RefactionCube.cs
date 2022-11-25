using UnityEngine;

public class RefactionCube : MonoBehaviour {

    [SerializeField]
    private bool m_RefactionEnabled;

    private Laser m_Laser;

    // Unity Awake
    void Awake() {
        m_Laser = GetComponentInChildren<Laser>();
    }

    // Unity Start
    void Start() {
        m_RefactionEnabled = false;
    }

    // Unity Update
    void Update() {
        if (m_RefactionEnabled) {
            m_Laser.EnableLaser();
        } else {
            m_Laser.DisableLaser();
        }

        m_RefactionEnabled = false;
    }

    // Método para crear una refracción del laser
    public void CreateRefaction() {
        if (m_RefactionEnabled)
            return;
        m_RefactionEnabled = true;
    }

}
