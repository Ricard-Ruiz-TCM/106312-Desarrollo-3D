using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CustomDebug : MonoBehaviour {

    [SerializeField]
    private float m_TimeSpeed = 1.0f;

    public void OnIncreaseSpeed(InputValue value) {
        m_TimeSpeed += 0.1f;
        SetSpeed();
    }

    public void OnDecreaseSpeeed(InputValue value) {
        m_TimeSpeed -= 0.1f;
        m_TimeSpeed = Mathf.Max(m_TimeSpeed, m_TimeSpeed, 0.0f);
        SetSpeed();
    }

    public void OnResetSpeed(InputValue value) {
        m_TimeSpeed = 1.0f;
        SetSpeed();
    }

    public void SetSpeed() {
        Time.timeScale = m_TimeSpeed;
    }

}