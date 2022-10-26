using UnityEngine;

public enum ColliderType {
    HELIX, BODY, HEAD
}

public class HitCollider : MonoBehaviour {

    [SerializeField]
    private ColliderType m_BodyPart;

    [SerializeField]
    private EnemyHit m_Hit;

    // Método para recibir el golpe
    // In: float dmg -> daño recibido por la bala
    public void Hit(float dmg) {
        float l_dmg = dmg;

        switch (m_BodyPart) {
            case ColliderType.HELIX:
                l_dmg -= dmg * 0.75f;
                break;
            case ColliderType.BODY:
                l_dmg -= dmg * 0.25f;
                break;
        }

        m_Hit.Hit(l_dmg);
    }

}
