using UnityEngine;

public class Victory : BasicScene {

    // Unity Awake
    void Awake() {
        // Set del nombre de la escena
        SetSceneName(GameScenes.Victory);
        // Añadimos escena al director
        uCore.Director.AddScene(this);
    }

    // Unity Start
    void Start() {
        uCore.FadeFX.FadeOut();
        uCore.GameManager.UnlockCursor();
    }

    // * --------------------------------------------------------------------------------- *
    // | - SCENE METHODS ----------------------------------------------------------------- |
    // V --------------------------------------------------------------------------------- V

    // Botones del canvas
    public void btn_Restart() {
        uCore.Director.LoadScene(GameScenes.Game);
    }

    // Botones del canvas
    public void btn_Exit() {
        Application.Quit(0);
    }

    // A --------------------------------------------------------------------------------- A
}
