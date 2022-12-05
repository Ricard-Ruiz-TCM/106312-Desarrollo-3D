using UnityEngine;

public abstract class BasicScene : MonoBehaviour {

    private string m_Name;
    public string Name() { return m_Name; }
    public void SetSceneName(string name) { m_Name = name; }

    [SerializeField, Header("StartPoint:")]
    private Transform m_StartPoint;
    public Transform StartPosition() { return m_StartPoint; }


}
