using UnityEngine;

public class AmmoItem : Item {

    [SerializeField, Header("Extras:")]
    private int m_Ammunition;

    // Override del Pick
    public override void Pick(Player player) {
        player.Shooting().AddAmmo(m_Ammunition);
        uCore.Audio.PlaySFX("ammo", player.transform.position);
        Deactivate();
    }
}
