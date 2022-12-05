using System;
using UnityEngine;
using UnityEngine.UI;

public class Effect_ScreenFade : MonoBehaviour {

    [SerializeField]
    private bool m_Fading;

    [SerializeField, Range(-0.5f, 0.5f)]
    private float m_FadeStr;

    private bool m_CallBackAction;
    // CallBack para el end del fade
    private Action m_CallBack;

    private float m_Max = 1.0f;

    // Imagen
    private Image m_Fade;

    // Color a modificar alpha
    private Color m_Color;

    void Awake() {
        m_Fade = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start() {
        m_Color = m_Fade.color;
    }

    // Unity Update
    void Update() {
        // Si no estámos trabajando, nos vamos
        if (!m_Fading)
            return;

        // Calculamos el alpha
        m_Color.a += m_FadeStr * Time.deltaTime;
        // Set del fade;
        m_Fade.color = m_Color;

        // Desactivamos el fadeo si hemos lelgado al límite
        if ((m_Color.a >= m_Max) || (m_Color.a <= 0.0f)) {
            m_Fading = false;
            if (m_CallBackAction) {
                m_CallBack();
                m_CallBack = null;
                m_CallBackAction = false;
            }
        }
    }

    // Método para esablecer el maximo de fade
    // In: float max -> máximo valor de fade
    public void SetMaxFade(float max) {
        m_Max = max;
    }

    // Método para configurar un FadeIn
    // In: Action callback = null -> Método que se ejecutara una vez se acabe el FadeIn
    public void FadeIn(Action callback = null) {
        m_Max = 1.0f;
        m_Color.a = 0.0f;
        if (m_Fade == null)
            m_Fading = GetComponent<Image>();
        m_Fade.color = m_Color;
        m_Fading = true;
        m_FadeStr = 0.5f;
        CallBackIt(callback);
    }

    // Método para configurar un FadeOut
    // In: Action callback = null -> Método que se ejecutara una vez se acabe el FadeOut
    public void FadeOut(Action callback = null) {
        m_Max = 1.0f;
        m_Color.a = 1.0f;
        if (m_Fade == null)
            m_Fading = GetComponent<Image>();
        m_Fade.color = m_Color;
        m_Fading = true;
        m_FadeStr = -0.5f;
        CallBackIt(callback);
    }

    // Método para configurar el CallBack
    // In: Action callback -> Método que se configurara
    private void CallBackIt(Action callback) {
        if (callback == null)
            return;

        m_CallBack = callback;
        m_CallBackAction = true;
    }

}
