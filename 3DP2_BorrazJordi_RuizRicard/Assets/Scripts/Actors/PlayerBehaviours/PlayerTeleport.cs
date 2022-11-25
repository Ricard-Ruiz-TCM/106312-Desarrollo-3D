using UnityEngine;

public class PlayerTeleport : MonoBehaviour {

    [SerializeField, Range(0.0f, 90.0f)]
    private float m_AngleToEnterPortal = 35.0f;
    [SerializeField]
    private float m_TeleportOffset = 2f;

    // Player
    private Player m_Player;

    // Unity Awake
    void Awake() {
        m_Player = uCore.GameManager.GetPlayer();
    }

    // Método para comprobar si se puede teletransportar
    // In: Portal portal -> Portal con el que intento teletransportarme
    public void TryTeleport(Portal portal) {
        if (Vector3.Dot(-m_Player.Direction(), portal.transform.forward) > Mathf.Cos(m_AngleToEnterPortal * Mathf.Deg2Rad))
            Teleport(portal);
    }

    // Método para relizar el teleprot del player
    // In: Portal portal -> Portal con el que ha chocado el player
    private void Teleport(Portal portal) {

        // Si el mirror esta desactivado, nos vamos
        if (!portal.MirrorON())
            return;

        Vector3 l_LocalPosition = portal.Other().InverseTransformPoint(transform.position);
        Vector3 l_Direction = portal.Other().transform.InverseTransformDirection(transform.forward);
        Vector3 l_LocalDirectionMovement = portal.Other().transform.InverseTransformDirection(m_Player.Direction());
        Vector3 l_WorldDirectionMovement = portal.Mirror().transform.TransformDirection(l_LocalDirectionMovement);

        m_Player.MovementSys().DisableMovement();
        transform.forward = portal.Mirror().transform.TransformDirection(l_Direction);
        m_Player.MovementSys().SetYaw(transform.rotation.eulerAngles.y);
        float l_Scale = portal.Mirror().CurrentScale();
        transform.localScale = new Vector3(l_Scale, l_Scale, l_Scale);
        transform.position = portal.Mirror().transform.TransformPoint(l_LocalPosition) + l_WorldDirectionMovement * m_TeleportOffset;

        m_Player.MovementSys().EnableMovement();
    }
}
