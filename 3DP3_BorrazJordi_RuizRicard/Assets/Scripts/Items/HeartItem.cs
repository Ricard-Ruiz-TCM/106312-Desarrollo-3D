using UnityEngine;

public class HeartItem : Item {

    [SerializeField]
    private float m_Amount = 0.125f;

    // Unity Awake
    void Awake() {
        // Restart GameElement
        uCore.GameManager.AddRestartGameElement(this);
    }

    // Override del Pick
    public override void Pick(Player player) {
        player.Health.Heal(m_Amount);
        DeactivateParent();
        uCore.Audio.Play2DSFX("get-life-loud", this.transform.position);
        uCore.Particles.PlayParticlesOnce("HeartLife", this.transform.position);
    }

}