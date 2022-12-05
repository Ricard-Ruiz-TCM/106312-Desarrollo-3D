using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    // OnTriggerEnter
    void OnTriggerEnter(Collider other) {
        Debug.Log(other.tag);
        // Player
        if (other.tag == "Enemy") {
            if (other.gameObject.GetComponent<Goomba>() != null) {
                other.gameObject.GetComponent<Goomba>().Kill();
            } else if (other.gameObject.GetComponent<Enemy>() != null) {
                other.gameObject.GetComponent<Enemy>().GotHit();
            }
        }
    }
}
