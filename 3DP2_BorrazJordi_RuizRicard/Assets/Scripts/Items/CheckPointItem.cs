
public class CheckPointItem : Item {

    // Override del Pick
    public override void Pick(Player player) {
        player.CheckPointSys().SaveData();
        uCore.Audio.Play2DSFX("solve", this.transform.position);
        Deactivate();
    }

}
