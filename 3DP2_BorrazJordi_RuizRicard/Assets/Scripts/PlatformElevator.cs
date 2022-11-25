using UnityEngine;

public class PlatformElevator : MonoBehaviour, IButtonAction {

    // animation
    private Animation m_Animation;

    // Unity Awake
    void Awake() {
        m_Animation = GetComponent<Animation>();
    }

    // Métodos de IButtonAction
    public void OnPressed() {
        m_Animation.Play();
    }

    // Métodos de IButtonAction
    public void OnReleased() { }

}
