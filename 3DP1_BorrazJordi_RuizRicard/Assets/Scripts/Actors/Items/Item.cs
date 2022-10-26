using UnityEngine;

public abstract class Item : MonoBehaviour {

    [SerializeField, Header("Item:")]
    private bool m_Near;
    [SerializeField]
    private float m_Distance;

    [SerializeField, Header("Animations:")]
    private AnimationClip m_ClipFar;
    [SerializeField]
    private AnimationClip m_ClipNear;

    // Animation
    private Animation m_Animation;
    // Player
    private Player m_Player;

    // OnEnable
    void OnEnable() {
        RespawnButton.OnPlayerRespawn += Activate;
    }

    // OnDisable
    void OnDisable() {
        RespawnButton.OnPlayerRespawn -= Activate;
    }

    // Unity Awake
    void Awake() {
        m_Animation = GetComponent<Animation>();
        m_Player = uCore.GameManager.GetPlayer();
    }

    // Unity Start
    void Start() {
        m_Near = false;
    }

    // Unity Update
    void Update() {
        if (Vector3.Distance(this.transform.position, m_Player.transform.position) < m_Distance) {
            if (m_Near)
                return;

            m_Near = true;
            m_Animation.Play(m_ClipNear.name);
        } else {
            if (!m_Near)
                return;

            m_Near = false;
            m_Animation.Play(m_ClipFar.name);
        }
    }

    // Método abstracto Pick para que pickear items
    // In: Player player -> el jugador principal
    public abstract void Pick(Player player);

    // Desactiva el objeto
    protected void Deactivate() {
        this.gameObject.SetActive(false);
    }

    // ReActivar el objeto
    protected void Activate() {
        this.gameObject.SetActive(true);
    }
}

