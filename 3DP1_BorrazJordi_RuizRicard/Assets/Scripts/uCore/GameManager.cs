using UnityEngine;

public class GameManager : MonoBehaviour {

    #region Player

    [Header("Player:")]
    [SerializeField]
    private PlayerTemporalData mPlayerTemporalData;
    private PlayerData m_PlayerData;
    private Player m_Player;

    // Set del ScriptableObject que contiene el objeto Player
    // In: Player player -> Player en la escena
    public void SetPlayer(Player player) {
        m_Player = player;
        m_PlayerData = player.Data();
    }

    // Get del player data
    // Out: Player Data :D 
    public PlayerData GetPlayerData() {
        if (m_Player == null)
            m_PlayerData = Resources.Load<PlayerData>("ScriptableObjects/Player");

        return m_PlayerData;
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

    [Header("Actor Container Ref#.:")]
    [SerializeField]
    private Transform m_Actorcontainer;
    [Header("Audio Container Ref#.:")]
    [SerializeField]
    private Transform m_AudioContainer;
    [Header("Particle Container Ref#.:")]
    [SerializeField]
    private Transform m_ParticleContainer;

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

}
