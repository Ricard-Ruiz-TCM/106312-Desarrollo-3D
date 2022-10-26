using UnityEngine;

public class LevelItem : Item {

    [SerializeField, Header("Extra:")]
    private string m_SceneName;

    // Override del Pick
    public override void Pick(Player player) {
        uCore.Director.LoadSceneFaded(m_SceneName);
        Deactivate();
    }
}
