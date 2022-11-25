using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : BasicScene {

    // Unity Awake
    void Awake() {
        // Set del nombre de la escena
        SetSceneName(GameScenes.GameOver);
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

    // Método para botón
    public void AppExit() {
        Application.Quit(0);
    }

    // Método para botón
    public void RePlay() {
        uCore.Director.LoadScene(GameScenes.Main);
    }

    // A --------------------------------------------------------------------------------- A
}
