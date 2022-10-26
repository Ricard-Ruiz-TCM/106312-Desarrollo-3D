using UnityEngine;

public class GameOver : BasicScene {

    // Unity Awake
    void Awake() {
        // Set del nombre de la escnea
        SetSceneName(GameScenes.GSCENE_GameOver);
        // A�adimos est� escena al director ;3 
        uCore.Director.AddScene(this);
        uCore.Director.GameLoaded(GameScenes.GSCENE_GameOver);
    }

    // Unity Start
    void Start() {
        uCore.FadeFX.FadeOut();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Override UnLoad
    public override void UnLoad() {
    }

    // * --------------------------------------------------------------------------------- *
    // | - CANVASF FLOW ------------------------------------------------------------------ |
    // V --------------------------------------------------------------------------------- V

    // M�todo para el boton de <Jugar>
    public void Btn_RePlay() {
        uCore.Director.LoadSceneFaded(GameScenes.GSCENE_Outside);
    }

    // M�todo para el boton de <Exit>
    public void Btn_Exit() {
        Application.Quit(0);
    }

    // A --------------------------------------------------------------------------------- A
}
