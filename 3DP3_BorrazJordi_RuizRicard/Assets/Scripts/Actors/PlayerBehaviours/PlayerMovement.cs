using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    // Character Controller
    private CharacterController m_CharacterController;

    [SerializeField, Header("Speeds:")]
    private float m_WalkSpeed;
    public float WalkSpeed() { return m_WalkSpeed; }

    [SerializeField]
    private float m_RunSpeed;
    public float RunSpeed() { return m_RunSpeed; }

    [SerializeField]
    private float m_PowerStarSpeed;
    public float PowerStarSpeed() { return m_PowerStarSpeed; }

    [SerializeField]
    private bool m_Crounch;
    public bool Crounching() { return m_Crounch; }

    [SerializeField]
    private float m_RotSmooth;

    // Jump Sys
    private PlayerJump Jump;

    // Unity Awake
    void Awake() {
        m_CharacterController = GetComponent<CharacterController>();
        Jump = GetComponent<PlayerJump>();
    }

    // Unity Start
    void Start() { }

    // M�todo para mover, rotar y ajustar fov
    // In: Vector3 dir -> direcci�n de movimiento
    // In: float speed -> velocidad 
    // In: Vector3 des -> destino de la rotaic�n objetivo
    // Out: bool -> (true -> se ha movido | false -> no se ha movido)
    public bool Move(Vector3 dir, float speed, Vector3 des) {
        if (Crounching())
            return false;

        PlayerMove(dir, speed);
        if ((speed != 0.0f) && (dir != Vector3.zero)) {
            if (!Jump.OnWall()) {
                PlayerRotation(des);
            }
        }

        return (dir != Vector3.zero);
    }

    // M�todo que agacha al player y contorla si esta agachado
    // In: bool active (true -> se sta pulsando el ilnput | false -> no hay input)
    public void Crounch(bool active) {
        m_Crounch = active;
    }

    // Calc de la rotaci�n seg�n el Input del rat�nr h
    // In: Vector3 des -> destino de rotaci�n
    public void PlayerRotation(Vector3 des) {
        Quaternion l_LookRotation = Quaternion.LookRotation(des);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, l_LookRotation, m_RotSmooth);
    }

    // Movimiento del jugador seg�n Input y el deltaTime
    // In: Vector3 dir -> Direci�n
    // In: float speed -> Velocidad
    private void PlayerMove(Vector3 dir, float speed) {
        m_CharacterController.Move(dir * speed * Time.deltaTime);
    }

}
