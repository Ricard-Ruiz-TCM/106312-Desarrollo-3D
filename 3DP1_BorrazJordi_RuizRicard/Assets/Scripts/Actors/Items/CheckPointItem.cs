
public class CheckPointItem : Item {

    // Override del Pick
    public override void Pick(Player player) {
        uCore.Audio.PlaySFX("checkpoint", player.transform.position);
        player.CheckPoint();
        Deactivate();
    }
}
