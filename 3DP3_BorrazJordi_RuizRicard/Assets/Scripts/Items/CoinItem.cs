
public class CoinItem : Item {

    private ItemID m_ID = ItemID.Coin;

    // Unity Awake
    void Awake() {
        // Restart GameElement
        uCore.GameManager.AddRestartGameElement(this);
    }

    // Override del Pick
    public override void Pick(Player player) {
        player.Inventory.PickItem(m_ID);
        DeactivateParent();
        uCore.Particles.PlayParticlesOnce("Coin",this.transform.position);
    }

}