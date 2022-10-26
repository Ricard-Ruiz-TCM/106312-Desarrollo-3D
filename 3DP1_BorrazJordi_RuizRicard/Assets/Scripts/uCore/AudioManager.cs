using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [SerializeField, Header("Prefabs")]
    private GameObject m_SFX;
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
        m_SFX = Resources.Load<GameObject>("Prefabs/SFX");
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

    // Método para reproducir un SFX solo 1 vec
    // In: string name -> Nombre ID del clip
    // In: Transform parent -> Parent dle audio, ES UN AUDIO 3D, NENE UBICATE
    // Out: GameObject -> objeto recien creado
    public GameObject PlaySFX(string name, Transform parent) {
        // Cargamos clip
        if (!Exists(name))
            LoadClip(name);
        // Instanciamos el SFX
        AudioSource g = InstantSFX(parent).GetComponent<AudioSource>();
        // Reproducimos el Clip
        g.clip = m_Clips[name]; g.Play();
        // Destruimos el Audio
        GameObject.Destroy(g.gameObject, g.clip.length * 1.5f);
        return g.gameObject;
    }

    // Método para reproducir un SFX solo 1 vec
    // In: string name -> Nombre ID del clip
    // In: Vector3 postiion -> Posición del audio en el mundo, 3D NENE, UBICATE
    public void PlaySFX(string name, Vector3 position) {
        // Instanciamos el audio
        GameObject g = PlaySFX(name, m_AudioContainer);
        // Set de la posición
        g.transform.position = position;
    }

    // Instncia el prefab SFX
    // In: Transform parent -> Parent dle audio, ES UN AUDIO 3D, NENE UBICATE
    // Out: GameObject -> objeto recien creado
    private GameObject InstantSFX(Transform parent) {
        return GameObject.Instantiate(m_SFX, parent);
    }

}