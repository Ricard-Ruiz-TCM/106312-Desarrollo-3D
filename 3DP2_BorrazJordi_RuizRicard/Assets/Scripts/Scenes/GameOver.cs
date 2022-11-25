using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : BasicScene {

    // Unity Awake
    void Awake() {
        // Set del nombre de la escena
        SetSceneName(GameScenes.GameOver);
        // A�adimos escena al director
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

    // M�todo para bot�n
    public void AppExit() {
        Application.Quit(0);
    }

    // M�todo para bot�n
    public void RePlay() {
        uCore.Director.LoadScene(GameScenes.Main);
    }

    // A --------------------------------------------------------------------------------- A
}
