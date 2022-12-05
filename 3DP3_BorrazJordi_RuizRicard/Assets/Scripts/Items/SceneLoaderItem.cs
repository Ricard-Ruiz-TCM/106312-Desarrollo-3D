using UnityEngine;

public class SceneLoaderItem : Item {

    [SerializeField, Header("Scene Name:")]
    private string m_Scene;

    [SerializeField]
    private AnimationClip m_ShowFlagClip;
    private Animation m_Animation;

    // Unity Awake
    void Awake() {
        this.name = "NextScene: -> [" + m_Scene + "]";
        m_Animation = GetComponent<Animation>();

        // Restart GameElement
        uCore.GameManager.AddRestartGameElement(this);
    }

    // Override del Pick
    public override void Pick(Player player) {
        m_Animation.CrossFade(m_ShowFlagClip.name);
        uCore.FadeFX.FadeIn(NextSceneLock);
    }

    private void NextSceneLock() {
        uCore.Director.LoadScene(m_Scene);
        Deactivate();
    }

}