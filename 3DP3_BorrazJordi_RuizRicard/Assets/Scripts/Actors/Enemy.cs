using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EnemyStates {
    IDDLE, PATROL, CHASE, ATTACK,
    TOTAL_STATES,

    INNITIAL_STATE = EnemyStates.IDDLE,
}

public abstract class Enemy : MonoBehaviour, IRestartGameElement {

    [SerializeField, Header("Control:")]
    protected bool m_Alive;

    [SerializeField, Header("Estado's:")]
    protected EnemyStates m_CurrentState;

    // States
    [SerializeField]
    protected List<BasicState> m_States;

    // States
    protected EnemyIddle m_Iddle;
    protected EnemyPatrol m_Patrol;
    protected EnemyChase m_Chase;
    protected EnemyAttack m_Attack;

    // Player
    protected Player m_Player;

    // Método para cargar los estados
    protected void Load() {
        m_States = new List<BasicState>();
        // Get
        m_Iddle = GetComponent<EnemyIddle>();
        m_Patrol = GetComponent<EnemyPatrol>();
        m_Chase = GetComponent<EnemyChase>();
        m_Attack = GetComponent<EnemyAttack>();
        // Add
        m_States.Add(m_Iddle);
        m_States.Add(m_Patrol);
        m_States.Add(m_Chase);
        m_States.Add(m_Attack);
        // Set
        m_CurrentState = EnemyStates.INNITIAL_STATE;

        // Get Player
        m_Player = uCore.GameManager.GetPlayer();

        // Restart GameElement
        uCore.GameManager.AddRestartGameElement(this);
    }

    // Método par iniciar el sistema
    protected void StarMachine() {
        m_States[(int)m_CurrentState].OnEnter();
    }

    // Unity Update
    protected void UpdateStates() {
        // Si no estoy alive, no hay update
        if (!m_Alive)
            return;
        
        // Update
        m_States[(int)m_CurrentState].OnUpdate();
    
        // FLOW DE ESTADOS //
        switch (m_CurrentState) {

            // --- IDDLE --- //
            case EnemyStates.IDDLE:
                // TO PATROL // No reason
                if (m_Alive) {
                    ChangeState(EnemyStates.PATROL);
                }
                break;

            // --- PATROL --- //
            case EnemyStates.PATROL:
                // TO CHASE // Cerca del player
                if (m_Chase.DetectionDistance()) {
                    ChangeState(EnemyStates.CHASE);
                }
                break;

            // --- CHASE --- //
            case EnemyStates.CHASE:
                // TO ATTACK // Si hemos acabado de chasear
                if (m_Chase.ChaseEnded()) {
                    ChangeState(EnemyStates.ATTACK);
                } else {
                    // TO PATROL // Si salimos de la distancia de chase 
                    if (!m_Chase.DetectionDistance()) {
                        ChangeState(EnemyStates.PATROL);
                    }
                }
                break;

            // --- ATTACK --- //
            case EnemyStates.ATTACK:
                // TO PATROL // Si hemos acabado el ataque
                if (m_Attack.AttackEnds()) {
                    ChangeState(EnemyStates.PATROL);
                }
                break;

            default: break;
        }
    }
    
    // Método abstracto para gestionar el golpe, dpeendiendo del enemigo
    public abstract void GotHit();

    // M�todo para cambiar de estado
    // In: EnemyStates newState -> Nuevo estado
    protected void ChangeState(EnemyStates newState) {
        m_States[(int)m_CurrentState].OnExit();
        m_CurrentState = newState;
        m_States[(int)m_CurrentState].OnEnter();
    }

    // * --------------------------------------------------------------------------------- *
    // | - COLLISION --------------------------------------------------------------------- |
    // V --------------------------------------------------------------------------------- V

    // OnTriggerEnter
    void OnTriggerEnter(Collider other) {
        // Player
        if (other.tag == "Player") {
            if (m_Attack.IsRushing()) {
                m_Player.TakeDamage();
            }
        }
        // Attack
        else if (other.tag == "Attack") {
            GotHit();
        }
    }

    // IRestartGameElement
    public void RestartGame() {
        this.gameObject.SetActive(true);
        m_CurrentState = EnemyStates.INNITIAL_STATE;
        m_Alive = true;
    }

    // A --------------------------------------------------------------------------------- A

}
