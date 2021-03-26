using System;
using System.Collections.Generic;
using UnityEngine;

struct Connection
{
    public Vector2Int Coordinate;
    public float Weight;

    public Connection(Vector2Int coordinate, float weight)
    {
        this.Coordinate = coordinate;
        this.Weight = weight;
    }
}



namespace Field
{
    public class FlowFieldPathfinding
    {
        private Grid m_Grid;
        private Vector2Int m_Target;
        private Vector2Int m_Start;
        private const float STRAIGHT_NEIGHBOUR_WEIGHT = 1;
        private float m_DiagonalNeighbourWeight = (float) Math.Sqrt(2);
        public FlowFieldPathfinding(Grid grid, Vector2Int target, Vector2Int start)
        {
            m_Grid = grid;
            m_Target = target;
            m_Start = start;
        }

        public void UpdateField()
        {
            foreach (Node node in m_Grid.EnumerateAllNodes())
            {
                node.Reset();
            }
            Queue<Connection> queue = new Queue<Connection>();
            
            queue.Enqueue(new Connection(m_Target, 0));
            m_Grid.GetNode(m_Target).PathWeight = 0;
            
            while (queue.Count > 0)
            {
                Connection current = queue.Dequeue();
                Node currentNode = m_Grid.GetNode(current.Coordinate);
                float weightToTarget = currentNode.PathWeight + current.Weight;
                
                foreach (Connection neighbour in GetNeighbours(current.Coordinate))
                {
                    Node neighbourNode = m_Grid.GetNode(neighbour.Coordinate);
                    if (weightToTarget < neighbourNode.PathWeight)
                    {
                        neighbourNode.NextNode = currentNode;
                        neighbourNode.PathWeight = weightToTarget;
                        queue.Enqueue(neighbour);
                    }
                }
            }

            Node startNode = m_Grid.GetNode(m_Start);
            Node targetNode = m_Grid.GetNode(m_Target);
            startNode.OccupationAvailability = OccupationAvailability.CanNotOccupy;
            Node nextNode = startNode.NextNode;
            while (nextNode != targetNode)
            {
                nextNode.OccupationAvailability = OccupationAvailability.Undefined;
                nextNode = nextNode.NextNode;
            }
            targetNode.OccupationAvailability = OccupationAvailability.CanNotOccupy;
        }

        private IEnumerable<Connection> GetNeighbours(Vector2Int coordinate)
        {
            Vector2Int rightCoordinate = coordinate + Vector2Int.right;
            Vector2Int leftCoordinate = coordinate + Vector2Int.left;
            Vector2Int upCoordinate = coordinate + Vector2Int.up;
            Vector2Int downCoordinate = coordinate + Vector2Int.down;
            
            Vector2Int upRightCoordinate = coordinate + Vector2Int.up + Vector2Int.right;
            Vector2Int upLeftCoordinate = coordinate + Vector2Int.up + Vector2Int.left;
            Vector2Int downRightCoordinate = coordinate + Vector2Int.down + Vector2Int.right;
            Vector2Int downLeftCoordinate = coordinate + Vector2Int.down + Vector2Int.left;

            bool hasRightNode = rightCoordinate.x < m_Grid.Width && !m_Grid.GetNode(rightCoordinate).IsOccupied;
            bool hasLeftNode = leftCoordinate.x >= 0 && !m_Grid.GetNode(leftCoordinate).IsOccupied;
            bool hasUpNode = upCoordinate.y < m_Grid.Height && !m_Grid.GetNode(upCoordinate).IsOccupied;
            bool hasDownNode = downCoordinate.y >= 0 && !m_Grid.GetNode(downCoordinate).IsOccupied;

            if (hasRightNode)
            {
                yield return new Connection(rightCoordinate, STRAIGHT_NEIGHBOUR_WEIGHT);
            }
            
            if (hasLeftNode)
            {
                yield return new Connection(leftCoordinate, STRAIGHT_NEIGHBOUR_WEIGHT);
            }
            if (hasUpNode)
            {
                yield return new Connection(upCoordinate, STRAIGHT_NEIGHBOUR_WEIGHT);
            }
            if (hasDownNode)
            {
                yield return new Connection(downCoordinate, STRAIGHT_NEIGHBOUR_WEIGHT);
            }

            if (hasUpNode && hasRightNode && !m_Grid.GetNode(upRightCoordinate).IsOccupied)
            {
                yield return new Connection(upRightCoordinate, m_DiagonalNeighbourWeight);
            }
            
            if (hasUpNode && hasLeftNode && !m_Grid.GetNode(upLeftCoordinate).IsOccupied)
            {
                yield return new Connection(upLeftCoordinate, m_DiagonalNeighbourWeight);
            }
            
            if (hasDownNode && hasRightNode && !m_Grid.GetNode(downRightCoordinate).IsOccupied)
            {
                yield return new Connection(downRightCoordinate, m_DiagonalNeighbourWeight);
            }
            
            if (hasDownNode && hasLeftNode && !m_Grid.GetNode(downLeftCoordinate).IsOccupied)
            {
                yield return new Connection(downLeftCoordinate, m_DiagonalNeighbourWeight);
            }
        }

        private IEnumerable<Vector2Int> GetNeighboursVector2Ints(Vector2Int coordinate)
        {
            Vector2Int rightCoordinate = coordinate + Vector2Int.right;
            Vector2Int leftCoordinate = coordinate + Vector2Int.left;
            Vector2Int upCoordinate = coordinate + Vector2Int.up;
            Vector2Int downCoordinate = coordinate + Vector2Int.down;

            bool hasRightNode = rightCoordinate.x < m_Grid.Width && !m_Grid.GetNode(rightCoordinate).IsOccupied;
            bool hasLeftNode = leftCoordinate.x >= 0 && !m_Grid.GetNode(leftCoordinate).IsOccupied;
            bool hasUpNode = upCoordinate.y < m_Grid.Height && !m_Grid.GetNode(upCoordinate).IsOccupied;
            bool hasDownNode = downCoordinate.y >= 0 && !m_Grid.GetNode(downCoordinate).IsOccupied;

            if (hasRightNode)
            {
                yield return rightCoordinate;
            }

            if (hasLeftNode)
            {
                yield return leftCoordinate;
            }
            if (hasUpNode)
            {
                yield return upCoordinate;
            }
            if (hasDownNode)
            {
                yield return downCoordinate;

            }
        }
        private bool CheckAvailability(Node testingNode)
        {
            testingNode.IsOccupied = true;
            
            foreach (Node node in m_Grid.EnumerateAllNodes())
            {
                node.ResetWeight();
            }
            
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            Node startNode = m_Grid.GetNode(m_Start);
            startNode.PathWeight = 0;
            queue.Enqueue(m_Start);
            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                if (current == m_Target)
                {
                    testingNode.OccupationAvailability = OccupationAvailability.CanOccupy;
                    testingNode.IsOccupied = false;
                    return true;
                }
                foreach (Vector2Int neighbour in GetNeighboursVector2Ints(current))
                {
                    Node neighbourNode = m_Grid.GetNode(neighbour);
                    if (neighbourNode.PathWeight != 0)
                    {
                        neighbourNode.PathWeight = 0;
                        queue.Enqueue(neighbour);
                    }
                }
            }

            testingNode.OccupationAvailability = OccupationAvailability.CanNotOccupy;
            testingNode.IsOccupied = false;
            return false;
        }
        public bool CanOccupy(Node node)
        {
            OccupationAvailability availability = node.OccupationAvailability;
            if (availability == OccupationAvailability.CanOccupy)
            {
                return true;
            }
            if (availability == OccupationAvailability.CanNotOccupy)
            {
                return false;
            }
            if (availability == OccupationAvailability.Undefined)
            {
                return CheckAvailability(node);
            }

            return true;
        }
    }
}



















