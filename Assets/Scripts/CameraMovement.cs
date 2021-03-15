using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float m_Speed;
    private const float SCROLL_SCALE = -5;
    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float scroll = Input.mouseScrollDelta.y;
        
        Vector3 delta = new Vector3(horizontal, scroll*SCROLL_SCALE, vertical) * (m_Speed * Time.deltaTime);
        transform.Translate(delta, Space.World);

    }
}