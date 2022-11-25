using UnityEngine;

public abstract class BasicState : MonoBehaviour {

    // Método abstracto para el enter, update y exit al estado
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();

}

