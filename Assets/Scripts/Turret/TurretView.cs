using UnityEngine;

namespace Turret
{
    public class TurretView: MonoBehaviour
    {
        [SerializeField] private Transform m_ProjectileOrigin;
        private TurretData m_Data;
        public TurretData Data => m_Data;

        public Transform ProjectileOrigin => m_ProjectileOrigin;

        public void AttachData(TurretData turretData)
        {
            m_Data = turretData;
            transform.position = m_Data.Node.Position; //Для этого и нужна position у Node!
        }
    }
}