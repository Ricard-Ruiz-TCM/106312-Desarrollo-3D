using UnityEngine;
using UnityEngine.UI;

public class BasicImageFiller : MonoBehaviour {

    [SerializeField, Range(0.0f, 1.0f)]
    private float m_value;

    [SerializeField]
    private Image m_Image;

    // Método para actualizar valores de las barras
    // In: value -> Nuevo valor
    // In: maxValue -> Valor máximo posible de value
    public void UpdateBar(float value, float maxValue = 1.0f) {
        m_value = value / maxValue;
        m_Image.fillAmount = m_value;
    }

}
