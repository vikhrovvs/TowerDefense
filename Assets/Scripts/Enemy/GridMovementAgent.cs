using Field;
using Runtime;
using UnityEngine;
using Grid = Field.Grid;

namespace Enemy
{
    public class GridMovementAgent : IMovementAgent
    {
        private float m_Speed;
        private Transform m_Transform;
        
        private const float TOLERANCE = 0.1f;

        private Node m_TargetNode;
        private Node m_CurrentNode;

        private EnemyData m_EnemyData;

        public GridMovementAgent(float speed, Transform transform, Grid grid, EnemyData enemyData)
        {
            m_Speed = speed;
            m_Transform = transform;
            
            SetTargetNode(grid.GetStartNode(), enemyData);
            m_EnemyData = enemyData;
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
                /*
                m_CurrentNode.EnemyDatas.Remove(m_EnemyData);
                m_CurrentNode = m_TargetNode;
                m_CurrentNode.EnemyDatas.Add(m_EnemyData);
                */
                m_TargetNode = m_TargetNode.NextNode;
                return;
            }

            Node currentNode = Game.Player.Grid.GetNodeAtPoint(m_Transform.position);
            if (currentNode != m_CurrentNode)
            {
                m_CurrentNode.EnemyDatas.Remove(m_EnemyData);
                m_CurrentNode = currentNode;
                m_CurrentNode.EnemyDatas.Add(m_EnemyData);
            }

            Vector3 dir = difference.normalized;
            Vector3 delta = dir * (m_Speed * Time.deltaTime);
            m_Transform.Translate(delta);
        }

        public void Die()
        {
            m_CurrentNode.EnemyDatas.Remove(m_EnemyData);
        }

        public void Reach()
        {
            m_CurrentNode.EnemyDatas.Remove(m_EnemyData);
        }

        private void SetTargetNode(Node node, EnemyData enemyData)
        {
            m_TargetNode = node;
            m_CurrentNode = node;
            node.EnemyDatas.Add(enemyData);
        }

        public Node GetCurrentNode()
        {
            return m_CurrentNode;
        }
    }
}