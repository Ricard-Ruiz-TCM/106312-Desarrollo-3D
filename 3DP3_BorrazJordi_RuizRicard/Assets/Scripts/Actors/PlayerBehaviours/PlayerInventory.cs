using System;
using UnityEngine;
using System.Collections.Generic;

// Enum para determinar todos los items del juego
public enum ItemID {
    Coin
}

// Struct que define como es un item
[Serializable]
public struct GameItem {

    public string Name;
    public ItemID ID;
    public int Amount;
    public string Desc;

    public GameItem(string name, ItemID id, int amount = 0, string desc = "") {
        this.Name = name; this.ID = id; this.Amount = amount; this.Desc = desc;
    }

    public void Add(int amount = 1) {
        Amount += amount;
    }

    public void Clear() {
        Amount = 0;
    }

}

public class PlayerInventory : MonoBehaviour {

    // Observer para avisar que hemos cogido un item 
    public static event Action<GameItem> OnPickItem;

    [SerializeField]
    private GameItem m_Coins;
    public GameItem Coins() { return m_Coins; }

    // Método para coger un item
    // In GameItemsType itemType -> objeto que estamos cogiendo
    public void PickItem(ItemID itemType) {
        switch (itemType) {
            case ItemID.Coin:
                PickCoin();
                break;
            default: break;
        }
    }

    // Método para sumar un Coin a nuestro invetnario
    private void PickCoin() {
        m_Coins.Add(1);
        OnPickItem?.Invoke(m_Coins);
        uCore.Audio.Play2DSFX("coin", this.transform.position);
    }

    // Método para reiniciar todos los elementos del inventario
    public void ClearInventory() {
        m_Coins.Clear();
    }

}
