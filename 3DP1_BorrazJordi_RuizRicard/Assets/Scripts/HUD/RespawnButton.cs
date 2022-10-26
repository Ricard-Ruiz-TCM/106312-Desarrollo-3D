using System;
using UnityEngine;

public class RespawnButton : MonoBehaviour {

    // Observer de restet
    public static event Action OnPlayerRespawn;

    // M�todo para el respawn
    public void Respawn() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        OnPlayerRespawn?.Invoke();
        this.gameObject.SetActive(false);
    }

}
