using UnityEngine;

public class Player : MonoBehaviour {

    // Camera
    [SerializeField, Header("Main Camera:")]
    private Camera m_Camera;
    public Camera Camera() { return m_Camera; }

    // Dirección para el calc del tp
    private Vector3 m_Direction;
    public Vector3 Direction() { return m_Direction; }

    // Player Components
    private PlayerJump m_Jump;
    public PlayerJump JumpSys() { return m_Jump; }
    private PlayerCatch m_Catch;
    public PlayerCatch CatchSys() { return m_Catch; }
    private PlayerMovement m_Movement;
    public PlayerMovement MovementSys() { return m_Movement; }
    private PlayerShooting m_Shooting;
    public PlayerShooting ShootingSys() { return m_Shooting; }
    private PlayerTeleport m_Teleport;
    public PlayerTeleport TeleportSys() { return m_Teleport; }
    private PlayerDeath m_Death;
    public PlayerDeath DeathSys() { return m_Death; }
    private PlayerCheckPoint m_CheckPoint;
    public PlayerCheckPoint CheckPointSys() { return m_CheckPoint; }
    private PlayerInventory m_Inventory;
    public PlayerInventory InventorySys() { return m_Inventory; }

    // Unity OnEnable
    void OnEnable() {
        PlayerDeath.OnDie += ShowRespawn;
        PlayerDeath.OnRespawn += HideRespawn;

        PlayerInventory.OnKeyring += UpdateKeyHUD;
    }

    // Unity OnDisable
    void OnDisable() {
        PlayerDeath.OnDie -= ShowRespawn;
        PlayerDeath.OnRespawn += HideRespawn;

        PlayerInventory.OnKeyring -= UpdateKeyHUD;
    }

    // Unity Awake
    void Awake() {
        m_Movement = GetComponent<PlayerMovement>();
        m_Jump = GetComponent<PlayerJump>();
        m_Shooting = GetComponent<PlayerShooting>();
        m_Catch = GetComponent<PlayerCatch>();
        m_Teleport = GetComponent<PlayerTeleport>();
        m_Death = GetComponent<PlayerDeath>();
        m_Inventory = GetComponent<PlayerInventory>();
        m_CheckPoint = GetComponent<PlayerCheckPoint>();
    }

    // Unity Start(){
    void Start() {
        m_Direction = Vector3.zero;
    }

    // Unity Update
    void Update() {

        // Si estamos muertos, estamos OUT
        if (!m_Death.IsAlive())
            return;

        // Movement Calcs
        m_Direction = Vector3.zero;
        float l_speed = m_Movement.Speed();
        if (uCore.Input.KeyW())
            m_Direction = transform.forward;
        if (uCore.Input.KeyS())
            m_Direction = -transform.forward;
        if (uCore.Input.KeyD())
            m_Direction += transform.right;
        if (uCore.Input.KeyA())
            m_Direction -= transform.right;
        if (uCore.Input.LShift())
            l_speed *= m_Movement.SpeedMultiplier();

        m_Direction.Normalize();

        // Movement
        m_Movement.Move(m_Direction, l_speed, uCore.Input.MouseMovement());

        // Jump
        if (uCore.Input.Space())
            m_Jump.Jump();

        // Attach
        if ((!m_Shooting.IsGhosting()) || (m_Catch.IsObjectAttached())) {
            if (uCore.Input.LMClickPress()) {
                m_Catch.Attach();
            } else if (uCore.Input.RMClickPress()) {
                m_Catch.DeAttach();
            }
            if ((uCore.Input.LMClickRelease()) || (uCore.Input.RMClickRelease())) {
                m_Catch.EnableThrow();
                if (!m_Catch.IsObjectAttached())
                    m_Catch.EnableAttach();
            }
        }

        // Disable shooting si tengo objeto attached
        if ((m_Catch.IsPullling()) || (m_Catch.IsObjectAttached())) {
            m_Shooting.DisableShoot();
        }

        // Shooting
        if ((!m_Catch.IsPullling()) && (!m_Catch.IsObjectAttached()) && (m_Shooting.CanShoot())) {
            if (uCore.Input.LMClickPress()) {
                m_Shooting.GhostingPortal(PORTAL.BLUE);
            } else if (uCore.Input.RMClickPress()) {
                m_Shooting.GhostingPortal(PORTAL.ORANGE);
            }
            if (uCore.Input.LMClickRelease()) {
                m_Shooting.ShootPortal(PORTAL.BLUE);
            } else if (uCore.Input.RMClickRelease()) {
                m_Shooting.ShootPortal(PORTAL.ORANGE);
            }
        }

        // Resizing
        if (m_Shooting.IsGhosting()) {
            if (uCore.Input.IsMouseScrolling()) {
                m_Shooting.Resize(uCore.Input.MouseScroll());
            }
        }

        // Enable del shoting si "reliseo" el mouse
        if (uCore.Input.LMClickRelease() || uCore.Input.RMClickRelease()) {
            m_Shooting.EnableShoot();
        }

    }

    // * --------------------------------------------------------------------------------- *
    // | - COLLISION --------------------------------------------------------------------- |
    // V --------------------------------------------------------------------------------- V

    // OnTriggerEnter
    void OnTriggerEnter(Collider other) {
        // Collision Portal
        if (other.tag == "Portal") {
            m_Teleport.TryTeleport(other.GetComponent<Portal>());
            uCore.Audio.Play2DSFX("teleport", this.transform.position);
        }

        // Collision DeadZone
        if (other.tag == "DeadZone") {
            m_Death.Die();
        }

        // Items
        if (other.tag == "Item") {
            other.GetComponent<Item>().Pick(this);
        }
    }

    // OnTriggerExit
    void OnTriggerExit(Collider other) { }

    // A --------------------------------------------------------------------------------- A

    // * --------------------------------------------------------------------------------- *
    // | - HUD - UPDATES ----------------------------------------------------------------- |
    // V --------------------------------------------------------------------------------- V

    // Método con fundido a negro para mostrar el menu de respawn
    public void ShowRespawn() {
        uCore.FadeFX.FadeIn(uCore.Canvas.ShowRespawn);
    }

    // Método con fundido de negro, para ocultar el respawn
    public void HideRespawn() {
        uCore.FadeFX.FadeOut();
        uCore.Canvas.HideRespawn();
    }

    // Método para mostrar el icono de llave en el HUD
    public void UpdateKeyHUD(bool key) {
        if (key)
            uCore.Canvas.ShowKey();
        if (!key)
            uCore.Canvas.HideKey();
    }

    // A --------------------------------------------------------------------------------- A
}
