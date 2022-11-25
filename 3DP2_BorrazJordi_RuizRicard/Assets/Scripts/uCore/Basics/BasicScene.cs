using UnityEngine;

public abstract class BasicScene : MonoBehaviour {

    private string m_Name;
    public string Name() { return m_Name; }
    public void SetSceneName(string name) { m_Name = name; }

    [SerializeField, Header("StartPoint:")]
    private Transform m_StartPoint;

    // M�todo para recuperar el StartPoint del nivel
    // Out: Vector3 -> posici�n dentro de la jerarqu�a
    public Vector3 StartPoint() {
        return m_StartPoint.position;
    }


}
