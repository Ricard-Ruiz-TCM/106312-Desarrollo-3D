using UnityEngine;

public class GameMenu : BasicScene {

    // Unity Awake
    void Awake() {
        // Set del nombre de la escena
        SetSceneName(GameScenes.GameMenu);
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
    public void btn_Play() {
        uCore.Director.LoadScene(GameScenes.Game);
    }

    // Botones del canvas
    public void btn_Exit() {
        Application.Quit(0);
    }

    // A --------------------------------------------------------------------------------- A
}
