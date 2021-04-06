using System.Collections.Generic;
using Enemy;
using Field;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Projectiles.Rocket
{
    public class RocketProjectile : MonoBehaviour, IProjectile
    {
        private float m_Speed;
        private float m_Damage;
        private float m_AOE;
        private bool m_DidHit = false;
        private List<EnemyData> m_HitEnemies = new List<EnemyData>();
        private EnemyData m_Target;
        
        public void SetAssetAndTarget(RocketProjectileAsset asset, EnemyData target)
        {
            m_Speed = asset.Speed;
            m_Damage = asset.Damage;
            m_AOE = asset.AOE;
            m_Target = target;
        }
        
        public void TickApproaching()
        {
            transform.Translate((m_Target.View.transform.position - transform.position).normalized * (m_Speed * Time.deltaTime), Space.World);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            m_DidHit = true;
            List<Node> nodes = Game.Player.Grid.GetNodesInCircle(transform.position, m_AOE);
            foreach (Node node in nodes)
            {
                foreach (EnemyData enemyData in node.EnemyDatas)
                {
                    //Debug.Log("Dealt damage");
                    m_HitEnemies.Add(enemyData);
                }
            }
        }
        
        public bool DidHit()
        {
            return m_DidHit;
        }

        public void DestroyProjectile()
        {
            foreach (EnemyData hitEnemy in m_HitEnemies)
            {
                hitEnemy.GetDamage(m_Damage);
            }
            Destroy(gameObject);
        }
    }
}