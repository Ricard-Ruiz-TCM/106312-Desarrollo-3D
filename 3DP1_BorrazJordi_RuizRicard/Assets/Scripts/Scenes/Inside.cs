using UnityEngine;

public class Inside : BasicScene {

    // Unity Awake
    void Awake() {
        // Set del nombre de la escnea
        SetSceneName(GameScenes.GSCENE_Inside);
        // Añadimos está escena al director ;3 
        uCore.Director.AddScene(this);
        uCore.Director.GameLoaded(GameScenes.GSCENE_Inside);
    }

    // Unity Start
    void Start() {
        uCore.FadeFX.FadeOut();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // UnLoad
    public override void UnLoad() { }

    // * --------------------------------------------------------------------------------- *
    // | - SCENE ------------------------------------------------------------------------- |
    // V --------------------------------------------------------------------------------- V

    [SerializeField, Header("Dungeon Exit:")]
    private Animation m_Exit;

    // Método para abrir la entrada al nivel 2
    public void OpenExitDoor() {
        m_Exit.Play();
    }

    // A --------------------------------------------------------------------------------- A

}
