using UnityEngine;

public class LifeItem : Item {

    [SerializeField, Header("Extras:")]
    private float m_Life;

    // Override del Pick
    public override void Pick(Player player) {
        if (!player.Vitals().CanHeal())
            return;
        player.Vitals().PlayerHealth().Heal(m_Life);
        uCore.Audio.PlaySFX("heal", player.transform.position);

        Deactivate();
    }
}
