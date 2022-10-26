using UnityEngine;

public class WeaponItem : Item {

    [SerializeField, Header("Extras:")]
    private WeaponData m_Weapon;

    // Unity Start
    void Start() {
        GetComponentInChildren<SpriteRenderer>().sprite = m_Weapon.Icon;
    }

    // Override del Pick
    public override void Pick(Player player) {
        player.Shooting().LoadWeapon(m_Weapon);
        uCore.Audio.PlaySFX("new_Weapon", player.transform.position);

        Deactivate();
    }
}
