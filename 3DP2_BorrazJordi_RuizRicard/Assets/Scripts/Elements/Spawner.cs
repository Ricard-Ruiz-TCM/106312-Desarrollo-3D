using UnityEngine;

public class Spawner : MonoBehaviour, IButtonAction {

    [SerializeField]
    private GameObject m_Prefab;
    [SerializeField]
    private GameObject m_Point;

    // Métodos de IButtonAction
    public void OnPressed() {
        SpawnCompanionCube();
    }

    // Métodos de IButtonAction
    public void OnReleased() { }

    // Método para spawnear un CompanionCube en mi posición
    public void SpawnCompanionCube() {
        GameObject g = Instantiate(m_Prefab);
        g.transform.SetParent(uCore.GameManager.EntityContainer());
        g.transform.position = m_Point.transform.position;
    }

}
