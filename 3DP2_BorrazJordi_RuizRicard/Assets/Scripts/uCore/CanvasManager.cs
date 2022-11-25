using UnityEngine;

public class CanvasManager : MonoBehaviour {

    [SerializeField]
    private GameObject m_KeyPanel;
    [SerializeField]
    private GameObject m_RespawnPanel;

    // Unity Awake
    void Awake() {
        m_KeyPanel.SetActive(false);
        m_RespawnPanel.SetActive(false);
    }

    // Método para mostar el RespawnButton
    public void ShowKey() {
        m_KeyPanel.SetActive(true);
    }

    // Método para ocultar el RespawnButton
    public void HideKey() {
        m_KeyPanel.SetActive(false);
    }

    // Método para mostar el RespawnButton
    public void ShowRespawn() {
        uCore.GameManager.UnlockCursor();
        m_RespawnPanel.SetActive(true);
    }

    // Método para ocultar el RespawnButton
    public void HideRespawn() {
        uCore.GameManager.LockCursor();
        m_RespawnPanel.SetActive(false);
    }
}
