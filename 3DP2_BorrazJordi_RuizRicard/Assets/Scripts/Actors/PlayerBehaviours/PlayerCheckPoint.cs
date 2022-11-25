using System;
using UnityEngine;

public class PlayerCheckPoint : MonoBehaviour {

    // Observer para saber cuando cogemos un chekcpoint
    public static event Action OnSaveData;

    // Transform
    private Vector3 m_Scale;
    private Vector3 m_Position;
    private Quaternion m_Rotation;

    // CharacterController
    private PlayerMovement m_Movement;

    // Unity Awake
    void Awake() {
        m_Movement = GetComponent<PlayerMovement>();
    }

    // Unity Start
    void Start() {
        SaveData();
    }

    // M�todo par aguardar la informaci�n relevante al checkpoint
    public void SaveData() {
        m_Scale = this.transform.localScale;
        m_Position = this.transform.position;
        m_Rotation = this.transform.rotation;
        OnSaveData?.Invoke();
    }

    // M�todo para respawnear
    public void RestoreData() {
        m_Movement.DisableMovement();
        this.transform.localScale = m_Scale;
        this.transform.position = m_Position;
        this.transform.rotation = m_Rotation;
        m_Movement.EnableMovement();
    }

}
