using System.Collections.Generic;
using Enemy;
using Field;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.FieldWeapon
{
    public class TurretFieldWeapon : ITurretWeapon
    {
        private TurretFieldWeaponAsset m_Asset;
        private TurretView m_View;
        private float m_Damage;
        private float m_Radius;
        private FieldSphereView m_SphereView;
        private List<Node> m_Nodes;
        private Vector3 m_Origin;
        public TurretFieldWeapon(TurretFieldWeaponAsset asset, TurretView view, FieldSphereView sphereView)
        {
            m_Asset = asset;
            m_View = view;
            m_Damage = asset.Damage;
            m_Radius = asset.Radius;
            m_Origin = m_View.ProjectileOrigin.transform.position;
            m_Nodes = Game.Player.Grid.GetNodesInCircle(m_Origin, m_Asset.Radius);
            m_SphereView = sphereView;
        }
        
        public void TickShoot()
        {
            foreach (Node node in m_Nodes)
            {
                foreach (EnemyData enemyData in node.EnemyDatas)
                {
                    if ((enemyData.View.transform.position - m_Origin).magnitude < m_Radius) 
                    //useful if there is difference in y coordinates
                    {
                        //Debug.Log("Hit!");
                        enemyData.GetDamage(m_Damage * Time.deltaTime);
                    }
                }
            }
        }
    }
}