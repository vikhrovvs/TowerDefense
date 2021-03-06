using UnityEngine;

public enum OccupationAvailability
{
    CanOccupy,
    CanNotOccupy,
    Undefined
}


namespace Field
{
    public class Node
    {
        public Vector3 Position;
        
        public Node NextNode;
        public bool IsOccupied;

        public float PathWeight;
        public OccupationAvailability OccupationAvailability;
        
        public Node(Vector3 position)
        {
            Position = position;
            OccupationAvailability = OccupationAvailability.Undefined;
        }

        public void Reset()
        {
            PathWeight = float.MaxValue;
            OccupationAvailability = OccupationAvailability.CanOccupy;
        }

        public void ResetWeight()
        {
            PathWeight = float.MaxValue;
        }
    }
}