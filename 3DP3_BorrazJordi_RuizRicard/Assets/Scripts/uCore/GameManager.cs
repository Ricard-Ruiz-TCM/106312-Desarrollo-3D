using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    private List<IRestartGameElement> m_RestartElements;

    #region RestartElements
    public void AddRestartGameElement(IRestartGameElement restartGameElement) {
        if (m_RestartElements == null)
            m_RestartElements = new List<IRestartGameElement>();

        m_RestartElements.Add(restartGameElement);
    }

    public void RestartAllGameElement() {
        foreach(IRestartGameElement i in m_RestartElements) {
            i.RestartGame();
        }
    }
    #endregion

    #region Player

    [Header("Player:")]
    private Player m_Player;

    // Set del ScriptableObject que contiene el objeto Player
    // In: Player player -> Player en la escena
    public void SetPlayer(Player player) {
        m_Player = player;
    }


    // Get del player
    // Out: Player D:
    public Player GetPlayer() {
        if (m_Player == null)
            m_Player = GameObject.FindObjectOfType<Player>();
        return m_Player;
    }

    #endregion

    #region Scene-Hierachy

    [SerializeField, Header("Actor Container Ref#.:")]
    private Transform m_Actorcontainer;
    [SerializeField, Header("Audio Container Ref#.:")]
    private Transform m_AudioContainer;
    [SerializeField, Header("Particle Container Ref#.:")]
    private Transform m_ParticleContainer;
    [SerializeField, Header("Entity Container Ref#.:")]
    private Transform m_EntityContainer;

    public Transform EntityContainer() {
        if (m_EntityContainer == null)
            m_EntityContainer = GameObject.FindObjectOfType<EntityContainer>().transform;

        return m_EntityContainer;
    }

    public Transform ActorContainer() {
        if (m_Actorcontainer == null)
            m_Actorcontainer = GameObject.FindObjectOfType<ActorContainer>().transform;

        return m_Actorcontainer;
    }

    public Transform AudioContainer() {
        if (m_AudioContainer == null)
            m_AudioContainer = GameObject.FindObjectOfType<AudioContainer>().transform;

        return m_AudioContainer;
    }


    public Transform ParticleContainer() {
        if (m_ParticleContainer == null)
            m_ParticleContainer = GameObject.FindObjectOfType<ParticleContainer>().transform;

        return m_Actorcontainer;
    }

    #endregion

    #region Some Layer Utility

    // Set de todas las layers del objeto y sus hijos a Default (0)
    public void SetDefaultLayer(GameObject obj) {
        R_NextChild(obj.transform, obj.transform.childCount);
    }

    // Método recursivo para setear todos los hijos de un objeto con el layr 0
    // In: Transform childs -> objeto padre
    // In: int amount -> Cantidad de hijos
    private void R_NextChild(Transform childs, int amount) {
        // Set del layer
        childs.gameObject.layer = 0;

        // Si no tien hijos, volvemos atras
        if (amount == 0)
            return;

        // Mientras tenga hijos, buscamos al último hijo y vamos retrocediendo buscnado ma´s y más para setearlos
        if (amount > 0) {
            amount--;
            R_NextChild(childs.GetChild(amount), childs.GetChild(amount).transform.childCount);
        }

    }

    #endregion

    #region Cursor

    // Cursor
    [SerializeField]
    private bool m_Locked = false;
    public bool IsCursorLocked() { return !m_Locked; }

    // Método par alternar el lockeo del ratón en el juego
    public void ToggleCursor() {
        m_Locked = !m_Locked;

        // Lock del cursor
        Cursor.visible = m_Locked;
        Cursor.lockState = (CursorLockMode)(m_Locked ? 1 : 0);
    }

    // Método para bloquear el cursor
    public void LockCursor() {
        m_Locked = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Método para desbloquear el cursor
    public void UnlockCursor() {
        m_Locked = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    #endregion

}
