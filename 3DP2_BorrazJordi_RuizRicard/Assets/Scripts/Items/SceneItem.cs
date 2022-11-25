using UnityEngine;

public class SceneItem : Item {

    [SerializeField, Header("Scene Name:")]
    private string m_Scene;

    // Unity Awake
    void Awake() {
        this.name = "NextScene: -> [" + m_Scene + "]";
    }

    // Override del Pick
    public override void Pick(Player player) {
        uCore.FadeFX.FadeIn(NextSceneLock);
        uCore.GameManager.GetPlayer().JumpSys().DisableJump();
        uCore.GameManager.GetPlayer().ShootingSys().DisableShoot();
        uCore.GameManager.GetPlayer().MovementSys().DisableMovement();
    }

    private void NextSceneLock() {
        uCore.Director.LoadScene(m_Scene);
    }
}
