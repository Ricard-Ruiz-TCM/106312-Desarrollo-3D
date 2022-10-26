using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerJump : MonoBehaviour {

    // Chracter Controller
    private CharacterController m_CharacterController;

    [SerializeField, Header("Jump Str:")]
    private float m_JumpStr;

    [SerializeField, Header("Jump Control:")]
    private bool m_Jumping;
    public bool IsJumping() { return m_Jumping; }
    [SerializeField]
    private bool m_Falling;
    public bool IsFalling() { return m_Falling; }
    [SerializeField]
    private bool m_OnGround;
    public bool IsGrounded() { return m_OnGround; }

    private bool m_CanPlayJumpAudio;

    private float m_TimeOnAir;
    private float m_CoyoteTime;
    private float m_VerticalSpeed;

    // Unity Awake
    void Awake() {
        m_CharacterController = GetComponent<CharacterController>();
    }

    // Unity Start
    void Start() {
        m_Jumping = false;
        m_Falling = false;
        m_OnGround = true;
        m_CanPlayJumpAudio = true;

        m_TimeOnAir = 0.0f;
        m_VerticalSpeed = 0.0f;
    }

    // Unity Update
    void Update() {
        // Check si estoy Jumping / Falling
        m_Jumping = (m_VerticalSpeed != 0.0f) && (!m_OnGround);
        m_Falling = (m_VerticalSpeed < 0.0f) && (!m_OnGround);

        // Gravedad + check colisión con el suelo
        GroundCollision(PlayerGravity());
    }

    // Set de la info necesaria desde un PlayerData
    // In: PlayerData data -> Información
    public void SetData(PlayerData data) {
        m_JumpStr = data.JumptStr;
        m_CoyoteTime = data.CoyoteTime;
    }

    // Método para saltar, encargado de ser llamado solo por Player.cs D:
    // Out: bool -> (true -> El player salta | false -> el player no salta)
    public bool Jump() {
        if (!m_OnGround)
            return false;

        if (m_CanPlayJumpAudio) {
            m_CanPlayJumpAudio = false;
            uCore.Audio.PlaySFX("jump", this.transform);
        }

        m_VerticalSpeed = m_JumpStr;
        return true;
    }

    // Calc de la gravedad + aplicación del movimiento
    // Out: CollisionFlags -> Resultado del método Move(···) del CharacterController
    private CollisionFlags PlayerGravity() {
        Vector3 l_Movement = Vector3.zero;
        m_VerticalSpeed += Physics.gravity.y * Time.deltaTime;
        l_Movement.y = m_VerticalSpeed * Time.deltaTime;
        return m_CharacterController.Move(l_Movement);
    }

    // Comprueba las colisiones con el suelo
    // In: CollisionFlags col -> Resultado del método Move(···) del CharacterController
    private void GroundCollision(CollisionFlags col) {
        // Colisón con el techo
        if ((col & CollisionFlags.Above) != 0 && m_VerticalSpeed > 0.0f) {
            m_VerticalSpeed = 0.0f;
        }

        // Colisón con el suelo
        if ((col & CollisionFlags.Below) != 0) {
            if (!m_OnGround) {
                m_CanPlayJumpAudio = true;
            }
            // ----------
            m_VerticalSpeed = 0.0f;
            m_TimeOnAir = 0.0f;
            m_OnGround = true;

        } else {
            m_TimeOnAir += Time.deltaTime;
            // Check del Coyote Time
            if (m_TimeOnAir > m_CoyoteTime)
                m_OnGround = false;
        }
    }
}
