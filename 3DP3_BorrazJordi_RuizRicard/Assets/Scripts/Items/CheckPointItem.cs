using UnityEngine;

public class CheckPointItem : Item {

    [SerializeField]
    private Transform m_RespawnPoint;

    // Unity Awake
    void Awake() {
        // Restart GameElement
        uCore.GameManager.AddRestartGameElement(this);
    }

    // Override del Pick
    public override void Pick(Player player) {
        player.CheckPoint.SetCheckPoint(m_RespawnPoint.position);
        Deactivate();
    }

}
