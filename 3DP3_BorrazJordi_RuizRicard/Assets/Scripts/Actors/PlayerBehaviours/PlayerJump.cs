using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerJump : MonoBehaviour {

    // Chracter Controller
    private CharacterController m_CharContr;

    [SerializeField, Header("Jump Control:")]
    private int m_Jumps;
    public int Jumps() { return m_Jumps; }
    [SerializeField]
    private float m_ExtraJumpSpeedLimit;
    [SerializeField]
    private int m_MaxJumps;
    [SerializeField]
    private bool m_Jumping;
    public bool IsJumping() { return m_Jumping; }
    public bool IsRising() { return m_JumpSpeed.y > 0.0f; }
    public bool IsForcedJumping() { return m_JumpSpeed.y > 0.0f; }
    [SerializeField]
    private bool m_Falling;
    public bool IsFalling() { return m_Falling; }
    [SerializeField]
    private bool m_OnGround;
    public bool IsGrounded() { return m_OnGround; }

    [SerializeField]
    private bool m_OnWall;
    public bool OnWall() { return m_OnWall; }
    [SerializeField]
    private float m_WallDotAngle;

    [SerializeField, Header("Jump Str & Time:")]
    private float m_JumpStr;

    // Control
    private float m_TimeOnAir;
    [SerializeField]
    private float m_CoyoteTime;
    [SerializeField]
    private float m_ExtraJumpTime;
    [SerializeField]
    private Vector3 m_JumpSpeed;
    [SerializeField, Header("Gravity:")]
    private float m_GravityMult;

    // Tiempo temp;
    private float m_Time;

    private PlayerMovement Movement;

    //Audio jump
    private bool m_canPlayAudioJump;
    // Unity Awake
    void Awake() {
        m_CharContr = GetComponent<CharacterController>();
        Movement = GetComponent<PlayerMovement>();
    }

    // Unity Start
    void Start() {
        m_Jumping = false;
        m_Falling = false;
        m_OnGround = true;
        m_canPlayAudioJump = true;
        m_TimeOnAir = 0.0f;
        m_JumpSpeed.y = 0.0f;
        m_Jumps = 0;
    }

    // Unity Update
    void Update() {
        m_Time += Time.deltaTime;
        // Check si estoy Jumping / Falling
        m_Jumping = (m_JumpSpeed.y > 0.0f) && (!m_OnGround);
        m_Falling = (m_JumpSpeed.y < 0.0f) && (!m_OnGround);

        // Gravedad + check colisión con el suelo
        GroundCollision(PlayerGravity());
    }

    // Método para saltar, simplemente seteamos la velocidad vertical
    public void Jump() {
        if (CanJump()) {
            IJump();
        }
    }

    // Método para hacer el LongJump
    public void LongJump() {
        if ((m_Jumps >= m_MaxJumps) || (m_Time >= m_ExtraJumpTime)) {
            m_Jumps = 0;
        }
        m_Jumps++;
        m_JumpSpeed = this.transform.forward * m_JumpStr * 3.5f;
        m_JumpSpeed.y = m_JumpStr * 2.5f;
    }

    // Método para forzar un salto, sin importar que 
    public void ForceJump() {
        IJump();
    }

    // Método para hacer un "salto" después de recibir golpe
    public void DamageJump() {
        m_JumpSpeed.y = m_JumpStr * 1.5F;
        m_JumpSpeed.x = (Random.Range(0, 2) % 2) * m_JumpStr * 1.5f;
    }

    // Método interno para ejecutar el salto
    // In: bool extra -> indica si es un salto esxtra o no
    private void IJump() {
        if ((m_Jumps >= m_MaxJumps) || (m_Time >= m_ExtraJumpTime)) {
            m_Jumps = 0;
        }
        m_Jumps++;
        if(m_canPlayAudioJump)
        {
            uCore.Particles.PlayParticlesOnce("CircleSmoke", this.transform.position);
            if (m_Jumps==1)
            uCore.Audio.Play2DSFX("single-jump-1", this.transform.position);
            if (m_Jumps == 2)
                uCore.Audio.Play2DSFX("single-jump-2", this.transform.position);
            if (m_Jumps == 3)
            {
                uCore.Audio.Play2DSFX("single-jump-3", this.transform.position);
                uCore.Particles.PlayParticlesOnce("Jump3r",this.transform.position);
            }
            m_canPlayAudioJump = false;
        }
        m_JumpSpeed.y = m_JumpStr + (Jumps() * 2.0f);
        if (m_OnWall) {
            m_OnWall = false;
            m_JumpSpeed.x = m_JumpStr;
        }
    }

    // Método que checka si podemos saltar
    // Out: bool -> (true -> podemos saltar | false -> no podemos saltar)
    public bool CanJump() {
        return (IsGrounded() || OnWall());
    }

    // Método que checka si puedo saltar otra vez
    // Out: bool -> (true -> podemos saltar | false -> no podemos saltar)
    public bool CanJumpAgain() {
        // No me quedan saltos
        if (m_MaxJumps <= 0)
            return false;

        // Mi velocidad no es negativa
        if (m_JumpSpeed.y >= 0.0f)
            return false;

        // Mi velocidad supera el límite
        if (Mathf.Abs(m_JumpSpeed.y) > m_ExtraJumpSpeedLimit)
            return false;

        // Si no estoy cayendo, no puedo volver a saltar
        if (!IsFalling())
            return false;

        return true;
    }

    public void AttachWall(GameObject wall = null) {
        if (!m_Falling)
            return;

        m_Jumps = 0;
        m_OnWall = true;
        m_JumpSpeed.y = 0.0f;
    }

    public void DeAttachWall() {
        m_OnWall = false;
    }

    public void CheckAttach(Collider wall) {
        if (!m_Falling)
            return;

        m_OnWall = Vector3.Dot(this.transform.forward, -wall.transform.forward) >= m_WallDotAngle;
        Movement.PlayerRotation(-wall.transform.forward);
    }

    // Calc de la gravedad + aplicación del movimiento
    // Out: CollisionFlags -> Resultado del método Move(···) del CharacterController
    private CollisionFlags PlayerGravity() {
        Vector3 l_Movement = Vector3.zero;
        m_JumpSpeed.y += Physics.gravity.y * Time.deltaTime * (m_Falling ? 2.0f : 1.0f) * (m_OnWall ? 0.05f : 1.0f);
        l_Movement.y = m_JumpSpeed.y * Time.deltaTime;
        if (!IsGrounded()) {
            m_JumpSpeed.x += (Physics.gravity.y * Time.deltaTime) * 0.25f * (m_JumpSpeed.x > 0 ? 1.0f : -1.0f);
            l_Movement.x = m_JumpSpeed.x * Time.deltaTime;
            m_JumpSpeed.z += (Physics.gravity.y * Time.deltaTime) * 0.25f * (m_JumpSpeed.z > 0 ? 1.0f : -1.0f);
            l_Movement.z = m_JumpSpeed.z * Time.deltaTime;
        }
        return m_CharContr.Move(l_Movement);
    }

    // Comprueba las colisiones con el suelo
    // In: CollisionFlags col -> Resultado del método Move(···) del CharacterController
    private void GroundCollision(CollisionFlags col) {
        // Colisón con el techo
        if ((col & CollisionFlags.Above) != 0 && m_JumpSpeed.y > 0.0f) {
            m_JumpSpeed.y = 0.0f;
        }

        // Colisón con el suelo
        if ((col & CollisionFlags.Below) != 0) {
            if (!m_OnGround) {
                m_Time = 0.0f;
            }
            // ----------
            m_JumpSpeed = Vector3.zero;
            m_TimeOnAir = 0.0f;
            m_OnGround = true;
            m_OnWall = false;
            m_canPlayAudioJump = true;
        }
        else {
            m_TimeOnAir += Time.deltaTime;
            // Check del Coyote Time
            if (m_TimeOnAir > m_CoyoteTime)
                m_OnGround = false;
        }
    }

}
