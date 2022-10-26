using UnityEngine;

public class Billboard : MonoBehaviour {
    // Unity Update
    void Update() {
        transform.LookAt(Camera.main.transform);
    }
}
