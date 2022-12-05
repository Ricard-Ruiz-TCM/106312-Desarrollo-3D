using UnityEngine;

public class CameraController : MonoBehaviour {

    // Target que vamos a "seguir"
    [SerializeField, Header("Target:")]
    private Transform m_Target;

    // Yaw
    [SerializeField, Header("Yaw:")]
    private float m_Yaw;
    [SerializeField]
    private float m_YawSpeed;
    [SerializeField]
    private bool m_YawInverted;
    public void ReverseYaw() { m_YawInverted = !m_YawInverted; }

    // Pitch
    [SerializeField, Header("Pitch:")]
    private float m_Pitch;
    [SerializeField]
    private float m_PitchSpeed;
    [SerializeField]
    private bool m_PitchInverted;
    [SerializeField]
    private float m_MaxPitch, m_MinPitch;
    public void ReversePitch() { m_PitchInverted = !m_PitchInverted; }

    // Avoid Collisión and move throw
    [SerializeField, Header("Collisión:")]
    private LayerMask m_Layers;
    [SerializeField]
    private float m_MinDistance;
    [SerializeField]
    private float m_MaxDistance;
    [SerializeField]
    private float m_PosOffset;

    private Vector3 m_CameraReCol;

    private float m_Time;

    // Unity LateUpdate
    void LateUpdate() {
        // Input
        Vector2 l_Mouse = uCore.Action.CameraMovement();
        // Lock del cursor??
        if (uCore.GameManager.IsCursorLocked()) {
            l_Mouse = Vector2.zero;
        }

        if (l_Mouse == Vector2.zero){
            m_Time += Time.deltaTime;
        } else {
            m_Time = 0.0f;
        }

        // Rotamos el player conforme la camara
        transform.LookAt(m_Target.transform.position);
        Vector3 l_EulerAngles = transform.rotation.eulerAngles;
        m_Yaw = l_EulerAngles.y;

        // Calc de distancia
        float l_Distance = Vector3.Distance(this.transform.position, m_Target.position);
        l_Distance = Mathf.Clamp(l_Distance, m_MinDistance, m_MaxDistance);

        // Calc de Yaw & Pitch
        m_Yaw += l_Mouse.x * m_YawSpeed * Time.deltaTime * (m_YawInverted ? -1.0f : 1.0f); ;
        m_Pitch += l_Mouse.y * m_PitchSpeed * Time.deltaTime * (m_PitchInverted ? -1.0f : 1.0f); ;
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);

        Vector3 l_ForwardCamera = new Vector3(Mathf.Sin(m_Yaw * Mathf.Deg2Rad) * Mathf.Cos(m_Pitch * Mathf.Deg2Rad),
            Mathf.Sin(m_Pitch * Mathf.Deg2Rad), Mathf.Cos(m_Yaw * Mathf.Deg2Rad) * Mathf.Cos(m_Pitch * Mathf.Deg2Rad));

        if (m_Time >= 5) {
            m_CameraReCol = m_Target.transform.forward;
        } else {
            m_CameraReCol = l_ForwardCamera;
        }

        l_ForwardCamera = Vector3.Lerp(l_ForwardCamera, m_CameraReCol, 0.002f);

        Vector3 l_DesiredPosition = m_Target.position - l_ForwardCamera * l_Distance;
        
        // CAmera máx distance
        RaycastHit l_RaycastHit;
        Ray l_Ray = new Ray(m_Target.transform.position, -l_ForwardCamera);
        if (Physics.Raycast(l_Ray, out l_RaycastHit, l_Distance, m_Layers)) {
            l_DesiredPosition = l_RaycastHit.point - l_ForwardCamera * m_PosOffset;
        }



        transform.position = l_DesiredPosition;
        transform.LookAt(m_Target.position);
    }
}
