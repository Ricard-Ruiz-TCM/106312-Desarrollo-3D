using UnityEngine;

public class Door : MonoBehaviour, IButtonAction {

    [SerializeField]
    private bool m_Open;

    [SerializeField, Header("Door ID:")]
    private int m_ID;
    [SerializeField]
    private bool m_Locked;

    [SerializeField, Header("Animations:")]
    private AnimationClip m_OpenClip;
    [SerializeField]
    private AnimationClip m_CloseClip;

    // Animation
    private Animation m_Animation;

    // Unity Awake
    void Awake() {
        m_Animation = GetComponent<Animation>();
    }

    // M�todos de IButtonAction
    public void OnPressed() {
        m_Locked = false;
        OpenDoor();
    }

    // M�todos de IButtonAction
    public void OnReleased() { }

    // M�todo para abrir una puerta
    public void OpenDoor() {
        if (m_Open || m_Locked)
            return;

        uCore.Audio.Play2DSFX("openDoor", this.transform.position);
        m_Open = true;
        m_Animation.CrossFade(m_OpenClip.name);
    }

    // M�todo para cerrar la puerta
    public void CloseDoor() {
        if (!m_Open)
            return;

        m_Open = false;
        m_Animation.CrossFade(m_CloseClip.name);
    }

    // * --------------------------------------------------------------------------------- *
    // | - COLLISION --------------------------------------------------------------------- |
    // V --------------------------------------------------------------------------------- V

    // OnTriggerEnter
    void OnTriggerEnter(Collider other) {
        // Collision
        if (other.tag == "Player") {
            if (m_Locked) {
                if (other.GetComponent<PlayerInventory>().HaveKey(m_ID)) {
                    other.GetComponent<PlayerInventory>().RemoveKey(m_ID);
                    m_Locked = false;
                }
            }
            OpenDoor();
        }
    }

    // OnTriggerExit
    void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            CloseDoor();
        }
    }

    // A --------------------------------------------------------------------------------- A

}
