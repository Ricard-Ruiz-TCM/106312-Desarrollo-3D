using UnityEngine;
using UnityEngine.UI;

public class BasicText : MonoBehaviour {

    [SerializeField]
    private Text m_Text;

    // Métodos para actualizar el valor de m_Text.text dependiendo del tipo de parametro

    // Int
    public void UpdateText(int str) {
        m_Text.text = str.ToString();
    }

    // Float
    public void UpdateText(float str) {
        m_Text.text = str.ToString();
    }

    // Double
    public void UpdateText(double str) {
        m_Text.text = str.ToString();
    }

    // Bool
    public void UpdateText(bool str) {
        m_Text.text = str.ToString();
    }

    // String
    public void UpdateText(string str) {
        m_Text.text = str.ToString();
    }

    // Short
    public void UpdateText(short str) {
        m_Text.text = str.ToString();
    }

    // Método para limpiar el texto
    public void Clear() {
        m_Text.text = "";
    }

}
