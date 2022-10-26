using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyRing : MonoBehaviour {

    // Observr para el evento de utilizar una llave
    public static event Action OnKeyRing;

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
        if (HaveKey(id) != -1)
            return;
        m_Keys.Add(id);
        OnKeyRing?.Invoke();
    }

    // Método para limpiar las keys que tenemos
    public void Clear() {
        m_Keys.Clear();
    }

    // Check si hay llave
    // In: int id -> ID de la llave
    // Out: int -> posición de la llave si la encuentra o (-1) si no la encuentar
    public int HaveKey(int id) {
        int pos = 0;
        foreach (int i in m_Keys) {
            if (i == id)
                return pos;
            pos++;
        }
        return -1;
    }

    // Utiliza una llave, borrandola de la lista
    // In: int id -> ID de la llave
    // Out: bool -> (true -> utiliza la llave | false -> no utiliza la llave)
    public bool RemoveKey(int id) {
        int pos = HaveKey(id);
        if (pos != -1) {
            m_Keys.RemoveAt(pos);
            OnKeyRing?.Invoke();
            return true;
        }
        return false;
    }
}
