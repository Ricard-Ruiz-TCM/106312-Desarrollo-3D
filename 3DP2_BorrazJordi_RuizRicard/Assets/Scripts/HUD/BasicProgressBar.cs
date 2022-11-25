using UnityEngine;
using UnityEngine.UI;

public class BasicProgressBar : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    public float m_value;

    public Image m_bar;

    // Método para actualizar valores de las barras
    // In: value -> Nuevo valor
    // In: maxValue -> Valor máximo posible de value
    public void UpdateBar(float value, float maxValue = 1.0f) {
        m_value = value / maxValue;
        m_bar.fillAmount = m_value;
    }

}
