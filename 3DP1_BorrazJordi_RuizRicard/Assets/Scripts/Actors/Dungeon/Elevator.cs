using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour {

    [SerializeField]
    private bool m_Ground;
    [SerializeField]
    private bool m_OnAnimation;

    // Animaiton
    private Animation m_Animation;
    [SerializeField, Header("Animations:")]
    private AnimationClip m_Up;
    [SerializeField]
    private AnimationClip m_Down;

    // Unity Awake
    void Awake() {
        m_Animation = GetComponent<Animation>();
    }

    // Unity Start
    void Start() {
        m_Ground = true;
    }

    public void Run() {
        if (m_OnAnimation)
            return;

        if (m_Ground)
            Rise();

        else if (!m_Ground)
            Fall();
    }

    private void Rise() {
        m_Ground = false;
        StartCoroutine(EndAnimation(m_Up.length));
        m_Animation.Play(m_Up.name);
    }

    private void Fall() {
        m_Ground = true;
        StartCoroutine(EndAnimation(m_Down.length));
        m_Animation.Play(m_Down.name);
    }

    private IEnumerator EndAnimation(float time) {
        m_OnAnimation = true;
        yield return new WaitForSeconds(time);
        m_OnAnimation = false;
    }

}
