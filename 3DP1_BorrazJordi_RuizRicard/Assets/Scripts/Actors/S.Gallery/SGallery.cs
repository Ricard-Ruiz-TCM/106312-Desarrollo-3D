using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGallery : MonoBehaviour {

    public static event Action<int> OnGalleryCompleted;

    // Control
    [SerializeField]
    private bool m_Active;
    public bool Active() { return m_Active; }
    [SerializeField]
    private bool m_Completed;
    [SerializeField]
    private GameObject m_Input;

    // Animation
    [SerializeField, Header("Animations:")]
    private AnimationClip m_CloseStageClip;
    [SerializeField]
    private AnimationClip m_OpenStageClip;

    [SerializeField, Header("Galería de tiro:")]
    private int m_Score;
    public int Score() { return m_Score; }

    [SerializeField, Header("Stage:")]
    private int m_CurrentStage;
    [SerializeField]
    private float m_StageDuration;
    [SerializeField]
    private float m_StageItemAppearRate;
    [SerializeField]
    private GameObject m_StageContainer;
    [SerializeField]
    private List<GalleryStage> m_Stages;

    // Unity Awake
    void Awake() {
        m_Stages = new List<GalleryStage>();
        // Get de los items y los stages
        foreach (Transform c in m_StageContainer.transform) {
            m_Stages.Add(new GalleryStage(this, m_Stages.Count, c.gameObject, m_StageDuration));
            m_Stages[m_Stages.Count - 1].FillItems();
        }
        // Desactivamos el reminder
        m_Input.SetActive(false);
    }

    // Unity Start
    void Start() {
        m_Active = false;
        m_Score = 0;
    }

    // Método para itneractuar con la gelería de tiro
    public void Interact() {
        StartGallery();
    }

    private IEnumerator ShowItem(float time) {
        yield return new WaitForSeconds(time);
        m_Stages[m_CurrentStage].ShowRandomItem();
        StartCoroutine(ShowItem(m_StageItemAppearRate));
    }

    // Método para cambiar de Stage,
    // Cierra el Stage actual y abre el siguiente si peude
    // Si no peude, envia el invoke de que ya ha terminado la galeería de tiro
    // In: float time -> Duración del stage
    private IEnumerator NextStage(float time) {
        if (!m_Completed) {
            yield return new WaitForSeconds(time);
            // Cerramos stage
            m_Stages[m_CurrentStage].ActiveItems();
            m_Stages[m_CurrentStage].StageObj.GetComponent<Animation>().Play(m_CloseStageClip.name);
            int l_nextStage = Mathf.Clamp(m_CurrentStage + 1, 0, m_Stages.Count - 1);

            // Chedk si acabamos la gelería
            if (m_CurrentStage == l_nextStage) {
                m_Completed = true;
                m_Active = false;
                m_CurrentStage = 0;
                m_Input.SetActive(true);
                OnGalleryCompleted?.Invoke(m_Score);
                m_Score = 0;
            } else {
                // Abrimos Stage
                m_CurrentStage = l_nextStage;
                m_Stages[m_CurrentStage].StageObj.GetComponent<Animation>().Play(m_OpenStageClip.name);
                StartCoroutine(NextStage(m_StageDuration));
            }
        }
    }

    // Empieza las animaciones por stage y el trabajo
    private void StartGallery() {
        if (m_Active)
            return;

        // Actdivamos y abrimos la gelería
        m_Active = true;
        m_Completed = false;
        m_Input.SetActive(false);
        m_Stages[m_CurrentStage].StageObj.GetComponent<Animation>().Play(m_OpenStageClip.name);

        // Empezamos el juego mostrando un par de items y activando el "timer"
        m_Stages[m_CurrentStage].ShowRandomItem();
        m_Stages[m_CurrentStage].ShowRandomItem();
        StartCoroutine(NextStage(m_StageDuration));
        StartCoroutine(ShowItem(m_StageItemAppearRate));
    }

    // Añade puntuación a la gelería y se la manda al player
    // In: int amount -> total dep untos a añadir
    public void AddPoints(int amount) {
        m_Score += amount;
        uCore.GameManager.GetPlayer().SetPoints(amount, m_Score);
    }

    // * --------------------------------------------------------------------------------- *
    // | - COLLISION --------------------------------------------------------------------- |
    // V --------------------------------------------------------------------------------- V

    // Unity OnTriggerEnter
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (!m_Active)
                m_Input.SetActive(true);
        }
    }

    // Unity OnTriggerExit
    void OnTriggerExit(Collider other) {
        if (other.tag == "Player")
            if (m_Input.activeSelf) {
                m_Input.SetActive(false);
            }
    }

    // A --------------------------------------------------------------------------------- A
}
