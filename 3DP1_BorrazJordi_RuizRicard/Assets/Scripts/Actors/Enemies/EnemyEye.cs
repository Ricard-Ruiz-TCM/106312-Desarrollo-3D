using UnityEngine;

public class EnemyEye : MonoBehaviour {

    [SerializeField]
    private Light m_Light;

    // Unity Awake
    void Awake() {
        m_Light = GetComponent<Light>();
    }

    // Cambia el color del ojo
    // In: Color c -> Nuevo color
    public void ChangeEyeColor(Color c) {
        m_Light.color = c;
    }
}
