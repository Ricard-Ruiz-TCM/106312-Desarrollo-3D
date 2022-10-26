using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField, Header("Speed:")]
    private float m_Speed;
    [SerializeField, Header("Layer \"Disparable\":")]
    private LayerMask m_Layer;
    [SerializeField, Header("Damage:")]
    private float m_Damage;
    private Vector3 m_Forward;
    [SerializeField, Header("Prefab del Decal:")]
    private GameObject m_DecalPrefab;

    // Unity Update
    void Update() {
        // Movimiento
        transform.position += m_Speed * m_Forward * Time.deltaTime;

        // Raycast
        RaycastHit l_raycastHit;
        if (Physics.Raycast(transform.position, m_Forward, out l_raycastHit, 1f, m_Layer.value)) {
            GameObject d = l_raycastHit.collider.gameObject;
            // Inenta hacer daño a un enemigo, si no lo consigue ...
            if (!TryHitEnemy(l_raycastHit.collider.gameObject)) {
                // Intenta dar a un galleryItem
                TryHitGalleryItem(l_raycastHit.collider.gameObject);
                // ... Instnacia un decal
                d = InstantiateDecal(l_raycastHit.collider, l_raycastHit.point, l_raycastHit.normal);
            }

            InstantiateParticles(d);
            // Destruye la bala ;3 
            Destroy(this.gameObject);
        }

    }

    // Método para intentar golpar al enemegio
    // In: GameObject enemy -> Objeto donde la bala colisiona
    // Out: bool -> (true -> ha colisionado con un enemigo | false -> no ha colisionado con u nenemigo)
    private bool TryHitEnemy(GameObject enemy) {
        if (enemy.tag != "Enemy")
            return false;
        // Aplica el daño al enemigo
        enemy.GetComponent<HitCollider>().Hit(m_Damage);
        return true;
    }

    // Método para intentar golpar al item
    // In: GameObject item -> Objeto donde la bala colisiona
    // Out: bool -> (true -> ha colisionado con un item | false -> no ha colisionado con u item)
    private void TryHitGalleryItem(GameObject item) {
        if (item.tag != "GalleryItem")
            return;
        item.GetComponent<GalleryItem>().Hit();
    }

    // Método que instnacia el Decal del arma
    // In: Collider obj -> Objeto donde colisionamos
    // In: Vector3 position -> Posición en el mundo
    // In: Vector3 normal -> Vector Director del objeeto con el que colisionoç
    // Out: GameObject -> Objeto recine creado
    private GameObject InstantiateDecal(Collider obj, Vector3 position, Vector3 normal) {
        Debug.DrawRay(position, normal, Color.red, 5f);
        GameObject d = Instantiate(m_DecalPrefab, position, Quaternion.LookRotation(normal));
        d.transform.SetParent(obj.transform);
        return d;
    }

    // Método para instanciar particles
    // In: Collider obj -> Objeto donde colisionamos
    private void InstantiateParticles(GameObject obj) {
        if (obj.tag == "Concrete") {
            uCore.Particles.PlayParticlesOnce("BImpact_Concrete", obj.transform);
        } else if (obj.tag == "Wood") {
            uCore.Particles.PlayParticlesOnce("BImpact_Wood", obj.transform);
        } else {
            uCore.Particles.PlayParticlesOnce("BImpact_Metal", obj.transform);
        }
    }

    // Establecemos la dirección de la bala a.k.a Forward
    // In: Vector3 forward -> Dirección forward para la bala
    // In: float speed -> Velocidad de la bala
    public void SetBulletDirection(Vector3 forward, float speed) {
        m_Forward = forward;
        m_Speed = speed;
    }
}