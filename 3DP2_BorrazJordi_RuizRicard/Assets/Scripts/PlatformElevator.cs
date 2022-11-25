using UnityEngine;

public class PlatformElevator : MonoBehaviour, IButtonAction {

    // animation
    private Animation m_Animation;

    // Unity Awake
    void Awake() {
        m_Animation = GetComponent<Animation>();
    }

    // M�todos de IButtonAction
    public void OnPressed() {
        m_Animation.Play();
    }

    // M�todos de IButtonAction
    public void OnReleased() { }

}
