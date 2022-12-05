public class StarItem : Item {

    // Unity Awake
    void Awake() {
        // Restart GameElement
        uCore.GameManager.AddRestartGameElement(this);
    }

    // Override del Pick
    public override void Pick(Player player) {
        player.Health.MakeInvencible();
        DeactivateParent();
        uCore.Audio.Play2DSFX("Mario_StarPowerUp", this.transform.position);
        uCore.Particles.PlayParticlesOnce("Estrellas", this.transform.position);

    }

}