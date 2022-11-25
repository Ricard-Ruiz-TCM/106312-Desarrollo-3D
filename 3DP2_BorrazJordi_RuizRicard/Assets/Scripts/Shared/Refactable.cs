using UnityEngine;

public class Refactable : MonoBehaviour {

    private Laser m_Laser;

    // Unity Awake
    void Awake() {
        m_Laser = GetComponentInChildren<Laser>();
    }

    // Unity Update
    void Update() {
        m_Laser.EnableLaser();
    }

}
