using UnityEngine;

public class PlayerCheckPoint : MonoBehaviour {

    // Transform
    private Vector3 m_Scale;
    private Vector3 m_Position;
    private Quaternion m_Rotation;

    // Player
    private CharacterController m_CharContr;

    // Unity Awake
    void Awake() {
        m_CharContr = GetComponent<CharacterController>();
    }

    // Unity Start
    void Start() {
        SetCheckPoint(this.transform.position);
    }

    // Método par aguardar la información relevante al checkpoint
    // In: Vector3 position -> Posición donde el player "respawneara"
    public void SetCheckPoint(Vector3 position) {
        m_Position = position;
        m_Scale = this.transform.localScale;
        m_Rotation = this.transform.rotation;
    }

    // Método para respawnear
    public void RestoreCheckPoint() {
        m_CharContr.enabled = false;

        this.transform.localScale = m_Scale;
        this.transform.position = m_Position;
        this.transform.rotation = m_Rotation;

        m_CharContr.enabled = true;
    }

}
