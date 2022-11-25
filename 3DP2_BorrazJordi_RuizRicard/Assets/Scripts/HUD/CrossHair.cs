using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour {

    [SerializeField]
    private Image m_Image;

    // PlayerShoting
    private PlayerShooting m_Shoting;

    // Sprites
    [SerializeField, Header("CrossHair Sprites:")]
    private Sprite m_BlueSprite;
    [SerializeField]
    private Sprite m_OrangeSprite;
    [SerializeField]
    private Sprite m_BOSprite;
    [SerializeField]
    private Sprite m_NOPortal;
    [SerializeField]
    private Sprite m_CanPlaceSprite;

    // Sprite Temporal
    private Sprite m_CurrentSprite;

    // Variables de control
    private bool m_Blue, m_Orange;

    // OnEnable
    void OnEnable() {
        PlayerShooting.OnShootPortal += changeColor;
    }

    // OnDisable
    void OnDisable() {
        PlayerShooting.OnShootPortal -= changeColor;
    }

    // Unity Start
    void Start() {
        m_Blue = m_Orange = false;
        m_CurrentSprite = m_Image.sprite;
        m_Shoting = uCore.GameManager.GetPlayer().ShootingSys();
    }

    // Unity Update
    void Update() {
        updateSprite();
    }

    // Método para actulziar el sprite
    private void updateSprite() {
        m_Image.sprite = m_CurrentSprite;
        // Comprobamos si estamos ghosteando
        if (!m_Shoting.CanPlacePortal()) {
            if (m_Shoting.IsGhosting())
                m_Image.sprite = m_CanPlaceSprite;
        }
    }

    // Método para cambiar el color
    void changeColor(PORTAL portal) {
        // cpom`robamos que portales tenemos activos
        switch (portal) {
            case PORTAL.GHOSTING:
                if (m_Shoting.GhostingPortal().Equals(PORTAL.BLUE)) {
                    m_Blue = false;
                }
                if (m_Shoting.GhostingPortal().Equals(PORTAL.ORANGE)) {
                    m_Orange = false;
                }
                break;
            case PORTAL.BLUE:
                m_Blue = true;
                break;
            case PORTAL.ORANGE:
                m_Orange = true;
                break;
        }

        // Cambaimos el valor de m_CurrentSprite
        if (m_Blue) {
            if (m_Orange) {
                m_CurrentSprite = m_BOSprite;
            } else {
                m_CurrentSprite = m_BlueSprite;
            }
        } else if (m_Orange) {
            m_CurrentSprite = m_OrangeSprite;
        } else {
            m_CurrentSprite = m_NOPortal;
        }
        
        updateSprite();
    }

}

