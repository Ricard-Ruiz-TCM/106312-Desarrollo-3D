using UnityEngine;

public class KeyItem : Item {

    [SerializeField, Header("Key ID:")]
    private int m_ID;

    // Unity Awake
    void Awake() {
        this.name = "Key ID -> [" + m_ID.ToString() + "]";
    }

    // Override del Pick
    public override void Pick(Player player) {
        player.InventorySys().AddKey(m_ID);
        uCore.Audio.Play2DSFX("key", this.transform.position);
        Deactivate();
    }
}
