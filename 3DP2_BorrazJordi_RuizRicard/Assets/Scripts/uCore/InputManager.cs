using System;
using System.Collections;
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
    private bool m_KeyA;
    private bool m_KeyD;
    private bool m_KeyW;
    private bool m_KeyS;
    private bool m_KeyE;
    private bool m_KeyF;
    private bool m_KeyO;
    private bool m_Space;
    private bool m_LMClickPress;
    private bool m_LMClickRelease;
    private bool m_RMClickPress;
    private bool m_RMClickRelease;
    private bool m_LeftShift;
    private Vector2 m_MouseDelta;
    private Vector2 m_MousePos;
    private float m_MouseScroll;

    // Métodos Check??
    public bool KeyA() { return m_KeyA; }
    public bool KeyD() { return m_KeyD; }
    public bool KeyW() { return m_KeyW; }
    public bool KeyS() { return m_KeyS; }
    public bool KeyE() { return m_KeyE; }
    public bool KeyF() { return m_KeyF; }
    public bool KeyO() { return m_KeyO; }
    public bool Space() { return m_Space; }
    public bool LMClickPress() { return m_LMClickPress; }
    public bool LMClickRelease() { return m_LMClickRelease; }
    public bool RMClickPress() { return m_RMClickPress; }
    public bool RMClickRelease() { return m_RMClickRelease; }
    public bool LShift() { return m_LeftShift; }
    public Vector2 MouseMovement() { return m_MouseDelta; }
    public Vector2 MousePos() { return m_MousePos; }
    public bool IsMouseScrolling() { return m_MouseScroll != 0.0f; }
    public float MouseScroll() { return m_MouseScroll; }

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

    void OnKeyA(InputValue value) {
        m_KeyA = value.isPressed;
    }

    void OnKeyD(InputValue value) {
        m_KeyD = value.isPressed;
    }

    void OnKeyW(InputValue value) {
        m_KeyW = value.isPressed;
    }

    void OnKeyS(InputValue value) {
        m_KeyS = value.isPressed;
    }

    void OnKeyE(InputValue value) {
        m_KeyE = value.isPressed;
    }

    void OnKeyF(InputValue value) {
        m_KeyF = value.isPressed;
    }

    IEnumerator OnKeyO(InputValue value) {
        m_KeyO = true;
        yield return new WaitForEndOfFrame();
        m_KeyO = false;
    }

    void OnSpace(InputValue value) {
        m_Space = value.isPressed;
    }

    void OnLMClickPress(InputValue value) {
        m_LMClickPress = value.isPressed;
    }

    IEnumerator OnLMClickRelease(InputValue value) {
        m_LMClickPress = false;
        m_LMClickRelease = !value.isPressed;
        yield return new WaitForEndOfFrame();
        m_LMClickRelease = false;
    }

    void OnRMClickPress(InputValue value) {
        m_RMClickPress = value.isPressed;
    }

    IEnumerator OnRMClickRelease(InputValue value) {
        m_RMClickPress = false;
        m_RMClickRelease = !value.isPressed;
        yield return new WaitForEndOfFrame();
        m_RMClickRelease = false;
    }

    void OnLeftShift(InputValue value) {
        m_LeftShift = value.isPressed;
    }

    void OnMouseDelta(InputValue value) {
        m_MouseDelta = value.Get<Vector2>();
    }

    void OnMousePosition(InputValue value) {
        m_MousePos = value.Get<Vector2>();
    }

    void OnMouseScroll(InputValue value) {
        m_MouseScroll = value.Get<float>();
        if (m_MouseScroll != 0)
            m_MouseScroll /= Mathf.Abs(m_MouseScroll);
    }

    // A ----------------------------------------------------------------------------------------------------------------------------------- A

}
