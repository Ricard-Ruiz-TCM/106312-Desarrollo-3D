using UnityEngine;

public class Spawner : MonoBehaviour, IButtonAction {

    [SerializeField]
    private GameObject m_Prefab;
    [SerializeField]
    private GameObject m_Point;

    // M�todos de IButtonAction
    public void OnPressed() {
        SpawnCompanionCube();
    }

    // M�todos de IButtonAction
    public void OnReleased() { }

    // M�todo para spawnear un CompanionCube en mi posici�n
    public void SpawnCompanionCube() {
        GameObject g = Instantiate(m_Prefab);
        g.transform.SetParent(uCore.GameManager.EntityContainer());
        g.transform.position = m_Point.transform.position;
    }

}
