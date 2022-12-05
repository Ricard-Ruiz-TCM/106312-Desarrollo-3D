using UnityEngine;

public class HUDManager : MonoBehaviour {

    [SerializeField, Header("Panels")]
    private GameObject m_CoinsPanel;
    [SerializeField]
    private GameObject m_LifePanel;
    [SerializeField]
    private GameObject m_HealthPanel;

    [SerializeField, Header("Animation")]
    private AnimationClip m_HideClip;
    [SerializeField]
    private AnimationClip m_ShowClip;

    // Animation
    private Animation m_Animation;

    // Control si tenemos o no la interfaz en pantalla
    private bool m_ShowingHUD;

    [SerializeField, Header("Time Control:")]
    private float m_TimeDelay;
    private float m_NoChangesTime;

    // Unity Awake
    void Awake() {
        m_Animation = GetComponent<Animation>();
    }

    // Unity Start
    void Start() {
        HideHUD();
    }

    // Unity Update
    void Update() {
        // Tiempo para la animación funcionar
        m_NoChangesTime += Time.deltaTime;
        if ((m_NoChangesTime >= m_TimeDelay) && (m_ShowingHUD)) {
            HideHUD();
        }
    }

    // Método para actualizar la vida del jugador
    // In: int lifes -> Valor de lifes
    // In: float health -> Valor de health
    public void UpdateHealth(int lifes, float health) {
        // Solo si no estamos mostrando el HUD
        if (!m_ShowingHUD) {
            ShowHUD();
        }

        // Si tenemos el HUD en pantalla, updateamos valores
        // Si lo hacemos, reiniciamos el tiempo de "sin cambios"
        if (m_ShowingHUD) {
            m_HealthPanel.GetComponent<BasicImageFiller>().UpdateBar(health);
            m_LifePanel.GetComponent<BasicText>().UpdateText(lifes);
            m_NoChangesTime = 0.0f;
        }
    }

    // Método para coroutine
    private void ShowHUD() {
        m_ShowingHUD = true;
        m_Animation.Play(m_ShowClip.name);
    }

    // Método para coroutine
    private void HideHUD() {
        m_ShowingHUD = false;
        m_Animation.Play(m_HideClip.name);
    }

    // Método que actualiza el valor de las monedas
    // In: int coins -> Valor de las monedas
    public void UpdateCoins(int coins) {
        m_CoinsPanel.GetComponent<BasicText>().UpdateText(coins);
    }

}
