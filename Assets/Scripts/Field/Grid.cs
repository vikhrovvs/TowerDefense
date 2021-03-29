using System;
using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public class Grid
    {
        private Node[,] m_Nodes;

        private int m_Width;
        private int m_Height;

        private Vector2Int m_StartCoordinate;
        private Vector2Int m_TargetCoordinate;
        
        private Node m_SelectedNode = null;

        private FlowFieldPathfinding m_Pathfinding;

        private Vector3 m_offset;
        private float m_nodeSize;

        public Grid(int width, int height, Vector3 offset, float nodeSize, Vector2Int start, Vector2Int target) //order
        {
            m_Width = width;
            m_Height = height;

            m_StartCoordinate = start;
            m_TargetCoordinate = target;

            m_offset = offset;
            m_nodeSize = nodeSize;
            
            m_Nodes = new Node[m_Width, m_Height];
            for (int i = 0; i < m_Nodes.GetLength(0); i++)
            {
                for (int j = 0; j < m_Nodes.GetLength(1); j++)
                {
                    m_Nodes[i, j] = new Node(offset + new Vector3(i + .5f, 0, j + .5f) * nodeSize);
                }
            }
            
            m_Pathfinding = new FlowFieldPathfinding(this, target, start);
            
            m_Pathfinding.UpdateField();
        }

        public Node GetStartNode()
        {
            return GetNode(m_StartCoordinate);
        }

        public Node GetTargetNode()
        {
            return GetNode(m_TargetCoordinate);
        }

        public void SelectCoordinate(Vector2Int coordinate)
        {
            m_SelectedNode = GetNode(coordinate);
        }

        public void UnselectNode()
        {
            m_SelectedNode = null;
        }

        public bool HasSelectedNode()
        {
            return m_SelectedNode != null;
        }

        public Node GetSelectedNode()
        {
            return m_SelectedNode;
        }

        public int Width => m_Width;

        public int Height => m_Height;

        public Node GetNode(Vector2Int coordinate)
        {
            return GetNode(coordinate.x, coordinate.y);
        }
        
        public Node GetNode(int i, int j)
        {
            if (i < 0 || i >= m_Width)
            {
                return null;
            }

            if (j < 0 || j >= m_Height)
            {
                return null;
            }
            return m_Nodes[i, j];
        }

        public IEnumerable<Node> EnumerateAllNodes()
        {
            for (int i = 0; i < m_Width; ++i)
            {
                for (int j = 0; j < m_Height; ++j)
                {
                    yield return GetNode(i, j);
                }

            }
        }

        public void UpdatePathfinding()
        {
            m_Pathfinding.UpdateField();
        }

        public void SwitchOccupyNode(Node node)
        {
            //Node node = GetNode(coordinate.x, coordinate.y);
            if (node.IsOccupied)
            {
                node.IsOccupied = false;
                UpdatePathfinding();
            }
            else
            {
                if (m_Pathfinding.CanOccupy(node))
                {
                    node.IsOccupied = !node.IsOccupied;
                    UpdatePathfinding();
                }
            }
        }

        public bool TryOccupyNode(Node node)
        {
            if (node.IsOccupied)
            {
                return false;
            }
            else
            {
                if (m_Pathfinding.CanOccupy(node))
                {
                    node.IsOccupied = !node.IsOccupied;
                    UpdatePathfinding();
                    return true;
                }
                else
                {
                    return false;
                }
            }   
        }
        
        public Node GetNodeAtPoint(Vector3 point)
        {
            Vector3 pointOnGrid = point - m_offset;
            int i = (int)Math.Floor(pointOnGrid.x / m_nodeSize);
            int j = (int)Math.Floor(pointOnGrid.z / m_nodeSize);
            return GetNode(i, j);
            //m_Nodes[i, j] = new Node(offset + new Vector3(i + .5f, 0, j + .5f) * nodeSize); 
        }

        public List<Node> GetNodesInCircle(Vector3 point, float radius)
        {
            List<Node> nodes = new List<Node>();
            foreach (Node node in EnumerateAllNodes())
            {
                float distance = (node.Position - point).magnitude;
                if (distance < radius)
                {
                    nodes.Add(node);
                }
            }
            return nodes;
        }
    }
}