using System.Collections.Generic;
using UnityEngine;

public class BasicPortal : MonoBehaviour {

    [SerializeField, Header("Puntos válidos:")]
    protected Transform m_ValidPoints;
    protected List<Transform> m_ValidPointsList;

    [SerializeField, Header("Layer:")]
    protected LayerMask m_WallLayer;

    [SerializeField, Header("Control de calidad:")]
    protected float m_MinValidDistance;
    [SerializeField]
    protected float m_MaxValidDistance;
    [SerializeField]
    protected float m_MinDotValidAngle;
    [SerializeField]
    protected float m_PLAYERSHOOT_ShootDistance;

    [SerializeField, Header("Color String:")]
    protected string m_PortalColorName;

    [SerializeField, Header("Scale:")]
    private float m_ScaleIncrement = 2.0f;
    [SerializeField]
    private float m_MaxScale = 2.0f;
    [SerializeField]
    private float m_MinScale = 0.5f;

    private float m_Scale = 1.0f;
    public float CurrentScale() { return m_Scale; }

    [SerializeField, Header("Deacivation system:")]
    private MeshRenderer m_Cylinder;
    [SerializeField]
    private Material m_Material;
    [SerializeField]
    private Material m_BlackMat;

    // Unity Awake
    void Awake() {
        LoadPoints();
    }

    // Método para cargar todos los puntos de ValidPoints
    protected void LoadPoints() {
        m_ValidPointsList = new List<Transform>();
        foreach (Transform t in m_ValidPoints) {
            m_ValidPointsList.Add(t);
        }
    }

    // Método para intentar colocar un portal
    // In: Vector3 newPos -> Nueva posición donde colocarlo
    // In: Vector3 newDir -> Nueva dirección de rotación
    // Out: bool -> (true -> se ha colocado | false -> no se ha colocado)
    public bool TryPlace(Vector3 newPos, Vector3 newDir) {
        RaycastHit l_RaycastHit;

        // Comprobamos en corto si podemos ponerlo
        if (!Raycast2Place(newPos, newDir, m_PLAYERSHOOT_ShootDistance, m_WallLayer, out l_RaycastHit))
            return false;

        transform.position = l_RaycastHit.point;
        transform.rotation = Quaternion.LookRotation(l_RaycastHit.normal);

        return R_CheckPortalPoint(0);
    }

    // Método que establece la es cala del objeto
    // In: float scale -> escala
    public void SetScale(float scale) {
        m_Scale = scale;
        this.transform.localScale = new Vector3(scale, scale, scale);
    }

    // Método para re-escalar el portal dummy mientras estamos ghosteando
    // In: float factor -> factor de escalada
    public void Resize(float factor) {
        float l_Scale = m_Scale;
        // Calc de incremento/decremento de escala
        l_Scale += m_ScaleIncrement * factor * Time.deltaTime;
        l_Scale = Mathf.Clamp(l_Scale, m_MinScale, m_MaxScale);
        // Set
        SetScale(l_Scale);
    }

    // Método recursivo para comprobar cada punto del portal
    // In: int index -> posición de la lista de m_ValidPointsList
    // Out: bool -> (true -> punto válido | false -> punto no válido)
    private bool R_CheckPortalPoint(int index) {
        // Si hemos checkeado todos los puntos, significa que estamos bien posicionados
        if (index == m_ValidPointsList.Count)
            return true;

        // Get del punto a analizar y raycast
        RaycastHit l_RaycastHit;
        Transform l_PointTransform = m_ValidPointsList[index];

        // Si no hay colision, estamos OUT
        if (!Raycast2Place(l_PointTransform.position, -l_PointTransform.forward, m_PLAYERSHOOT_ShootDistance, m_WallLayer, out l_RaycastHit))
            return false;

        // Si no es un "ValidWall", estamos OUT
        if (l_RaycastHit.collider.tag != "ValidWall")
            return false;

        // Calculamos distancia y ángulo
        float l_Distance = Vector3.Distance(l_PointTransform.position, l_RaycastHit.point);
        float l_DotAngle = Vector3.Dot(l_PointTransform.forward, l_RaycastHit.normal);
        // Si no nos gustan para poenr el portal, estamos OUT
        if (!(l_Distance >= m_MinValidDistance && l_Distance <= m_MaxValidDistance && l_DotAngle > m_MinDotValidAngle))
            return false;

        // Comprobamos el siguiente punto
        return R_CheckPortalPoint(index + 1);
    }

    // Método para extraer el raycast y utilizarlo más veces ;3
    // In: Vector3 position -> Posición origen del raycast
    // In: Vector3 direction -> Dirección del raycast
    // In: float distance -> Distancia del raycast
    // In: LayerMask layer -> Layer donde calculamos físicas
    // Out: Bool -> raycast valido contra algo
    // Out: RaycastHit -> Objeto con el que colisiona, PUEDE SER NULL, CUIDADO
    protected bool Raycast2Place(Vector3 position, Vector3 direction, float distance, LayerMask layer, out RaycastHit raycastHit) {
        Ray l_Ray = new Ray(position, direction);
        Debug.DrawRay(position, direction);
        Physics.Raycast(l_Ray, out raycastHit, distance, layer);
        return (raycastHit.collider != null);
    }

    // Método para comprboar el colro dle portal
    // Out: string -> Color del portal
    public string PortalColor() {
        return m_PortalColorName;
    }

    public void Deactivate() {
        m_Cylinder.material = m_BlackMat;
    }

    public void ResetMat() {
        m_Cylinder.material = m_Material;
    }

}
