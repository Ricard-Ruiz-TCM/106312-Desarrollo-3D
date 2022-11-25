using System.Collections.Generic;
using UnityEngine;

public class ParticleInstancer : MonoBehaviour {

    // Actor Container
    private Transform m_ActorContainer;

    [SerializeField]
    private Dictionary<string, GameObject> m_Particles;

    // Unity Awake
    void Awake() {
        // Contenedor base de audios
        m_ActorContainer = uCore.GameManager.ActorContainer();
        // Diccionario
        m_Particles = new Dictionary<string, GameObject>();
    }

    // Check si esta el prefab cargado
    // In: string name -> Nombre ID del prefab
    // Out: bool -> (true -> prefab está cargado | false -> prefab no está cargado)
    private bool Exists(string name) {
        return m_Particles.ContainsKey(name);
    }

    // Carga un prefab al sistema
    // In: string name -> Nombre ID del prefab
    private void LoadParticles(string name) {
        m_Particles.Add(name, Resources.Load<GameObject>("Prefabs/Particles/" + name));
    }

    // Método para cargar particulas solo 1 vez
    // In: string name -> Nombre ID del prefab
    // In: Transform parent -> Parent de la particulas
    // Out: GameObject -> objeto recien creado
    public GameObject PlayParticlesOnce(string name, Transform parent) {
        // Cargamos prefab
        if (!Exists(name))
            LoadParticles(name);
        // Instanciamos el particles
        ParticleSystem g = InstantPart(m_Particles[name], parent).GetComponent<ParticleSystem>();
        // Destruimos el particles
        return g.gameObject;
    }

    // Método para reproducir un SFX solo 1 vec
    // In: string name -> Nombre ID del clip
    // In: Vector3 postiion -> Posición del audio en el mundo, 3D NENE, UBICATE
    public void PlayParticlesOnce(string name, Vector3 position) {
        // Instanciamos el audio
        GameObject g = PlayParticlesOnce(name, m_ActorContainer);
        // Set de la posición
        g.transform.position = position;
    }

    public ParticleSystem PlayLoopedParticles(string name, Transform parent) {
        return PlayParticlesOnce(name, transform).GetComponent<ParticleSystem>();
    }

    // Instncia el prefab SFX
    // In: GameObject obj -> Objeto
    // In: Transform parent -> Parent de las particulas
    // Out: GameObject -> objeto recien creado
    private GameObject InstantPart(GameObject obj, Transform parent) {
        return GameObject.Instantiate(obj, parent);
    }
}