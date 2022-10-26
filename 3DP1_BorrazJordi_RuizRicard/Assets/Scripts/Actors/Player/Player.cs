using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerJump)), RequireComponent(typeof(PlayerVitals)), RequireComponent(typeof(PlayerMovement)), RequireComponent(typeof(PlayerShooting)), RequireComponent(typeof(PlayerKeyRing))]
public class Player : MonoBehaviour {

    // Player Data
    [SerializeField, Header("Player Data:")]
    private PlayerData m_Data;
    public PlayerData Data() { return m_Data; }

    [SerializeField]
    private PlayerTemporalData m_TemporalData;

    // Player Components
    private PlayerJump m_Jump;
    public PlayerJump Jump() { return m_Jump; }
    private PlayerVitals m_Vitals;
    public PlayerVitals Vitals() { return m_Vitals; }
    private PlayerMovement m_Movement;
    public PlayerMovement Movement() { return m_Movement; }
    private PlayerShooting m_Shooting;
    public PlayerShooting Shooting() { return m_Shooting; }
    private PlayerKeyRing m_KeyRing;
    public PlayerKeyRing KeyRing() { return m_KeyRing; }

    // Movimiento
    private bool m_Moving;
    public bool Moving() { return m_Moving; }

    // Animation
    private Animation m_Animation;
    [SerializeField, Header("Animation:")]
    private List<AnimationClip> m_Clip;
    // Enum con los IDS de posición para m_Clip
    private enum AnimClips {
        IDDLE, SHOOT, RELOAD, JUMP, START_AIM, END_AIM,
        BASE_ANIM = AnimClips.IDDLE,
        TOTAL_ANIMS
    }
    [SerializeField]
    public float m_CrossFadeTime = 0.2f;

    // Dungeon Elements
    private Door m_Door;

    // Glería de tiro
    private SGallery m_Gallery;

    // Interface Components
    [SerializeField, Header("HUD Controller:")]
    private PlayerHUD m_HUD;

    // Footsteps
    [SerializeField, Header("Pisadas:")]
    private float m_StepDelayLimit;
    private float m_StepDelay;
    public string escene;
    // Unity OnEnable
    void OnEnable() {
        // Shooting
        PlayerShooting.OnAmmoChanged += UpdateAmmunition;
        PlayerShooting.OnWeaponChange += UpdateWeaponIcon;
        // Vitals
        PlayerVitals.OnVitalsChanged += () => { UpdateHealth(); UpdateShield(); };
        Health.OnDie += Die;
        // KeyRing
        PlayerKeyRing.OnKeyRing += UpdateInventory;
        // Respawn
        RespawnButton.OnPlayerRespawn += Respawn;
        // Score
        SGallery.OnGalleryCompleted += (int s) => { StartCoroutine(DisableScore()); };
    }

    // Unity OnDisable
    void OnDisable() {
        // Shooting
        PlayerShooting.OnAmmoChanged -= UpdateAmmunition;
        PlayerShooting.OnWeaponChange -= UpdateWeaponIcon;
        // Vitals
        PlayerVitals.OnVitalsChanged -= () => { UpdateHealth(); UpdateShield(); };
        Health.OnDie += Die;
        // KeyRing
        PlayerKeyRing.OnKeyRing -= UpdateInventory;
        // Respawn
        RespawnButton.OnPlayerRespawn -= Respawn;
        // Score
        SGallery.OnGalleryCompleted -= (int s) => { StartCoroutine(DisableScore()); };
    }

    // Unity Awake
    void Awake() {
        // Player Componentes
        m_Jump = GetComponent<PlayerJump>();
        m_Vitals = GetComponent<PlayerVitals>();
        m_Movement = GetComponent<PlayerMovement>();
        m_Shooting = GetComponent<PlayerShooting>();
        m_KeyRing = GetComponent<PlayerKeyRing>();
        // Animation
        m_Animation = GetComponent<Animation>();
        // HUD Controller
        m_HUD = GameObject.FindObjectOfType<PlayerHUD>();

        // Temporal Data
        m_TemporalData = Resources.Load<PlayerTemporalData>("ScriptableObjects/TemporalPlayer");
    }

    // Unity Start(){
    void Start() {
        // SetData de los componentes de "Player"
        m_Jump.SetData(m_Data);
        m_Vitals.SetData(m_Data);
        m_Movement.SetData(m_Data);
        m_Shooting.SetData(m_Data);

        m_Animation.CrossFade(m_Clip[(int)AnimClips.BASE_ANIM].name, m_CrossFadeTime);

        // Cargamos info temporal, entre escenas y así
        if (uCore.Director.Scene().Name() != GameScenes.GSCENE_Outside)
            LoadTemporalData();

        // Seteamos un CheckPoint con informaicón inicial
        CheckPoint();

        // Actualizamos HUD
        UpdateHUD();

    }

    // Unity Update
    void Update() {
        m_Moving = false;

        // Check si estoy vivo
        if (m_Vitals.PlayerHealth().IsDead())
            return;

        // Movement Calcs
        Vector3 l_Direction = Vector3.zero;
        float l_speed = m_Movement.Speed();
        if (uCore.Input.Forward())
            l_Direction = transform.forward;
        if (uCore.Input.Backward())
            l_Direction = -transform.forward;
        if (uCore.Input.Right())
            l_Direction += transform.right;
        if (uCore.Input.Left())
            l_Direction -= transform.right;
        if (uCore.Input.LShift())
            l_speed *= m_Movement.SpeedMultiplier();

        if (l_Direction != Vector3.zero)
            m_Moving = true;

        // Movement
        if (m_Movement.Move(l_Direction.normalized, l_speed, uCore.Input.MouseMovement()))
            PlayFootStep();

        // Jump
        if (uCore.Input.Jump())
            if (m_Jump.Jump()) {
                ChangeAnimation(m_Clip[(int)AnimClips.JUMP].name);
            }

        // Shoot
        if (uCore.Input.Shoot())
            if (m_Shooting.Shoot())
                ChangeAnimation(m_Clip[(int)AnimClips.SHOOT].name);

        // Reload
        if (uCore.Input.Reload())
            if (m_Shooting.Reload())
                ChangeAnimation(m_Clip[(int)AnimClips.RELOAD].name);

        // Aim
        if (uCore.Input.Aim()) {
            // TODO // Apuntar
        }

        // Interact
        if (uCore.Input.Interact()) {
            // Door
            if (m_Door != null) {
                if (m_KeyRing.RemoveKey(m_Door.DoorID()))
                    m_Door.UnlockDoor();
            }

            // SGallery
            if (m_Gallery != null) {
                m_Gallery.Interact();
                EnableScore();
            }
        }

    }

    // Cambia a la animación según la string
    // In: string name -> nombre de la anim
    private void ChangeAnimation(string name) {
        if (m_Animation.clip.name == name)
            return;

        m_Animation.CrossFade(name, m_CrossFadeTime);
        m_Animation.CrossFadeQueued(m_Clip[(int)AnimClips.BASE_ANIM].name, m_CrossFadeTime);
    }

    // Método para guardar la finromación temporal :O
    public void SaveData() {
        m_TemporalData.ValidInfo = true;
        // Transform
        m_TemporalData.Position = this.transform.position;
        m_TemporalData.Rotation = this.transform.rotation;
        // Vitals
        m_TemporalData.Health = m_Vitals.PlayerHealth().CurrentHealth();
        m_TemporalData.Shield = m_Vitals.Playershield().CurrentShield();
        // Shooting
        m_TemporalData.Weapon = m_Shooting.WeaponData();
        m_TemporalData.Ammunition = m_Shooting.CurrentWeapon().Ammo();
        m_TemporalData.TotalAmmunition = m_Shooting.CurrentWeapon().TotalAmmo();
        // Keys
        foreach (int key in m_KeyRing.Keys())
            m_TemporalData.Keys.Add(key);
    }

    // Método para cargar la finromación temporal :o
    public void LoadTemporalData() {
        if (m_TemporalData.ValidInfo) {
            // Vitals
            m_Vitals.PlayerHealth().SetHealt(m_TemporalData.Health);
            m_Vitals.Playershield().SetShield(m_TemporalData.Shield);
            // Shooting
            m_Shooting.LoadWeapon(m_TemporalData.Weapon);
            m_Shooting.SetAmmo(m_TemporalData.Ammunition, m_TemporalData.TotalAmmunition);
            m_KeyRing.Clear();
            // Keys
            foreach (int k in m_TemporalData.Keys)
                m_KeyRing.AddKey(k);
        }
    }

    // Método handler para la muerte y respawn
    private void Die(GameObject obj) {
        if (obj.GetComponent<Player>() == null)
            return;
        // Hacemos un FadeIn con el método respawn de callback
        uCore.FadeFX.FadeIn(EnableRespawn);
        uCore.FadeFX.SetMaxFade(0.5f);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Método respawn, la última información temporal que tenemos y vamos tirando
    public void Respawn() {
        Time.timeScale = 1.0f;
        // Invencibildiad del player urante unos segundos
        m_Vitals.PlayerHealth().MakeInvencible();
        // Cargamos data, update del HUD y fadeOut
        LoadTemporalData();
        // Transform
        CharacterController cc = GetComponent<CharacterController>();
        cc.enabled = false;
        this.transform.position = m_TemporalData.Position;
        this.transform.rotation = m_TemporalData.Rotation;
        cc.enabled = true;
        UpdateHUD();
        uCore.FadeFX.FadeOut();
    }

    // Guardamos la info en la temporal data
    public void CheckPoint() {
        SaveData();
    }

    // Método para repdocudir una pisada
    private void PlayFootStep() {
        if (!m_Jump.IsGrounded())
            return;

        m_StepDelay += Time.deltaTime;
        if (m_StepDelay >= m_StepDelayLimit) {
            uCore.Audio.PlaySFX(escene + ((int)Random.Range(1, 6)).ToString(), this.transform);
            m_StepDelay = 0.0f;
        }
    }

    // Método para añadir puntos al score y tocar el HUD,
    // QUIZÁ ROMPE UN POCO EL SOLID, PERO LO METO AQUÍ YA 
    // LO CONSIDERO ELEMENTO DEL PLAYER HUD, HUD QUE TOCO DESDE EL PLAYER
    // :(
    // In: int amount -> Cantidad de puntos
    public void SetPoints(int amount, int total) {
        AddPoints2Hud(amount, total);
    }

    // * --------------------------------------------------------------------------------- *
    // | - COLLISION --------------------------------------------------------------------- |
    // V --------------------------------------------------------------------------------- V

    private void OnTriggerEnter(Collider other) {
        // Item Collision
        if (other.tag == "Item")
            other.GetComponent<Item>().Pick(this);

        // Door Collision
        else if (other.tag == "Door") {
            m_Door = other.GetComponent<Door>();
        }

        // DeadZones Collision
        else if (other.tag == "DeadZone") {
            m_Vitals.TakeDamage(9999.99f);
        }

        // Elevator Collision
        else if (other.tag == "Elevator") {
            other.GetComponent<Elevator>().Run();
            this.transform.SetParent(other.transform);
        }

        // SGallery Collision
        else if (other.tag == "SGallery") {
            m_Gallery = other.GetComponent<SGallery>();
        }
    }

    private void OnTriggerExit(Collider other) {
        // Door DesCollision
        if (other.tag == "Door") {
            m_Door.Close();
        }

        // Elevator Collision
        else if (other.tag == "Elevator") {
            this.transform.SetParent(uCore.GameManager.ActorContainer());
        }
    }

    // A --------------------------------------------------------------------------------- A

    // * --------------------------------------------------------------------------------- *
    // | - HUD - UPDATES ----------------------------------------------------------------- |
    // V --------------------------------------------------------------------------------- V

    // Método para activar las opciones de respawn
    public void EnableRespawn() {
        m_HUD.EnableRespawn();
        Time.timeScale = 0.0f;
    }

    // Método update para el HUD completo
    public void UpdateHUD() {
        UpdateHealth();
        UpdateShield();
        UpdateAmmunition();
        UpdateWeaponIcon();
    }

    // Método update del HUD para vida
    public void UpdateHealth() {
        m_HUD.UpdateHealthBar(m_Vitals.PlayerHealth().CurrentHealth());
    }

    // Método update del HUD para escudo
    public void UpdateShield() {
        m_HUD.UpdateShieldBar(m_Vitals.Playershield().CurrentShield());
    }

    // Método update del HUD para munición
    public void UpdateAmmunition() {
        m_HUD.UpdateAmmunitionText(m_Shooting.CurrentWeapon().Ammo(), m_Shooting.CurrentWeapon().TotalAmmo());
    }

    // Método update del HUD para icono de arma
    public void UpdateWeaponIcon() {
        m_HUD.ChangeWeaponIcon(m_Shooting.CurrentWeapon().Icon());
    }

    // Método update del HUD para el inventario
    public void UpdateInventory() {
        m_HUD.UpdateKeys(m_KeyRing.Keys().ToArray());
    }

    // Método para añadir score al HUD
    // In: int amount -> Cantidad de puntos
    // In: int total -> total de puntos
    public void AddPoints2Hud(int amount, int total) {
        m_HUD.AddPoints(amount, total);
    }

    public IEnumerator DisableScore() {
        yield return new WaitForSeconds(5);
        if (!GameObject.FindObjectOfType<SGallery>().Active()) {
            m_HUD.DisableScore();
        }
    }

    public void EnableScore() {
        m_HUD.EnableScore();
    }

    // A --------------------------------------------------------------------------------- A

}
