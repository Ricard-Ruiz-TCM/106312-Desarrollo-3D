using UnityEngine;

public class AudioUtil : MonoBehaviour {

    [SerializeField, Header("Audio")]
    private AudioSource m_Source;

    // Unity Awake
    void Awake() {
        m_Source = GetComponent<AudioSource>();
    }

}
