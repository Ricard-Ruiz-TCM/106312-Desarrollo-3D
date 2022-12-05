using UnityEngine;

public class PunchBehaviour : StateMachineBehaviour {

    [SerializeField, Header("Control")]
    private bool m_Active;

    [SerializeField, Header("Time Control:")]
    private float m_StartTime;
    [SerializeField]
    private float m_EndTime;

    [SerializeField, Header("Elemento de golpe:")]
    public TPunchType m_Type;

    // Player
    private Player m_Player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (m_Player == null) {
            m_Player = animator.gameObject.GetComponent<Player>();
        }

        m_Player.Attack.StartAttack();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (!m_Active && stateInfo.normalizedTime >= m_StartTime && stateInfo.normalizedTime <= m_EndTime) {
            m_Player.Attack.SetPunchActive(m_Type, true);
            m_Active = true;
        } else if (m_Active && stateInfo.normalizedTime > m_EndTime) {
            m_Player.Attack.SetPunchActive(m_Type, false);
            m_Active = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        m_Player.Attack.SetPunchActive(m_Type, false);
        m_Active = false;
        m_Player.Attack.EndAttack();
    }

}
