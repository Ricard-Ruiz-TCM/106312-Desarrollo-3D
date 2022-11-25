using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerJump : MonoBehaviour {

    [SerializeField]
    private bool m_Locked;

    // Chracter Controller
    private CharacterController m_CharacterController;

    [SerializeField, Header("Jump Control:")]
    private bool m_Jumping;
    public bool IsJumping() { return m_Jumping; }
    [SerializeField]
    private bool m_Falling;
    public bool IsFalling() { return m_Falling; }
    [SerializeField]
    private bool m_OnGround;
    public bool IsGrounded() { return m_OnGround; }

    [SerializeField, Header("Jump Str & Time:")]
    private float m_JumpStr;

    // Control
    private float m_TimeOnAir;
    [SerializeField]
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

        m_TimeOnAir = 0.0f;
        m_VerticalSpeed = 0.0f;
    }

    // Unity Update
    void Update() {
        if (m_Locked)
            return;

        // Check si estoy Jumping / Falling
        m_Jumping = (m_VerticalSpeed != 0.0f) && (!m_OnGround);
        m_Falling = (m_VerticalSpeed < 0.0f) && (!m_OnGround);

        // Gravedad + check colisión con el suelo
        GroundCollision(PlayerGravity());
    }

    // Método para saltar, simplemente seteamos la velocidad vertical
    public void Jump() {
        if (CanJump())
            m_VerticalSpeed = m_JumpStr;
    }

    // Método que checka si podemos saltar
    // Out: bool -> (true -> podemos saltar | false -> no podemos saltar)
    public bool CanJump() {
        return (IsGrounded());
    }

    // Calc de la gravedad + aplicación del movimiento
    // Out: CollisionFlags -> Resultado del método Move(···) del CharacterController
    private CollisionFlags PlayerGravity() {
        Vector3 l_Movement = Vector3.zero;
        m_VerticalSpeed += Physics.gravity.y * Time.deltaTime * 2.0f;
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

    // Método para activar el CharacterController
    public void EnableJump() {
        m_Locked = false;
    }

    // Método para desactivar el CharacterController
    public void DisableJump() {
        m_Locked = true;
    }

}
