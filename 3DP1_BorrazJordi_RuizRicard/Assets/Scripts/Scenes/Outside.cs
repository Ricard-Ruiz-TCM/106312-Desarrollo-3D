using UnityEngine;

public class Outside : BasicScene {

    // Unity OnEnable
    void OnEnable() {
        SGallery.OnGalleryCompleted += OpenEntrance;
    }

    // Unity OnDisable
    void OnDisable() {
        SGallery.OnGalleryCompleted -= OpenEntrance;
    }

    // Unity Awake
    void Awake() {
        // Set del nombre de la escnea
        SetSceneName(GameScenes.GSCENE_Outside);
        // Añadimos está escena al director ;3 
        uCore.Director.AddScene(this);
        uCore.Director.GameLoaded(GameScenes.GSCENE_Outside);
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

    [SerializeField, Header("Dungeon Entrance:")]
    private Animation m_Entrance;

    [SerializeField, Header("Score para next lvl:")]
    private int m_Score;

    // Método para cambiar de nivel si se lelga al escore
    // In: int score -> Puntuación optenida en el SGallery
    private void OpenEntrance(int score) {
        if (score < m_Score)
            return;

        m_Entrance.Play();
    }

    // A --------------------------------------------------------------------------------- A
}
