using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

    // Observer para avisar que hemos pickeado un item
    // Bool incida si tenemos llave o no
    public static event Action<bool> OnKeyring;

    [SerializeField, Header("Keys List:")]
    private List<int> m_Keys;
    public List<int> Keys() { return m_Keys; }

    // Unity Start
    void Start() {
        m_Keys = new List<int>();
    }

    // Añade una llave al inventario
    // In: int id -> ID de la llave
    public void AddKey(int id) {
        if (HaveKey(id))
            return;

        m_Keys.Add(id);
        OnKeyring?.Invoke(m_Keys.Count != 0);
    }

    // Check si hay llave
    // In: int id -> ID de la llave
    // Out: bool -> posición de la llave si la encuentra o (-1) si no la encuentar
    public bool HaveKey(int id) {
        return (FindKey(id) != -1);
    }

    // Método para buscar una llave dentro del sistema
    // In: int id -> ID de la llave
    // Out: int -> posición de la llave en la lista
    private int FindKey(int id) {
        int l_pos = 0;
        foreach (int i in m_Keys) {
            if (i == id)
                return l_pos;
            l_pos++;
        }
        return -1;
    }

    // Método para borrar una llave del llavero
    // In: int id -> ID de la llave
    public void RemoveKey(int id) {
        if (HaveKey(id))
            m_Keys.RemoveAt(FindKey(id));

        OnKeyring?.Invoke(m_Keys.Count != 0);
    }

}
