using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDie : BasicState {

    [SerializeField, Header("Die State:")]
    private AnimationClip m_ExplodeClip;

    [SerializeField]
    private float m_FallSpeed;
    [SerializeField]
    private float m_DestroyTime;

    [SerializeField]
    private GameObject m_Drone;

    // NavMeshAgent
    private NavMeshAgent m_Agent;

    [SerializeField, Header("Items 2 Drop:")]
    private GameObject[] m_Items;

    // Unity Awake
    void Awake() {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    // OnEnter
    public override void OnEnter() {
        m_Agent.destination = this.transform.position;
        GetComponentInChildren<Animator>().Play(m_ExplodeClip.name);

        StartCoroutine(DropItem(m_DestroyTime));
    }

    // OnExit
    public override void OnExit() { }

    // OnUpdate
    public override void OnUpdate() {
        // No bajamos más que el suelo
        if (m_Drone.transform.position.y <= 0.0f)
            return;

        // Nos movemos hacia abajo del chill
        float distance = m_FallSpeed * Time.unscaledDeltaTime;
        m_Drone.transform.Translate(new Vector3(0.0f, -distance, 0.0f));
    }

    private IEnumerator DropItem(float wait) {
        yield return new WaitForSeconds(wait);
        GameObject g = Instantiate(m_Items[Random.Range(0, m_Items.Length - 1)], uCore.GameManager.ActorContainer());
        g.transform.position = m_Drone.transform.position;
        uCore.Audio.PlaySFX("dron_death", this.transform.position);
        this.gameObject.SetActive(false);
    }

}
