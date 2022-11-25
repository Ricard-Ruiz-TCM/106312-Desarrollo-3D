
public class Main : BasicScene {

    // Unity Awake
    void Awake() {
        // Set del nombre de la escena
        SetSceneName(GameScenes.Main);
        // Añadimos escena al director
        uCore.Director.AddScene(this);
    }

    // Unity Start
    void Start() {
        uCore.FadeFX.FadeOut();
        uCore.GameManager.ToggleCursor();
        uCore.Audio.PlayMusic("backgroundMusic", 0.2f);
        // Posición del player
        uCore.GameManager.GetPlayer().MovementSys().DisableMovement();
        uCore.GameManager.GetPlayer().transform.position = StartPoint();
        uCore.GameManager.GetPlayer().MovementSys().EnableMovement();
    }

    // Unity Update
    void Update() {
        if (uCore.Input.KeyO()) {
            uCore.GameManager.ToggleCursor();
            uCore.GameManager.GetPlayer().MovementSys().ToggleMovement();
            uCore.GameManager.GetPlayer().ShootingSys().ToggleShooting();
        }
    }

    // * --------------------------------------------------------------------------------- *
    // | - SCENE METHODS ----------------------------------------------------------------- |
    // V --------------------------------------------------------------------------------- V

    //   ···

    // A --------------------------------------------------------------------------------- A
}
