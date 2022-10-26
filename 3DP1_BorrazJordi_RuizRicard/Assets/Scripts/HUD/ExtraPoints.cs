using System.Collections;
using UnityEngine;

public class ExtraPoints : MonoBehaviour {

    private float m_DurationTime = 2.5f;

    void Start() {
        StartCoroutine(Duration(m_DurationTime));
    }

    private IEnumerator Duration(float time) {
        yield return new WaitForSeconds(time);
        Animation a = GetComponent<Animation>();
        a.Play("Score_HIDE");
        StartCoroutine(DestroyMe(a.clip.length));
    }

    private IEnumerator DestroyMe(float time) {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }



}
