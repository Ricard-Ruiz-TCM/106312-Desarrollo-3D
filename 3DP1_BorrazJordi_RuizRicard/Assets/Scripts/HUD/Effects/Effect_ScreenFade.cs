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
        m_Color.a += m_FadeStr * Time.unscaledDeltaTime;
        // Set del fade;
        m_Fade.color = m_Color;

        // Desactivamos el fadeo si hemos lelgado al límite
        if ((m_Color.a >= m_Max) || (m_Color.a <= 0.0f)) {
            m_Fading = false;
            if (m_CallBackAction) {
                m_CallBackAction = false;
                m_CallBack();
            }
        }
    }

    public void SetMaxFade(float max) {
        m_Max = max;
    }

    public void FadeIn(Action callback = null) {
        m_Max = 1.0f;
        m_Color.a = 0.0f;
        if (m_Fade == null)
            m_Fading = GetComponent<Image>();
        m_Fade.color = m_Color;
        m_Fading = true;
        m_FadeStr = 0.5f;
        CallBAckIt(callback);
    }

    public void FadeOut(Action callback = null) {
        m_Max = 1.0f;
        m_Color.a = 1.0f;
        if (m_Fade == null)
            m_Fading = GetComponent<Image>();
        m_Fade.color = m_Color;
        m_Fading = true;
        m_FadeStr = -0.5f;
        CallBAckIt(callback);
    }

    private void CallBAckIt(Action callback) {
        if (callback == null)
            return;

        m_CallBack = callback;
        m_CallBackAction = true;
    }

}
