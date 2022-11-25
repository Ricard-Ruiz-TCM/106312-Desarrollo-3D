using UnityEngine;

public class PortalButton : MonoBehaviour {

    private bool m_CanPress = true;

    [SerializeField]
    private bool m_PlayerCanPress;
    public bool PlayerCanPress() { return m_PlayerCanPress; }
    [SerializeField]
    private bool m_LaserCanPress;
    public bool CanLaserPress() { return m_LaserCanPress; }

    // GameObject que contiene las acciones
    [SerializeField]
    private GameObject m_ActionObject;
    // Acciones definidas en la interfaz
    private IButtonAction m_Action;
    // Método para simular un "Pressed"
    public void OnPressed() {
        if (!m_CanPress)
            return;

        m_CanPress = false;
        uCore.Audio.Play2DSFX("button", this.transform.position);
        m_Action.OnPressed();
    }

    public void OnReleased() {
        m_CanPress = true;
        m_Action.OnReleased();
    }

    // Unity Awake
    void Awake() {
        if (m_ActionObject != null)
            m_Action = m_ActionObject.GetComponent<IButtonAction>();
    }

    // Método para setear las acciones
    // In: IButtonAction actions -> Métodos Pressed y Released
    public void SetActions(IButtonAction actions) {
        m_Action = actions;
    }

    // * --------------------------------------------------------------------------------- *
    // | - COLLISION --------------------------------------------------------------------- |
    // V --------------------------------------------------------------------------------- V

    // OnCollisionEnter
    void OnCollisionEnter(Collision collision) {
        if (m_Action == null)
            return;

        OnPressed();
    }

    // OnCollisionExit
    void OnCollisionExit(Collision collision) {
        if (m_Action == null)
            return;

        OnReleased();
    }

    // A --------------------------------------------------------------------------------- A
}
