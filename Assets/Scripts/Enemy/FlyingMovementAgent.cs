using Field;
using UnityEngine;
using Grid = Field.Grid;

namespace Enemy
{
    public class FlyingMovementAgent : IMovementAgent
    {
        private float m_Speed;
        private Transform m_Transform;
        private const float TOLERANCE = 0.1f;
        private Node m_TargetNode;
        public FlyingMovementAgent(float speed, Transform transform, Grid grid)
        {
            m_Speed = speed;
            m_Transform = transform;
            
            SetTargetNode(grid.GetTargetNode());
        }
        public void TickMovement()
        {
            if (m_TargetNode == null)
            {
                return;
            }
            Vector3 target = m_TargetNode.Position;

            Vector3 difference = target - m_Transform.position;
            difference.y = 0;
            float distance = difference.magnitude;
            if (distance < TOLERANCE)
            {
                m_TargetNode = m_TargetNode.NextNode;
                return;
            }

            Vector3 dir = difference.normalized;
            Vector3 delta = dir * (m_Speed * Time.deltaTime);
            m_Transform.Translate(delta);
            
        }
        private void SetTargetNode(Node node)
        {
            m_TargetNode = node;
        }
    }
}