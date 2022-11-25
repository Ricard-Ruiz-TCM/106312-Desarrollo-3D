using UnityEngine;
using UnityEngine.InputSystem;

public class uCore : MonoBehaviour {

    // ----------------------------------------- //
    // - Singleton Instance -------------------- //
    private static InputManager m_InputManager = null;
    // - Get - (Depende de GameManager) -------- //
    public static InputManager Input {
        get {
            if (m_InputManager != null)
                return m_InputManager;

            m_InputManager = GameObject.FindObjectOfType<InputManager>();
            if (m_InputManager != null)
                return m_InputManager;

            // MUST NEED del PlayerInput para funcionar de forma correcta
            m_InputManager = new GameObject("InputManager").AddComponent<InputManager>();
            PlayerInput l_playerInput = m_InputManager.GetComponent<PlayerInput>();
            l_playerInput.actions = Resources.Load<InputActionAsset>("Settings/InputActions");
            l_playerInput.currentActionMap = l_playerInput.actions.actionMaps[0];
            // \ MUST NEED \

            m_InputManager.transform.SetParent(uCore.GameManager.transform);

            return m_InputManager;
        }
    }
    // ----------------------------------------- //

    // ----------------------------------------- //
    // - Singleton Instance -------------------- //
    private static AudioManager m_AudioManager = null;
    // - Get - (Depende de GameManager) -------- //
    public static AudioManager Audio {
        get {
            if (m_AudioManager != null)
                return m_AudioManager;

            m_AudioManager = GameObject.FindObjectOfType<AudioManager>();
            if (m_AudioManager != null)
                return m_AudioManager;

            m_AudioManager = new GameObject("AudioManager").AddComponent<AudioManager>();

            m_AudioManager.transform.SetParent(uCore.GameManager.transform);

            return m_AudioManager;
        }
    }
    // ----------------------------------------- //

    // ----------------------------------------- //
    // - Singleton Instance -------------------- //
    private static SceneDirector m_Scenedirector = null;
    // - Get - (Depende de GameManager) -------- //
    public static SceneDirector Director {
        get {
            if (m_Scenedirector != null)
                return m_Scenedirector;

            m_Scenedirector = GameObject.FindObjectOfType<SceneDirector>();
            if (m_Scenedirector != null)
                return m_Scenedirector;

            m_Scenedirector = new GameObject("SceneDirector").AddComponent<SceneDirector>();

            m_Scenedirector.transform.SetParent(uCore.GameManager.transform);

            return m_Scenedirector;
        }
    }
    // ----------------------------------------- //

    // ----------------------------------------- //
    // - Singleton Instance -------------------- //
    private static ParticleInstancer m_ParticleInstancer = null;
    // - Get - (Depende de GameManager) -------- //
    public static ParticleInstancer Particles {
        get {
            if (m_ParticleInstancer != null)
                return m_ParticleInstancer;

            m_ParticleInstancer = GameObject.FindObjectOfType<ParticleInstancer>();
            if (m_ParticleInstancer != null)
                return m_ParticleInstancer;

            m_ParticleInstancer = new GameObject("ParticleInstancer").AddComponent<ParticleInstancer>();

            m_InputManager.transform.SetParent(uCore.GameManager.transform);

            return m_ParticleInstancer;
        }
    }
    // ----------------------------------------- //

    // ----------------------------------------- //
    // - Singleton Instance -------------------- //
    private static Effect_ScreenFade m_Fader = null;
    // - Get ----------------------------------- //
    public static Effect_ScreenFade FadeFX {
        get {
            if (m_Fader != null)
                return m_Fader;

            m_Fader = GameObject.FindObjectOfType<Effect_ScreenFade>();

            return m_Fader;
        }
    }
    // ----------------------------------------- //

    // ----------------------------------------- //
    // - Singleton Instance -------------------- //
    private static GameManager m_GameManager = null;
    // - Get ----------------------------------- //
    public static GameManager GameManager {
        get {
            if (m_GameManager != null)
                return m_GameManager;

            m_GameManager = new GameObject("uCore(Game Manager)").AddComponent<uCore>().gameObject.AddComponent<GameManager>();

            return m_GameManager;
        }
    }
    // ----------------------------------------- //

    // ----------------------------------------- //
    // - Singleton Instance -------------------- //
    private static CanvasManager m_Canvas = null;
    // - Get ----------------------------------- //
    public static CanvasManager Canvas {
        get {
            if (m_Canvas != null)
                return m_Canvas;

            m_Canvas = GameObject.FindObjectOfType<CanvasManager>();

            return m_Canvas;
        }
    }
    // ----------------------------------------- //

    // Destruye posibles GameObjects de tipo "GameManager" en la escena si ya existe uno en "DontDestroyOnLoad"
    private void InstanceDestroyer() {
        GameManager[] instances = GameObject.FindObjectsOfType<GameManager>();
        int count = instances.Length;

        if (count >= 1) {
            for (var i = 1; i < instances.Length; i++)
                GameObject.Destroy(instances[i].gameObject);
            m_GameManager = instances[0];
        }
    }

    // Unity Awake
    void Awake() {
        InstanceDestroyer();
        DontDestroyOnLoad(this.gameObject);
    }

}
