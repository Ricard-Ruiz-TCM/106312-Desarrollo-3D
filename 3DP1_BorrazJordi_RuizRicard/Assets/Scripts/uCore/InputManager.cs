using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour {

    // Enum que determina el timpo de Input D: (algo más "homemade")
    public enum INPUT_SCHEME {
        I_KEYBOARD, I_GAMEPAD
    }

    // Observer para saber caundo se cambia de Scheme a.k.a enchufo el mando je
    public static event Action<INPUT_SCHEME> OnChangeInput;

    // Componente New Input System
    [SerializeField]
    private PlayerInput m_Input;

    // Control del Scheme actual
    [SerializeField]
    private INPUT_SCHEME m_CurrentScheme;
    public INPUT_SCHEME Scheme() { return m_CurrentScheme; }
    public bool GamePad() { return (Scheme().Equals(INPUT_SCHEME.I_GAMEPAD)); }
    public bool Keyboard() { return (Scheme().Equals(INPUT_SCHEME.I_KEYBOARD)); }

    // Atributos
    private bool m_Left;
    private bool m_Right;
    private bool m_Forward;
    private bool m_Backward;
    private bool m_LeftShift;
    private bool m_Jump;
    private bool m_Shoot;
    private bool m_Aim;
    private bool m_Reload;
    private bool m_Interact;
    private Vector2 m_MouseMovement;
    private Vector2 m_MousePos;
    private bool m_LockAngle;
    private bool m_LockAim;
    private bool m_NextScene;

    // Métodos Check??
    public bool Left() { return m_Left; }
    public bool Right() { return m_Right; }
    public bool Forward() { return m_Forward; }
    public bool Backward() { return m_Backward; }
    public bool LShift() { return m_LeftShift; }
    public bool Jump() { return m_Jump; }
    public bool Shoot() { return m_Shoot; }
    public bool Aim() { return m_Aim; }
    public bool Reload() { return m_Reload; }
    public bool Interact() { return m_Interact; }
    public Vector2 MouseMovement() { return m_MouseMovement; }
    public Vector2 MousePos() { return m_MousePos; }
    public bool LockAngle() { return m_LockAngle; }
    public bool LockAim() { return m_LockAim; }
    public bool NextScene() { return m_NextScene; }

    // Unity Awake
    void Awake() {
        m_Input = GetComponent<PlayerInput>();
        m_CurrentScheme = INPUT_SCHEME.I_KEYBOARD;
    }

    // Unity Start
    void Start() { }

    // Unity Update
    void Update() { }

    // * ----------------------------------------------------------------------------------------------------------------------------------- *
    // | - Send Mesagges - PlayerInput Component ------------------------------------------------------------------------------------------- |
    // V ----------------------------------------------------------------------------------------------------------------------------------- V
    void OnControlsChanged() {
        if (m_Input.currentControlScheme.Equals("Gamepad")) m_CurrentScheme = INPUT_SCHEME.I_GAMEPAD;
        if (m_Input.currentControlScheme.Equals("Keyboard&Mouse")) m_CurrentScheme = INPUT_SCHEME.I_KEYBOARD;
        OnChangeInput?.Invoke(Scheme());
    }

    void OnLeft(InputValue value) {
        m_Left = value.isPressed;
    }

    void OnRight(InputValue value) {
        m_Right = value.isPressed;
    }

    void OnForward(InputValue value) {
        m_Forward = value.isPressed;
    }

    void OnBackward(InputValue value) {
        m_Backward = value.isPressed;
    }

    void OnLeftShift(InputValue value) {
        m_LeftShift = value.isPressed;
    }

    void OnJump(InputValue value) {
        m_Jump = value.isPressed;
    }

    void OnShoot(InputValue value) {
        m_Shoot = value.isPressed;
    }

    void OnAim(InputValue value) {
        m_Aim = value.isPressed;
    }

    void OnReload(InputValue value) {
        m_Reload = value.isPressed;
    }

    void OnInteract(InputValue value) {
        m_Interact = value.isPressed;
    }

    void OnMouseMovement(InputValue value) {
        m_MouseMovement = value.Get<Vector2>();
    }

    void OnMousePosition(InputValue value) {
        m_MousePos = value.Get<Vector2>();
    }

    void OnLockAngle(InputValue value) {
        m_LockAngle = value.isPressed;
    }

    void OnLockAim(InputValue value) {
        m_LockAim = value.isPressed;
    }

    void OnNextScene(InputValue value) {
        m_NextScene = value.isPressed;
    }

    // A ----------------------------------------------------------------------------------------------------------------------------------- A

}
