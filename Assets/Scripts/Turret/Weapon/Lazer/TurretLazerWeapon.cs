using System;
using System.Collections.Generic;
using Enemy;
using Field;
using JetBrains.Annotations;
using Runtime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Turret.Weapon.Lazer
{
    public class TurretLazerWeapon : ITurretWeapon
    {
        private TurretLazerWeaponAsset m_Asset;
        private TurretView m_View;
        private float m_MaxDistance;
        private List<Node> m_Nodes;
        
        [CanBeNull]
        private EnemyData m_ClosestEnemyData;
        private LineRenderer m_LineRenderer;

        private float m_Damage;

        public TurretLazerWeapon(TurretLazerWeaponAsset asset, TurretView view)
        {
            m_Asset = asset;
            m_View = view;
            m_MaxDistance = m_Asset.MaxDistance;
            m_Damage = asset.Damage;
            
            m_LineRenderer = Object.Instantiate(m_Asset.LineRendererPrefab, m_View.ProjectileOrigin.transform);
            m_Nodes = Game.Player.Grid.GetNodesInCircle(m_View.ProjectileOrigin.transform.position, m_MaxDistance);
        }
        public void TickShoot()
        {
            m_ClosestEnemyData = EnemySearch.GetClosestEnemy(m_View.transform.position, m_MaxDistance, m_Nodes);
            if (m_ClosestEnemyData == null)
            {
                m_LineRenderer.gameObject.SetActive(false);
            }
            else
            {
                Vector3 closestEnemyPosition = m_ClosestEnemyData.View.transform.position;
                m_View.TowerLookAt(closestEnemyPosition);
                m_LineRenderer.gameObject.transform.LookAt(closestEnemyPosition);
                float distance = (m_View.ProjectileOrigin.transform.position - closestEnemyPosition).magnitude;
                m_LineRenderer.gameObject.transform.localScale = new Vector3(1, 1, distance);
                m_LineRenderer.gameObject.SetActive(true);
                m_ClosestEnemyData.GetDamage(m_Damage * Time.deltaTime);
                //Debug.Log("Dealt damage");
            }
        }
    }
}