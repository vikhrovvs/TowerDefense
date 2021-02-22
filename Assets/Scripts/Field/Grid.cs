using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public class Grid
    {
        private Node[,] m_Nodes;

        private int m_Width;
        private int m_Height;

        private FlowFieldPathfinding m_Pathfinding;

        public Grid(int width, int height, Vector3 offset, float nodeSize, Vector2Int target)
        {
            m_Width = width;
            m_Height = height;

            m_Nodes = new Node[m_Width, m_Height];
            for (int i = 0; i < m_Nodes.GetLength(0); i++)
            {
                for (int j = 0; j < m_Nodes.GetLength(1); j++)
                {
                    m_Nodes[i, j] = new Node(offset + new Vector3(i + .5f, 0, j + .5f) * nodeSize);
                }
            }
            
            m_Pathfinding = new FlowFieldPathfinding(this, target);
            
            m_Pathfinding.UpdateField();
        }

        public int Width => m_Width;

        public int Height => m_Height;

        public Node GetNode(Vector2Int coordinate)
        {
            return GetNode(coordinate.x, coordinate.y);
        }
        
        public Node GetNode(int i, int j) //можно и через ...
        {
            //Исключение (дефолтное) - плохо, так как вылетит до конца треда. Лучше руками проверить. Также exception'ы медленные
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
    }
}