using UnityEngine;

public class Billboard : MonoBehaviour {

    // Unity LateUpdate
    void LateUpdate() {
        transform.LookAt(Camera.main.transform);
    }
}
