
public class Game : BasicScene {

    // Unity Awake
    void Awake() {
        // Set del nombre de la escena
        SetSceneName(GameScenes.Game);
        // Añadimos escena al director
        uCore.Director.AddScene(this);
    }

    // Unity Start
    void Start() {
        uCore.FadeFX.FadeOut();
        uCore.GameManager.LockCursor();
    }

    // Unity Update
    void Update() {
        if (uCore.Action.KeyO()) {
            uCore.GameManager.ToggleCursor();
        }
    }

    // * --------------------------------------------------------------------------------- *
    // | - SCENE METHODS ----------------------------------------------------------------- |
    // V --------------------------------------------------------------------------------- V

    //   ···

    // A --------------------------------------------------------------------------------- A
}
