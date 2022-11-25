using System.Collections;
using UnityEngine;

public class Destroyable : MonoBehaviour {

    [SerializeField]
    public float m_DestroyOnTime;

    // Unity Start
    void Start() {
        StartCoroutine(DestroyOnTime());
    }

    // Método para Coroutine
    private IEnumerator DestroyOnTime() {
        yield return new WaitForSeconds(m_DestroyOnTime);
        GameObject.Destroy(this.gameObject);
    }


}
