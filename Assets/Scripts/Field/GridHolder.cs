using UnityEngine;

namespace Field
{
    public class GridHolder: MonoBehaviour
    {
        [SerializeField]
        private int m_GridWidth;
        [SerializeField]
        private int m_GridHeight;

        [SerializeField]
        private Vector2Int m_StartCoordinate;

        [SerializeField]
        private Vector2Int m_TargetCoordinate;

        [SerializeField]
        private float m_NodeSize;
        
        private Grid m_Grid;

        private Camera m_Camera;

        private Vector3 m_Offset;

        public Vector2Int StartCoordinate => m_StartCoordinate;

        public Grid Grid => m_Grid;

        private void Awake()
        {
            m_Camera = Camera.main;
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            transform.localScale = new Vector3(width * 0.1f, 1f, height * 0.1f);

            m_Offset = transform.position - (new Vector3(width, 0f, height) * 0.5f);
            m_Grid = new Grid(m_GridWidth, m_GridHeight, m_Offset, m_NodeSize, m_TargetCoordinate, m_StartCoordinate);
        }

        private void OnValidate()
        {
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            transform.localScale = new Vector3(width * 0.1f, 1f, height * 0.1f);

            m_Offset = transform.position - (new Vector3(width, 0f, height) * 0.5f);
            m_Grid = new Grid(m_GridWidth, m_GridHeight, m_Offset, m_NodeSize, m_TargetCoordinate, m_StartCoordinate);
        }

        private void Update()
        {
            if (m_Grid == null || m_Camera == null)
            {
                return;
            }

            Vector3 mousePosition = Input.mousePosition;

            Ray ray = m_Camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))

            {
                if (hit.transform != transform)
                {
                    return;
                }

                Vector3 hitPosition = hit.point;
                Vector3 difference = hitPosition - m_Offset;

                int x = (int)(difference.x / m_NodeSize);
                int y = (int)(difference.z / m_NodeSize);

                if (Input.GetMouseButtonDown(0))
                {
                    m_Grid.TryOccupyNode(new Vector2Int(x, y));
                }
            }
        }

        private void OnDrawGizmos() //debug method
        {
            if (m_Grid == null)
            {
                return;
            }

            
            foreach (Node node in m_Grid.EnumerateAllNodes())
            {
                if (node.NextNode == null)
                {
                    continue;
                }
                
                if (node.IsOccupied)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(node.Position, 0.2f*m_NodeSize);
                    continue;
                }
                if (node.OccupationAvailability == OccupationAvailability.CanOccupy)
                {
                    Gizmos.color = Color.green;
                }
                if (node.OccupationAvailability == OccupationAvailability.Undefined)
                {
                    Gizmos.color = Color.yellow;
                }
                if (node.OccupationAvailability == OccupationAvailability.CanNotOccupy)
                {
                    Gizmos.color = Color.red;
                }
                Vector3 start = node.Position;
                Vector3 end = node.NextNode.Position;

                Vector3 direction = end - start;

                start -= direction * 0.25f;
                end -= direction * 0.75f;
                
                Gizmos.DrawLine(start, end);
                Gizmos.DrawSphere(end, 0.1f * m_NodeSize);
            }
        }
    }
}