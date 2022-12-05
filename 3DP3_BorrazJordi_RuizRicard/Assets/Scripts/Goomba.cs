using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Goomba : MonoBehaviour, IRestartGameElement {

    public EnemyStates state;

    public Player player;
    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Waypoints")]
    public Transform[] wayPoints;
    NavMeshAgent agent;
    public int wayPointsIndex;
    Vector3 walkPoint;

    bool walkPointSet;

    public float m_ExtraSpeed = 7;
    public float m_ExtraAcc = 16;

    //attacking
    public float timeBetweenAtttacks;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    Vector3 m_goombaGoAttack;

    Animator m_animator;

    public float m_TimeAttacking;

    public bool m_hasArrivedTargetAfterAttacking;

    //kill
    public float m_KillTime = 0.5f;
    public float m_KillScale = 0.2f;
    public void Awake() {
        player = uCore.GameManager.GetPlayer();
        m_animator = GetComponent<Animator>();
    }

    void Start() {
        //GameController.GetGameController().AddRestartGameElement(this);
        m_hasArrivedTargetAfterAttacking = true;
        SetPatrol();
        agent = GetComponent<NavMeshAgent>();
        UpdateDestination();
        uCore.GameManager.AddRestartGameElement(this);

    }

    void Update() {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        switch (state) {
            case EnemyStates.PATROL:
            UpdatePatrol();
            break;
            case EnemyStates.CHASE:
            UpdateChase();
            break;
            case EnemyStates.ATTACK:
            UpdateAttack();
            break;
        }
    }


    void SetPatrol() {
        state = EnemyStates.PATROL;
        m_animator.Play("Walk");
    }

    void UpdatePatrol() {
        RestartSpeedAcca();
        if (!walkPointSet) IterativeWaypointIndex();
        if (walkPointSet) UpdateDestination();

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) {
            m_hasArrivedTargetAfterAttacking = true;
            walkPointSet = false;

        }



        if (playerInSightRange && m_hasArrivedTargetAfterAttacking) SetChase();


    }

    void UpdateDestination() {
        walkPoint = wayPoints[wayPointsIndex].position;

        agent.SetDestination(walkPoint);
    }

    void IterativeWaypointIndex() {
        wayPointsIndex++;
        walkPointSet = true;
        if (wayPointsIndex == wayPoints.Length) {
            wayPointsIndex = 0;
        }
    }

    void RestartSpeedAcca() {
        agent.speed = 4f;
        agent.acceleration = 7f;


    }
    void SetChase() {
        state = EnemyStates.CHASE;
        m_animator.Play("Alert");
        uCore.Audio.Play2DSFX("goomba-whatwhat", this.transform.position);
        agent.SetDestination(player.transform.position);

    }

    private void UpdateChase() {

        if (playerInAttackRange) SetAttack();
        if (!playerInSightRange && !playerInAttackRange)
            SetPatrol();
    }

    void SetAttack() {
        Vector3 dir = player.transform.position - this.transform.position;
        dir.Normalize();
        m_goombaGoAttack = player.transform.position + (3 * dir);
        agent.speed = m_ExtraSpeed;
        agent.acceleration = m_ExtraAcc;
        state = EnemyStates.ATTACK;
        m_animator.Play("Run");
    }

    private void UpdateAttack() {

        agent.SetDestination(m_goombaGoAttack);
        if (agent.SetDestination(m_goombaGoAttack)) {
            StartCoroutine(EndAttack());
        }
    }

    public void Kill() {
        uCore.Audio.Play2DSFX("goomba-die", this.transform.position);
        uCore.Particles.PlayParticlesOnce("Enemy_Dead", this.transform.position);
        transform.localScale = new Vector3(1.0f, m_KillScale, 1.0f);
        StartCoroutine(Hide());
    }
    private IEnumerator EndAttack() {
        yield return new WaitForSeconds(m_TimeAttacking);
        SetPatrol();
        m_hasArrivedTargetAfterAttacking = false;
    }
    IEnumerator Hide() {
        yield return new WaitForSeconds(m_KillTime);
        gameObject.SetActive(false);

    }
    public void RestartGame() {
        transform.localScale = Vector3.one;
        gameObject.SetActive(true);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    // OnTriggerEnter
    void OnTriggerEnter(Collider other) {
        // Player
        if (other.tag == "Player") {
            if (state == EnemyStates.ATTACK) {
                player.TakeDamage();
            }
        }
        // Attack
        else if (other.tag == "Attack") {
            Kill();
        }
    }
}
