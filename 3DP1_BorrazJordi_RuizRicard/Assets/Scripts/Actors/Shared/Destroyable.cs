using System.Collections;
using UnityEngine;

public class Destroyable : MonoBehaviour {

    [SerializeField]
    public float m_DestroyOnTime = 3.0f;

    // Unity Start
    void Start() {
        StartCoroutine(DestroyOnTime());
    }

    // M�todo para Coraoutine, destruir� el objeto
    private IEnumerator DestroyOnTime() {
        yield return new WaitForSeconds(m_DestroyOnTime);
        GameObject.Destroy(this.gameObject);
    }


}
