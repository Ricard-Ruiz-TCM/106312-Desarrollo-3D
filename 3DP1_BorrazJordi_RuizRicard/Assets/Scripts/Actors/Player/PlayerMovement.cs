using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    // Player Yaw
    private float m_Yaw;
    [SerializeField, Header("Yaw:")]
    private bool m_YawInverted;
    [SerializeField]
    private float m_YawRotationSpeed;
    public void ReverseYaw() { m_YawInverted = !m_YawInverted; }

    // Player Pitch
    private float m_Pitch;
    [SerializeField, Header("Pitch:"), Space(5)]
    private bool m_PitchInverted;
    [SerializeField]
    private float m_PitchRotationSpeed;
    private float m_MaxPitch, m_MinPitch;
    public void ReversePitch() { m_PitchInverted = !m_PitchInverted; }

    // Pitch Controller
    [SerializeField]
    private Transform m_PitchController;

    // Character Controller
    private CharacterController m_CharacterController;
    [SerializeField, Header("Speed:"), Space(5)]
    private float m_Speed;
    public float Speed() { return m_Speed; }
    [SerializeField]
    private float m_FastSpeedMultiplier;
    public float SpeedMultiplier() { return m_FastSpeedMultiplier; }

    // Camera
    [SerializeField, Header("Camera."), Space(5)]
    private Camera m_Camera;
    [SerializeField]
    private float m_SmoothFOV;
    [SerializeField]
    private float m_FastMovementFOV;
    [SerializeField]
    private float m_NormalMovementFOV;

    // Unity Awake
    void Awake() {
        m_CharacterController = GetComponent<CharacterController>();
    }

    // Unity Start
    void Start() {
        m_Yaw = transform.rotation.y;
        m_Pitch = m_PitchController.transform.localRotation.x;
    }

    // Set de la info necesaria desde un PlayerData
    // In: PlayerData data -> Información
    public void SetData(PlayerData data) {
        // Movement
        m_YawRotationSpeed = data.YawRotationSpeed;
        m_PitchRotationSpeed = data.PitchRotationSpeed;
        m_MaxPitch = data.MaxPitch;
        m_MinPitch = data.MinPitch;
        m_Speed = data.SpeedBase;
        m_FastSpeedMultiplier = data.SpeedMultiplier;

        // Camera
        m_SmoothFOV = data.SmoothFOV;
        m_NormalMovementFOV = data.NormalSpeedFOV;
        m_FastMovementFOV = data.FastSpeedFOV;
    }

    // Método para mover, rotar y ajustar fov
    // In: Vector3 dir -> dirección de movimiento
    // In: float speed -> velocidad 
    // In: Vector2 mouse -> Delta del movimiento del ratón
    // Out: bool -> (true -> se ha movido | false -> no se ha movido)
    public bool Move(Vector3 dir, float speed, Vector2 mouse) {
        CameraFOV(speed);
        PlayerRotation(mouse);
        PlayerMove(dir, speed);

        return (dir != Vector3.zero);
    }

    // Calc de la FOV de la camara
    // In: float speed -> Velocidad del jugador
    private void CameraFOV(float speed) {
        float l_FOV = m_Camera.fieldOfView;
        float l_NextFov = m_NormalMovementFOV;

        // Comprobamos si la velocidad es 
        if (speed == m_Speed)
            l_FOV = Mathf.Lerp(l_FOV, m_FastMovementFOV, m_SmoothFOV);

        // Calc + Set del FOV
        l_FOV = Mathf.Lerp(l_FOV, l_NextFov, m_SmoothFOV);
        m_Camera.fieldOfView = l_FOV;
    }

    // Calc de la rotación según el Input del ratón
    // In: Vector2 deltaMouse -> Movimiento delta del ratón
    private void PlayerRotation(Vector2 deltaMouse) {
        // Calc de Yaw y Pitch
        m_Yaw += deltaMouse.x * m_YawRotationSpeed * Time.deltaTime * (m_YawInverted ? -1.0f : 1.0f);
        m_Pitch += deltaMouse.y * m_PitchRotationSpeed * Time.deltaTime * (m_PitchInverted ? -1.0f : 1.0f);

        // Clamp con los límites del Pitch
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);

        // Set de las rotaciones al player y al Pitch Controller
        transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
        m_PitchController.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);
    }

    // Movimiento del jugador según Input y el deltaTime
    // In: Vector3 dir -> Direción
    // In: float speed -> Velocidad
    private void PlayerMove(Vector3 dir, float speed) {
        m_CharacterController.Move(dir * speed * Time.deltaTime);
    }

}
