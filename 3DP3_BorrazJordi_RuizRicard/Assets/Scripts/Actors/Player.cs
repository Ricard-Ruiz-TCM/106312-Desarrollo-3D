using UnityEngine;

public class Player : MonoBehaviour, IRestartGameElement {

    [SerializeField, Header("Main Camera:")]
    private Camera m_Camera;
    public Camera Camera() { return m_Camera; }

    [SerializeField, Header("Iddle")]
    private float m_IddleTime;
    private float m_StandTime;

    // Player Compos
    public PlayerCheckPoint CheckPoint { get; private set; }
    public PlayerInventory Inventory { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerHealth Health { get; private set; }
    public PlayerAttack Attack { get; private set; }
    public PlayerJump Jump { get; private set; }

    [SerializeField, Header("Control Enganche:")]
    private float m_ElevatorDotAngle = 0.95f;
    [SerializeField]
    private Collider m_CurrentElevator = null;

    [SerializeField]
    private float m_BridgeForce = 2.3f;

    [SerializeField]
    private float m_MaxAngleToKill = 45.0f;

    // Animation 
    private Animator m_Animation;

    // Footsteps
    [SerializeField, Header("Pisadas:")]
    private float m_StepDelayLimit;
    private float m_StepDelay;
    public float m_StepDelayLimitParticle;
    private float m_StepDelayParticle;
    public Transform m_EmitterParticle;
    private bool _IsRunningOrNot;

    // Unity OnEnable
    void OnEnable() {
        PlayerHealth.OnDie += OnDie;
        PlayerHealth.OnRespawn += OnRespawn;
        PlayerInventory.OnPickItem += OnPickItem;
        PlayerHealth.OnHealthChanged += OnHealthChanged;
    }

    // Unity OnDisable
    void OnDisable() {
        PlayerHealth.OnDie -= OnDie;
        PlayerHealth.OnRespawn -= OnRespawn;
        PlayerInventory.OnPickItem -= OnPickItem;
        PlayerHealth.OnHealthChanged -= OnHealthChanged;
    }

    // Unity Awake
    void Awake() {
        m_Animation = GetComponent<Animator>();

        // Player
        CheckPoint = GetComponent<PlayerCheckPoint>();
        Inventory = GetComponent<PlayerInventory>();
        Movement = GetComponent<PlayerMovement>();
        Health = GetComponent<PlayerHealth>();
        Attack = GetComponent<PlayerAttack>();
        Jump = GetComponent<PlayerJump>();

        // Set del player
        uCore.GameManager.SetPlayer(this);
        uCore.Audio.Play2DSFX("24 - Game Start", this.transform.position);
        _IsRunningOrNot = false;
    }

    // Unity Start(){
    void Start() {
        // Update del HUD según los datos del player
        OnHealthChanged();
        OnPickItem(Inventory.Coins());
    }

    // Unity Update
    void Update() {

        // Si estamos muertos, estamos OUT
        if (!Health.IsAlive())
            return;

        m_StandTime += Time.deltaTime;

        Vector3 l_ForwardCamera = m_Camera.transform.forward;
        Vector3 l_RightCamera = m_Camera.transform.right;
        l_ForwardCamera.y = l_RightCamera.y = 0.0f;
        l_ForwardCamera.Normalize(); l_RightCamera.Normalize();

        // CalcMovement
        Vector3 l_Movement = Vector3.zero;
        float l_Speed = 0.0f;
        if (uCore.Action.MoveForward()) {
            l_Movement += l_ForwardCamera;
        }
        if (uCore.Action.MoveBackward()) {
            l_Movement -= l_ForwardCamera;
        }
        if (uCore.Action.MoveRight()) {
            l_Movement += l_RightCamera;
        }
        if (uCore.Action.MoveLeft()) {
            l_Movement -= l_RightCamera;
        }

        // CalcSpeed
        if (l_Movement != Vector3.zero) {
            l_Speed = Movement.WalkSpeed();
            _IsRunningOrNot = false;
            if (uCore.Action.Run()) {
                l_Speed = Movement.RunSpeed();
                _IsRunningOrNot = true;
            }
        }
        if (Health.IsInvencible()) {
            l_Speed = Movement.PowerStarSpeed();
        }

        l_Movement.Normalize();

        // Crounch
        if (!Jump.IsForcedJumping()) {
            Movement.Crounch(uCore.Action.Crounch());
        }

        // Movement
        m_Animation.SetFloat("speed", l_Speed);
        if (Movement.Move(l_Movement, l_Speed, l_Movement)) {
            m_StandTime = 0.0f;
            PlayFootStep();
            PlayFootStepParticles();
        }

        // Jump
        if (uCore.Action.Jump()) {
            if (Movement.Crounching()) {
                Movement.Crounch(false);
                Jump.LongJump();
            } else {
                Jump.Jump();
            }
            m_StandTime = 0.0f;
        }

        m_Animation.SetBool("crounch", Movement.Crounching());
        m_Animation.SetInteger("jumps", Jump.Jumps());
        m_Animation.SetBool("jump", Jump.IsRising());
        m_Animation.SetBool("fall", Jump.IsFalling());
        m_Animation.SetBool("grounded", Jump.IsGrounded());
        m_Animation.SetBool("wall", Jump.OnWall());

        // Punch
        if (uCore.Action.Punch()) {
            if (Attack.CanAttack() && Jump.IsGrounded()) {
                Attack.Punch();
                m_Animation.SetTrigger("attack");
                m_Animation.SetInteger("combo", Attack.Combo());
                m_StandTime = 0.0f;
            }
        }

        // Iddle
        if (m_StandTime >= m_IddleTime) {
            m_Animation.SetTrigger("iddle");
            m_StandTime = 0.0f;
        }
    }

    // Unity LateUpdate
    void LateUpdate() {
        if (m_CurrentElevator != null) {
            Vector3 l_EulerRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0.0f, l_EulerRotation.y, 0.0f);
        }
    }

    // OnControllerColliderHit
    public void OnControllerColliderHit(ControllerColliderHit hit) {
        // Bridge
        if (hit.collider.tag == "Bridge") {
            hit.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(-hit.normal * m_BridgeForce, hit.point);
        }
    }

    // Método que compreuba si pdoemos matar al enemigo
    // In: Vector3 normal -> Normal del enemigo con respsecto am iposición, para comprboar el angulod e salto
    // Out: bool (true -> puedo matarlo | false -> no peudo matarlo)
    private bool CanKill(Vector3 normal) {
        return (Vector3.Dot(normal, Vector3.up) >= Mathf.Cos(m_MaxAngleToKill * Mathf.Deg2Rad));
    }

    // Método "intermedio" para recibir daño
    public void TakeDamage() {
        Health.TakeDamage();
        uCore.Audio.Play2DSFX("take-damage", this.transform.position);
        m_Animation.SetTrigger("hit");
        uCore.Particles.PlayParticlesOnce("HitEnemyWords",this.transform.position);
        Jump.DamageJump();
    }

    // Método Handler para cuando el player muere
    private void OnDie() {
        if (Health.Lifes() <= 0) {
            uCore.FadeFX.FadeIn(LoadNextScene);
        } else {
            uCore.FadeFX.FadeIn(Health.Revive);
        }
        uCore.Audio.Play2DSFX("Mario_die", this.transform.position);
        uCore.Particles.PlayParticlesOnce("NoLife", this.transform.position);
        m_Animation.SetTrigger("die");
    }

    // Método callback para cargar la escena de GameOver
    private void LoadNextScene() {
        uCore.Director.LoadScene(GameScenes.GameOver);
    }

    // Método Handler para cuando el player hace respawn
    private void OnRespawn() {
        uCore.FadeFX.FadeOut();
        CheckPoint.RestoreCheckPoint();

        m_Animation.SetTrigger("respawn");
    }

    // IRestartGameElement
    public void RestartGame() {
        GetComponent<CharacterController>().enabled = false;
        Health.RestoreFull();
        Inventory.ClearInventory();
        this.transform.position = uCore.Director.Scene().StartPosition().position;
        GetComponent<CharacterController>().enabled = true;
    }

    // Elevator Controler
    private void Attach(Collider col) {
        m_CurrentElevator = col;
        this.transform.SetParent(col.transform);
    }

    private bool CanAttach(Collider col) {
        return Vector3.Dot(col.transform.up, Vector3.up) >= m_ElevatorDotAngle && m_CurrentElevator == null;
    }

    private void DeAttach() {
        m_CurrentElevator = null;
        this.transform.SetParent(uCore.GameManager.ActorContainer());
    }

    //Método para reproducir la pisada
    private void PlayFootStep()
    {
        if (!Jump.IsGrounded())
            return;

        m_StepDelay += Time.deltaTime;
        if (m_StepDelay >= m_StepDelayLimit)
        {
            uCore.Audio.Play2DSFX("step-floor", this.transform);
            m_StepDelay = 0.0f;
        }
    }

    //Método para reproducir particulas de las pisadas
    private void PlayFootStepParticles()
    {
        if(_IsRunningOrNot)
        {
            if (!Jump.IsGrounded())
                return;

            m_StepDelayParticle += Time.deltaTime;
            if (m_StepDelayParticle >= m_StepDelayLimitParticle)
            {
                uCore.Particles.PlayParticlesOnce("Run_Smoke", m_EmitterParticle.position);
                m_StepDelayParticle = 0.0f;
            }
        }      
    }


    // * --------------------------------------------------------------------------------- *
    // | - COLLISION --------------------------------------------------------------------- |
    // V --------------------------------------------------------------------------------- V

    // OnTriggerEnter
    void OnTriggerEnter(Collider other) {
        // Item
        if (other.tag == "Item") {
            other.GetComponent<Item>().Pick(this);
        }
        // DeadZone
        else if (other.tag == "DeadZone") {
            Health.Kill();
            uCore.Particles.PlayParticlesOnce("DeadByFire",this.transform.position);
        }
        // Elevator
        else if (other.tag == "Elevator") {
            if (CanAttach(other)) {
                Attach(other);
            }
        }
        // Wall
        else if (other.tag == "Wall") {
            Jump.AttachWall(other.gameObject);
        }
        // Enemy
        else if (other.tag == "Enemy") {
            Vector3 normal = this.transform.position - other.transform.position;
            if (CanKill(normal.normalized)) {
                if (other.gameObject.GetComponent<Goomba>() != null) {
                    other.gameObject.GetComponent<Goomba>().Kill();
                } else if (other.gameObject.GetComponent<Enemy>() != null) {
                    other.gameObject.GetComponent<Enemy>().GotHit();
                }
                Jump.ForceJump();
            }
        }
    }

    // OnTriggerStay
    void OnTriggerStay(Collider other) {
        // DeadZone
        if (other.tag == "DeadZone") {
            Health.Kill();
        }
        // Elevator
        else if (other.tag == "Elevator") {
            if (CanAttach(other)) {
                Attach(other);
            }
        }
        // Wall
        else if (other.tag == "Wall") {
            Jump.CheckAttach(other);
        }
    }

    // OnTriggerExit
    void OnTriggerExit(Collider other) {
        // Elevator
        if (other.tag == "Elevator") {
            if (other == m_CurrentElevator) {
                DeAttach();
            }
        }
        // Wall
        else if (other.tag == "Wall") {
            Jump.DeAttachWall();
        }
    }

    // A --------------------------------------------------------------------------------- A

    // * --------------------------------------------------------------------------------- *
    // | - HUD - UPDATES ----------------------------------------------------------------- |
    // V --------------------------------------------------------------------------------- V

    public void OnHealthChanged() {
        uCore.HUD.UpdateHealth(Health.Lifes(), Health.CurrentHealth());
    }

    public void OnPickItem(GameItem item) {
        switch (item.ID) {
            case ItemID.Coin:
                uCore.HUD.UpdateCoins(item.Amount);
                break;
            default: break;
        }
    }

    // A --------------------------------------------------------------------------------- A
}
