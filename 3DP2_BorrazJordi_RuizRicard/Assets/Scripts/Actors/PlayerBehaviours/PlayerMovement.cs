using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    private bool m_Locked;

    // Player Yaw
    private float m_Yaw;
    [SerializeField, Header("Yaw & Pitch:")]
    private bool m_YawInverted;
    [SerializeField]
    private float m_YawRotationSpeed;
    public void ReverseYaw() { m_YawInverted = !m_YawInverted; }

    // Player Pitch
    private float m_Pitch;
    [SerializeField, Space(10)]
    private bool m_PitchInverted;
    [SerializeField]
    private float m_PitchRotationSpeed;
    [SerializeField]
    private float m_MaxPitch, m_MinPitch;
    public void ReversePitch() { m_PitchInverted = !m_PitchInverted; }

    // Pitch Controller
    [SerializeField]
    private Transform m_PitchController;
    public Transform PitchCtrl() { return m_PitchController; }

    // Character Controller
    private CharacterController m_CharacterController;
    [SerializeField, Header("Velocidad:")]
    private float m_Speed;
    public float Speed() { return m_Speed; }
    [SerializeField]
    private float m_FastSpeedMultiplier;
    public float SpeedMultiplier() { return m_FastSpeedMultiplier; }

    // Camera
    [SerializeField, Header("Camera Control:")]
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
        m_Locked = false;
        m_Yaw = transform.rotation.y;
        m_Pitch = m_PitchController.transform.localRotation.x;
    }

    // M�todo para mover, rotar y ajustar fov
    // In: Vector3 dir -> direcci�n de movimiento
    // In: float speed -> velocidad 
    // In: Vector2 mouse -> Delta del movimiento del rat�n
    // Out: bool -> (true -> se ha movido | false -> no se ha movido)
    public bool Move(Vector3 dir, float speed, Vector2 mouse) {
        if (m_Locked)
            return false;

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

    // Calc de la rotaci�n seg�n el Input del rat�n
    // In: Vector2 deltaMouse -> Movimiento delta del rat�n
    private void PlayerRotation(Vector2 deltaMouse) {
        // Calc de Yaw y Pitch
        m_Yaw += deltaMouse.x * m_YawRotationSpeed * Time.deltaTime * (m_YawInverted ? -1.0f : 1.0f);
        m_Pitch += deltaMouse.y * m_PitchRotationSpeed * Time.deltaTime * (m_PitchInverted ? -1.0f : 1.0f);

        // Clamp con los l�mites del Pitch
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);

        // Set de las rotaciones al player y al Pitch Controller
        transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
        m_PitchController.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);
    }

    // Movimiento del jugador seg�n Input y el deltaTime
    // In: Vector3 dir -> Direci�n
    // In: float speed -> Velocidad
    private void PlayerMove(Vector3 dir, float speed) {
        m_CharacterController.Move(dir * speed * Time.deltaTime);
    }

    // M�todo par establecer un Yaw fijo
    // In: float newYaw -> neuvo valor para el yaw
    public void SetYaw(float newYaw) {
        m_Yaw = newYaw;
    }

    // M�todo para activar el CharacterController
    public void EnableMovement() {
        m_Locked = false;
        m_CharacterController.enabled = true;
    }

    // M�todo para desactivar el CharacterController
    public void DisableMovement() {
        m_Locked = true;
        m_CharacterController.enabled = false;
    }

    // M�todo para alterar el bloqueo del moviemiento
    public void ToggleMovement() {
        m_Locked = !m_Locked;
    }

}
