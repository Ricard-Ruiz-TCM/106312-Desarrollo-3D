using UnityEngine;

public class KeyItem : Item {

    [SerializeField, Header("Extras:")]
    private int m_ID;

    // Override del Pick
    public override void Pick(Player player) {
        player.KeyRing().AddKey(m_ID);
        uCore.Audio.PlaySFX("key", this.transform.position);
        Deactivate();
    }
}
