using UnityEngine;

namespace Field
{
    public class Node
    {
        public Vector3 Position;
        
        public Node NextNode;
        public bool IsOccupied;

        public float PathWeight;

        public Node(Vector3 position)
        {
            Position = position;
        }

        public void ResetWeight()
        {
            PathWeight = float.MaxValue; //гипотетически может быть больше площади поля, но не на практике
        }
    }
}