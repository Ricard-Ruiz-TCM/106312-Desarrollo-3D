using UnityEngine;

public abstract class Item : MonoBehaviour, IRestartGameElement {

    // Método abstracto Pick para que pickear items
    // In: Player player -> el jugador principal
    public abstract void Pick(Player player);

    // IRestartGameElement
    public void RestartGame() {
        this.gameObject.SetActive(true);
        this.transform.parent.gameObject.SetActive(true);
    }

    // Desactiva el objeto
    protected void Deactivate() {
        this.gameObject.SetActive(false);
    }

    // Desactiva el padre
    protected void DeactivateParent() {
        this.transform.parent.gameObject.SetActive(false);
    }

    // Destruye el objeto
    protected void Destroy() {
        GameObject.Destroy(this.gameObject);
    }

    // Destruye el padre
    protected void DestroyParent() {
        GameObject.Destroy(this.transform.parent.gameObject);
    }

}