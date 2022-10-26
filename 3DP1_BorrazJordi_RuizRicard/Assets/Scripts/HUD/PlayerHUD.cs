using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    [Header("Vitals:")]
    public BasicProgressBar m_ShieldBar;
    public TextMeshProUGUI m_ShieldText;
    public BasicProgressBar m_HealthBar;
    public TextMeshProUGUI m_HealthText;

    [Header("Shooting:")]
    public TextMeshProUGUI m_CurrentAmmunition;
    public TextMeshProUGUI m_TotalAmmunition;
    public Image m_WeaponIcon;

    [Header("Key Ring:")]
    public GameObject m_ItemIconPrefab;
    public RectTransform m_PanelSize;

    [Header("Respawn Button:")]
    public GameObject m_RespawnButton;

    [Header("Score:")]
    public GameObject m_ScoreOBJ;
    public TextMeshProUGUI m_TotalScore;
    public GameObject m_ScoreItem;
    public GameObject m_ScoreContainer;

    // Unity Start
    void Start() {
        m_RespawnButton.SetActive(false);
        DisableScore();
    }

    public void EnableScore() {
        m_ScoreOBJ.SetActive(true);
        m_TotalScore.text = "";
    }

    public void DisableScore() {
        m_TotalScore.text = "";
        m_ScoreOBJ.SetActive(false);
    }

    public void EnableRespawn() {
        m_RespawnButton.SetActive(true);
    }

    public void UpdateHealthBar(float health) {
        m_HealthBar.UpdateBar(health);
        m_HealthText.text = ((int)(health * 100.0f)).ToString() + " | 100";
    }

    public void UpdateShieldBar(float shield) {
        m_ShieldBar.UpdateBar(shield);
        m_ShieldText.text = ((int)(shield * 100.0f)).ToString() + " | 100";
    }

    public void UpdateAmmunitionText(int magazine, int total) {
        m_CurrentAmmunition.text = magazine.ToString();
        m_TotalAmmunition.text = total.ToString();
    }

    public void ChangeWeaponIcon(Sprite icon) {
        m_WeaponIcon.sprite = icon;
    }

    public void UpdateKeys(int[] keys) {
        foreach (Transform c in m_PanelSize) {
            Destroy(c.gameObject);
        }
        foreach (int id in keys) {
            GameObject k = GameObject.Instantiate(m_ItemIconPrefab, m_PanelSize.gameObject.transform);
            k.transform.GetComponentInChildren<TextMeshProUGUI>().text = id.ToString();
        }
        float value = 70 + (keys.Length * 60);
        m_PanelSize.offsetMax = new Vector2(value, m_PanelSize.offsetMax.y);
    }

    // In: int amount -> Cantidad de puntos
    public void AddPoints(int amount, int total) {
        m_TotalScore.text = total.ToString();
        Instantiate(m_ScoreItem, m_ScoreContainer.transform).GetComponent<TextMeshProUGUI>().text = "+" + amount.ToString();

    }


}
