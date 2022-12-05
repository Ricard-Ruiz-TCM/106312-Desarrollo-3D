using UnityEngine;

public class EnemyTurtle : Enemy {

    [SerializeField, Header("Shell")]
    private GameObject m_Shell;

    // Unity Awake
    void Awake() {
        Load();
    }

    // Unity Start
    void Start() {
        StarMachine();
    }

    // Unity Update
    void Update() {
        UpdateStates();
    }

    // Override del GotHit
    public override void GotHit() {
        SpawnShell();
        uCore.Particles.PlayParticlesOnce("Enemy_Dead", this.transform.position);
        GameObject.Destroy(this.gameObject);
    }

    // Método para spawnear la shell de la tortuga
    private void SpawnShell() {
        m_Shell = GameObject.Instantiate(m_Shell, this.transform);
        m_Shell.transform.SetParent(uCore.GameManager.EntityContainer().transform);
    }

}
