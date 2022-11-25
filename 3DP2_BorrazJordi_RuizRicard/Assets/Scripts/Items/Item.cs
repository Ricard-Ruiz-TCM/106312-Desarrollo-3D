using UnityEngine;

public abstract class Item : MonoBehaviour {

    // M�todo abstracto Pick para que pickear items
    // In: Player player -> el jugador principal
    public abstract void Pick(Player player);

    // Desactiva el objeto
    protected void Deactivate() {
        this.gameObject.SetActive(false);
    }

}

