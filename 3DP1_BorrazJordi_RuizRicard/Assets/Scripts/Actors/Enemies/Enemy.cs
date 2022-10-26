using System.Collections.Generic;
using UnityEngine;

// Enum para definir los estados que tienen los enemigos ;3 
public enum EnemyStates {
    DIE, HIT, ALERT, ATTACK, PATROL, IDDLE, CHASE,
    TOTAL_STATES,

    INNITIAL_STATE = EnemyStates.IDDLE,
}

[RequireComponent(typeof(EnemyDie)), RequireComponent(typeof(EnemyHit)), RequireComponent(typeof(EnemyAttack)), RequireComponent(typeof(EnemyAlert)), RequireComponent(typeof(EnemyPatrol)), RequireComponent(typeof(EnemyIddle)), RequireComponent(typeof(EnemyChase))]
public class Enemy : MonoBehaviour {

    [SerializeField, Header("Estado:")]
    private EnemyStates m_State;
    private EnemyStates m_LastState;
    private BasicState m_CurrentState;
    [SerializeField]
    private List<BasicState> m_States;

    [SerializeField]
    private Vector3 m_InnitialPosition;

    // Estados
    private EnemyDie m_Die;
    private EnemyHit m_Hit;
    private EnemyAlert m_Alert;
    private EnemyAttack m_Attack;
    private EnemyPatrol m_Patrol;
    private EnemyIddle m_Iddle;
    private EnemyChase m_Chase;

    // OnEnable
    void OnEnable() {
        RespawnButton.OnPlayerRespawn += () => { this.transform.position = m_InnitialPosition; };
    }

    // OnDisable
    void OnDisable() {
        RespawnButton.OnPlayerRespawn -= () => { this.transform.position = m_InnitialPosition; };
    }

    // Unity Awake
    void Awake() {
        // Get de los estados
        m_States = new List<BasicState>();
        m_Die = GetComponent<EnemyDie>();
        m_Hit = GetComponent<EnemyHit>();
        m_Alert = GetComponent<EnemyAlert>();
        m_Attack = GetComponent<EnemyAttack>();
        m_Patrol = GetComponent<EnemyPatrol>();
        m_Iddle = GetComponent<EnemyIddle>();
        m_Chase = GetComponent<EnemyChase>();

        // Add de los estados a la FSM
        // Relación directa de orden con los definidos en EnemyStates !!!!!!!!!
        m_States.Add(m_Die);
        m_States.Add(m_Hit);
        m_States.Add(m_Alert);
        m_States.Add(m_Attack);
        m_States.Add(m_Patrol);
        m_States.Add(m_Iddle);
        m_States.Add(m_Chase);

        // Set del estado inicial
        m_State = EnemyStates.INNITIAL_STATE;
        EnterState();
    }

    // Unity Start
    void Start() {
        m_InnitialPosition = this.transform.position;
    }

    // Método para cambiar de estado
    // In: EnemyStates newState -> Nuevo estado
    public void ChangeState(EnemyStates newState) {
        ExitState();
        m_LastState = m_State;
        m_State = newState;
        EnterState();
    }

    // Método para hacer call al OnExit
    private void ExitState() {
        m_CurrentState.OnExit();
    }

    // Método para hacer call del OnEnter y asignar m_CurrentState según m_State
    private void EnterState() {
        m_CurrentState = m_States[(int)m_State];
        m_CurrentState.OnEnter();
    }

    // Unity Update
    void Update() {
        // Uppdate del estado
        m_CurrentState.OnUpdate();

        // TO HIT // TO DI // Recibi un ´tiro y quizá morí :O 
        if (m_Hit.GotHit()) {
            if (!m_Hit.EnemyHealth().IsDead()) {
                ChangeState(EnemyStates.HIT);
            } else {
                GetComponent<Animation>().Stop();
                ChangeState(EnemyStates.DIE);
            }
            // NOS VAMOS TT
            return;
        }

        // Comprobación para el Flow de estados
        switch (m_State) {

            // ----------------------- HIT //
            case (EnemyStates.HIT):
                if (m_Hit.HitAnimEnds()) {
                    // TO ALERT // Ir a alerta al acabar animación
                    if (m_LastState == EnemyStates.PATROL)
                        ChangeState(EnemyStates.ALERT);
                    else
                        // TO LAST // Voy a la última si no es patrol la última D:
                        ChangeState(m_LastState);
                }
                break;

            // ----------------------- ALERT //
            case (EnemyStates.ALERT):
                if (!m_Alert.SeesPlayer()) {
                    // TO PATROL // Deja de ver/escuchar al player y acaba la animación
                    if (m_Alert.AlertAnimEnds()) {
                        ChangeState(EnemyStates.PATROL);
                    }
                } else {
                    // TO ATTACK // Ve al player, esta a rango de ataque
                    if (m_Attack.InShootRange()) {
                        ChangeState(EnemyStates.ATTACK);
                    } else {
                        // TO CHASE // Ve al player, no esta a rango de ataque
                        if (m_Chase.InsideMaxDistance()) {
                            ChangeState(EnemyStates.CHASE);
                        }
                    }
                }
                break;

            // ----------------------- ATTACK //
            case (EnemyStates.ATTACK):
                // TO CHASE // Si ha acabado de pegar, vuelve al chase 1 frame min
                if (m_Attack.AttackAnimEnd()) {
                    ChangeState(EnemyStates.CHASE);
                }
                break;

            // ----------------------- PATROL //
            case (EnemyStates.PATROL):
                // TO ALERT // Si escucha al player y esta en distancia
                if (m_Patrol.HearsPlayer()) {
                    ChangeState(EnemyStates.ALERT);
                }
                break;

            // ----------------------- IDDLE //
            case (EnemyStates.IDDLE):
                // TO PATROL // xke sí (idk)
                ChangeState(EnemyStates.PATROL);
                break;

            // ----------------------- CHASE //
            case (EnemyStates.CHASE):
                // TO PATROL // No ve al player
                if (m_Chase.PlayerFlee()) {
                    ChangeState(EnemyStates.PATROL);
                }
                // TO ATTACK // Si está dentro del rango de ataque
                else if (m_Attack.InShootRange()) {
                    ChangeState(EnemyStates.ATTACK);
                }
                break;

            default:
                break;
        }

    }

}
