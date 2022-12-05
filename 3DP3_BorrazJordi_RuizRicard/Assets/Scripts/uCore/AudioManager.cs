using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [SerializeField, Header("Prefabs")]
    private GameObject m_3D_SFX;
    [SerializeField]
    private GameObject m_2D_SFX;
    [SerializeField]
    private GameObject m_Music;

    // Audio Container
    private Transform m_AudioContainer;

    [SerializeField]
    private Dictionary<string, AudioClip> m_Clips;

    // Unity Awake
    void Awake() {
        // Contenedor base de audios
        m_AudioContainer = uCore.GameManager.AudioContainer();
        // Prefabs
        m_3D_SFX = Resources.Load<GameObject>("Prefabs/2D_SFX");
        m_2D_SFX = Resources.Load<GameObject>("Prefabs/2D_SFX");
        m_Music = Resources.Load<GameObject>("Prefabs/Music");
        // Diccionario
        m_Clips = new Dictionary<string, AudioClip>();
    }

    // Check si esta el clip cargado
    // In: string name -> Nombre ID del clip
    // Out: bool -> (true -> Clip está cargado | false -> Clip no está cargado)
    private bool Exists(string name) {
        return m_Clips.ContainsKey(name);
    }

    // Carga un clip al sistema
    // In: string name -> Nombre ID del clip
    private void LoadClip(string name) {
        m_Clips.Add(name, Resources.Load<AudioClip>("Audio/SFX/" + name));
    }

    public void PlayMusic(string name, float volume) {
        // Cargamos clip
        if (!Exists(name))
            LoadClip(name);
        // Instanciamos el SFX
        AudioSource g = InstantMusic(uCore.GameManager.AudioContainer()).GetComponent<AudioSource>();
        // Play
        g.clip = m_Clips[name]; g.Play();
        g.volume = volume;
    }

    // Método para reproducir un SFX solo 1 vec
    // In: string name -> Nombre ID del clip
    // In: Transform parent -> Parent dle audio, ES UN AUDIO 3D, NENE UBICATE
    // Out: GameObject -> objeto recien creado
    public GameObject Play2DSFX(string name, Transform parent) {
        // Cargamos clip
        if (!Exists(name))
            LoadClip(name);
        // Instanciamos el SFX
        AudioSource g = Instant2DSFX(parent).GetComponent<AudioSource>();
        // Reproducimos el Clip
        g.clip = m_Clips[name]; g.Play();
        // Destruimos el Audio
        GameObject.Destroy(g.gameObject, g.clip.length * 1.5f);
        return g.gameObject;
    }

    // Método para reproducir un SFX solo 1 vec
    // In: string name -> Nombre ID del clip
    // In: Vector3 postiion -> Posición del audio en el mundo, 3D NENE, UBICATE
    public void Play2DSFX(string name, Vector3 position) {
        // Instanciamos el audio
        GameObject g = Play2DSFX(name, m_AudioContainer);
        // Set de la posición
        g.transform.position = position;
    }

    // Instncia el prefab SFX
    // In: Transform parent -> Parent dle audio, ES UN AUDIO 3D, NENE UBICATE
    // Out: GameObject -> objeto recien creado
    private GameObject Instant3DSFX(Transform parent) {
        return GameObject.Instantiate(m_3D_SFX, parent);
    }

    // Instncia el prefab SFX
    // In: Transform parent -> Parent dle audio, ES UN AUDIO 3D, NENE UBICATE
    // Out: GameObject -> objeto recien creado
    private GameObject Instant2DSFX(Transform parent) {
        return GameObject.Instantiate(m_2D_SFX, parent);
    }

    private GameObject InstantMusic(Transform parent) {
        return GameObject.Instantiate(m_Music, parent);
    }

}