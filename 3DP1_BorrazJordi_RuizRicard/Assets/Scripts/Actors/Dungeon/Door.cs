using UnityEngine;

public class Door : MonoBehaviour {

    // Animations
    private Animation m_Animation;
    private AnimationClip m_ClipOpen;
    private AnimationClip m_ClipClose;

    [SerializeField]
    private bool m_Locked;
    [SerializeField]
    private bool m_Opened;

    [SerializeField]
    private int m_DoorID;
    public int DoorID() { return m_DoorID; }

    [SerializeField]
    private GameObject m_InteractUI;

    // Unity Awake
    void Awake() {
        m_Animation = GetComponent<Animation>();

        m_InteractUI = transform.Find("Input").gameObject;
        m_InteractUI.SetActive(false);
    }

    // Unity Start
    void Start() {
        // Load de las animaciones
        m_ClipOpen = Resources.Load<AnimationClip>("Animations/Door/DoorOpen");
        m_ClipClose = Resources.Load<AnimationClip>("Animations/Door/DoorClose");

        // Set de las puertas a cerrado
        m_Opened = false;
    }

    // Intena abrir la puerta por proximidad, si no esta lockeada
    public void ProximityOpen() {
        if (m_Locked)
            m_InteractUI.SetActive(true);

        if (!m_Locked)
            Open();
    }

    // Desbloquea la puerta
    public void UnlockDoor() {
        m_Locked = false;
        m_InteractUI.SetActive(false);
        Open();
    }

    // Abre la puerta
    public void Open() {
        if (m_Opened)
            return;

        m_Opened = true;
        m_Animation.CrossFadeQueued(m_ClipOpen.name);
    }

    // Cierra una puerta abierta, así del chill
    public void Close() {
        if (m_Locked)
            m_InteractUI.SetActive(false);

        if (!m_Opened)
            return;

        m_Opened = false;
        m_Animation.CrossFadeQueued(m_ClipClose.name);
    }


    // * --------------------------------------------------------------------------------- *
    // | - COLLISION --------------------------------------------------------------------- |
    // V --------------------------------------------------------------------------------- V

    private void OnTriggerEnter(Collider other) {
        // PlayerEnemy Collision
        if ((other.tag == "Player") || (other.tag == "Enemy")) {
            ProximityOpen();
        }
    }

    private void OnTriggerExit(Collider other) {
        // PlayerEnemy DesCollision
        if ((other.tag == "Player") || (other.tag == "Enemy")) {
            Close();
        }
    }

    // A --------------------------------------------------------------------------------- A

}

