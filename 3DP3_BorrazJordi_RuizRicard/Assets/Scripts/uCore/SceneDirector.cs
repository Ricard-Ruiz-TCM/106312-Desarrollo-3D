using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct GameScenes {
    public static string Game = "Game";
    public static string GameMenu = "GameMenu";
    public static string GameOver = "GameOver";
    public static string Victory = "Victory";
};

public class SceneDirector : MonoBehaviour {

    [Header("Scenes:")]
    [SerializeField]
    private Dictionary<string, BasicScene> m_Scenes;
    [SerializeField]
    private string m_CurrentScene = "";
    [SerializeField]
    private string m_LastScene = "";

    // NextScene After Effects
    private string m_InmediateScene;

    [SerializeField]
    private bool m_WorkingOnChange = false;

    // Get de la escena actual
    // Out: BasicScene -> Escena actual dle diccionario en posición m_CurrentScene
    public BasicScene Scene() {
        return m_Scenes[m_CurrentScene];
    }

    // Método para añadair als escenas al director
    // In: BasicScene scene -> Escena básica controlada
    public void AddScene(BasicScene scene) {
        if (m_Scenes == null)
            m_Scenes = new Dictionary<string, BasicScene>();

        // Check no tenemos la escena
        if (m_Scenes.ContainsKey(scene.Name()))
            // Substitumos la escena
            m_Scenes[scene.Name()] = scene;
        else
            // añadimos de la escena 
            m_Scenes.Add(scene.Name(), scene);
    }

    // Método que hace el call la priemra escena, apra indicar que el juego a cargado en esa escena
    // In: string scenenName -> Nombre de la primera escena que carga el juego
    public void GameLoaded(string sceneName) {
        SetScene(sceneName);
    }

    // Método interno para gaurdar la escena actual y la última escena ;3 
    // In: string sceneName -> Nombre de la escena
    private void SetScene(string sceneName) {
        m_LastScene = m_CurrentScene;
        m_CurrentScene = sceneName;
    }

    // Método para cargar una escena nueva de forma inmediatea
    // In: string sceneName -> nueva escena
    public void LoadScene(string sceneName) {
        SetScene(sceneName);
        SceneManager.LoadScene(sceneName);
        m_WorkingOnChange = false;
    }

    // Cargamos escena con FadeIn
    // In: string sceneName -> nombre de la escena
    public void LoadSceneFaded(string sceneName) {
        if (m_WorkingOnChange)
            return;

        m_InmediateScene = sceneName;
        m_WorkingOnChange = true;
        uCore.FadeFX.FadeIn(ILoadFaded);
    }

    // Internal para cargar la escena del faded
    private void ILoadFaded() {
        LoadScene(m_InmediateScene);
    }

}
