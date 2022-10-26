using UnityEngine;

public class ShieldItem : Item {

    [SerializeField, Header("Extras:")]
    private float m_Shield;

    // Override del Pick
    public override void Pick(Player player) {
        if (!player.Vitals().CanIncreaseShield())
            return;

        player.Vitals().Playershield().IncreaseShield(m_Shield);
        uCore.Audio.PlaySFX("shield", player.transform.position);
        Deactivate();
    }
}
