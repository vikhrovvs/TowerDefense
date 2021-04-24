using System.Collections;
using Assets;
using Runtime;
using UnityEngine;

namespace Enemy
{
    public class EnemyData
    {
        private EnemyView m_View;
        private float m_Health;
        
        public EnemyView View => m_View;

        private EnemyAsset m_Asset;
        public EnemyAsset Asset => m_Asset;
        //public readonly EnemyAsset Asset;

        public bool IsDead => m_Health <= 0;
        
        public EnemyData(EnemyAsset asset)
        {
            m_Health = asset.StartHealth;
            m_Asset = asset;
        }
        public void AttachView(EnemyView view)
        {
            m_View = view;
            m_View.AttachData(this);
        }

        public void GetDamage(float damage)
        {
            if (IsDead)
            {
                return;
            }
            m_Health -= damage;
        }

        public void Die()
        {
            m_View.Die();
            //Game.Player.EnemyDied(this);
            //Debug.Log("Die");
        }

        public void ReachedTarget()
        {
            m_Health = 0;
            //IsDead -> True; EnemyDeathController не вызовется, т. к. из списка удаляем
            m_View.ReachedTarget();
        }
    }
}