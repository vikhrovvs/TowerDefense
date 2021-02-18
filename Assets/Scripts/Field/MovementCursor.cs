using System;
using System.Net;
using UnityEngine;

namespace Field
{
    public class MovementCursor: MonoBehaviour
    {
        [SerializeField] private int m_GridWidth;
        [SerializeField] private int m_GridHeight;
        [SerializeField] private float m_NodeSize;

        [SerializeField] private MovementAgent m_MovementAgent;
        [SerializeField] private GameObject m_Cursor;
        
        private Camera m_Camera;
        private Vector3 m_Offset;
        
        private void OnValidate()
        {
            m_Camera = Camera.main;
            
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            transform.localScale = new Vector3(width * 0.1f, 1f, height * 0.1f);

            m_Offset = transform.position - (new Vector3(width, 0f, height) * 0.5f);
        }

        private void Update()
        {
            if (m_Camera == null) 
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
                m_Cursor.SetActive(true);

                Vector3 hitPosition = hit.point;
                Vector3 difference = hitPosition - m_Offset;
                
                int x = (int)(difference.x / m_NodeSize);
                int y = (int)(difference.z / m_NodeSize);
                Vector3 targetPosition = m_Offset + new Vector3((x+ 0.5f)*m_NodeSize, hitPosition.y, (y+0.5f)*m_NodeSize);
                if (Input.GetMouseButtonDown(1))
                {
                    m_MovementAgent.SetTarget(targetPosition);
                }
                m_Cursor.transform.position = targetPosition;
                
            }
            else
            {
                m_Cursor.SetActive(false);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < m_GridWidth; ++i)
            {
                Vector3 from = new Vector3(m_Offset.x + i * m_NodeSize, m_Offset.y, m_Offset.z);
                Vector3 to = new Vector3(m_Offset.x + i * m_NodeSize, m_Offset.y, m_Offset.z + m_GridHeight * m_NodeSize);
                Gizmos.DrawLine(from, to);
            }
            for (int j = 0; j < m_GridWidth; ++j)
            {
                Vector3 from = new Vector3(m_Offset.x, m_Offset.y, m_Offset.z + j * m_NodeSize);
                Vector3 to = new Vector3(m_Offset.x + m_GridWidth * m_NodeSize, m_Offset.y, m_Offset.z + j * m_NodeSize);
                Gizmos.DrawLine(from, to);
            }
        }
    }
}