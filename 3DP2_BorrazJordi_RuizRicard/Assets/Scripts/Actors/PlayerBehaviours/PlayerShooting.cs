using System;
using UnityEngine;

// Enum para definir los portales que existen
public enum PORTAL {
    GHOSTING, BLUE, ORANGE
}

public class PlayerShooting : MonoBehaviour {

    public static event Action<PORTAL> OnShootPortal;

    [SerializeField]
    private bool m_Locked;

    [SerializeField]
    private bool m_Ghosting;
    public bool IsGhosting() { return m_Ghosting; }

    [SerializeField, Header("Portales: ")]
    private BasicPortal m_OrangePortal;
    [SerializeField]
    private BasicPortal m_DummyPortal;
    [SerializeField]
    private BasicPortal m_BluePortal;

    // Control, Si el DummyPortal da el visto bueno para colocarlo
    private bool m_CanPlace;
    public bool CanPlacePortal() { return m_CanPlace; }

    // Control de si puedo disparar
    [SerializeField]
    private bool m_CanShoot;
    public bool CanShoot() { return m_CanShoot; }
    public void EnableShoot() { m_CanShoot = true; }
    public void DisableShoot() { m_CanShoot = false; }

    private Vector3 m_Position;
    private Vector3 m_Normal;

    // ghosting
    private PORTAL m_GhostingPortal;
    public PORTAL GhostingPortal() { return m_GhostingPortal; }

    // Unity Start
    void Start() {
        m_Locked = false;
        m_BluePortal.gameObject.SetActive(false);
        m_DummyPortal.gameObject.SetActive(false);
        m_OrangePortal.gameObject.SetActive(false);

        m_CanPlace = false;
        m_CanShoot = true;
        m_Normal = Vector3.zero;
        m_Position = Vector3.zero;
    }

    // Método para ghostar la posición del dummy
    public void GhostingPortal(PORTAL portal) {
        if (m_Locked || !m_CanShoot)
            return;

        m_Ghosting = true;
        m_GhostingPortal = portal;

        // Creamos un rayo a mitad de la pantalla
        Ray l_ray = uCore.GameManager.GetPlayer().Camera().ViewportPointToRay(new Vector3(0.5f, 0.5f));
        m_Position = l_ray.origin;
        m_Normal = l_ray.direction;

        // Intentamos colocar el dummy por el mapa
        m_CanPlace = m_DummyPortal.TryPlace(m_Position, m_Normal);
        m_DummyPortal.gameObject.SetActive(m_CanPlace);
    }

    // Método para re-escalar el portal dummy mientras estamos ghosteando
    // In: float factor -> factor de escalada
    public void Resize(float factor) {
        if (!m_Ghosting)
            return;

        m_DummyPortal.Resize(factor);
    }

    // Método para colocar el portal
    // In: PORTAL_TYPE type -> tipo del portal
    public void ShootPortal(PORTAL portal) {
        m_Ghosting = false;

        // Si no puedo colocarlo, estoy fuera
        if (!m_CanPlace) {
            switch (m_GhostingPortal) {
                case PORTAL.BLUE:
                    if (m_BluePortal.gameObject.activeSelf) {
                        m_BluePortal.GetComponent<Portal>().Mirror().Deactivate();
                        m_BluePortal.gameObject.SetActive(false);
                    }
                break;
                case PORTAL.ORANGE:
                    if (m_OrangePortal.gameObject.activeSelf) {
                        m_OrangePortal.GetComponent<Portal>().Mirror().Deactivate();
                        m_OrangePortal.gameObject.SetActive(false);
                    }
                break;
            }
            OnShootPortal?.Invoke(PORTAL.GHOSTING);
            return;
        }

        switch (portal) {
            // Blue Portal
            case PORTAL.BLUE:
                PlacePortal(m_BluePortal);
                OnShootPortal?.Invoke(PORTAL.BLUE);
                uCore.Audio.Play2DSFX("bluePortal", this.transform.position);

                break;
            // Orange Portal
            case PORTAL.ORANGE:
                PlacePortal(m_OrangePortal);
                OnShootPortal?.Invoke(PORTAL.ORANGE);
                uCore.Audio.Play2DSFX("orangePortal", this.transform.position);
                break;

            default: break;
        }

        // Reset de valores
        m_CanPlace = false;
        m_Normal = Vector3.zero;
        m_Position = Vector3.zero;
        // Set de la escala base
        m_DummyPortal.SetScale(1.0f);
    }

    // Método interno para colocar un portal
    // In: GameoObject portal -> Objeto portal que vamos a intenar colocar
    private void PlacePortal(BasicPortal portal) {
        portal.gameObject.SetActive(true);
        if (portal.GetComponent<Portal>().Mirror().gameObject.activeSelf) {
            portal.ResetMat();
            portal.GetComponent<Portal>().Mirror().ResetMat();
        }
        portal.SetScale(m_DummyPortal.CurrentScale());
        m_DummyPortal.gameObject.SetActive(false);
        portal.TryPlace(m_Position, m_Normal);
    }

    // Método para alterar el bloqueo del shooting
    public void ToggleShooting() {
        m_Locked = !m_Locked;
    }

}
