using Enemy;
using UnityEngine;

namespace Turret.Weapon.Projectiles.Bullet
{
    public class BulletProjectile : MonoBehaviour, IProjectile
    {
        private float m_Speed = 10f;
        private int m_Damage = 5;
        private bool m_DidHit = false;
        private EnemyData m_HitEnemy = null;
        public void TickApproaching()
        {
            transform.Translate(transform.forward * (m_Speed * Time.deltaTime), Space.World);
            //Должны в начале повернуть в правильную сторону
            //Self/World
            //Self - с учетом поворота тела в собственном пространстве; по умолчанию
        }

        private void OnTriggerEnter(Collider other)
        {
            m_DidHit = true;
            if (other.CompareTag("Enemy"))
            {
                EnemyView enemyView = other.GetComponent<EnemyView>();
                if (enemyView != null)
                {
                    m_HitEnemy = enemyView.Data;
                }
            }
        }

        public bool DidHit()
        {
            return m_DidHit;
        }

        public void DestroyProjectile()
        {
            if (m_HitEnemy != null)
            {
                m_HitEnemy.GetDamage(m_Damage);
                //Debug.Log("hit!");
            }
            Destroy(gameObject);
            //Уничтожает MonoBehaviour; передадим this => уничтожим BulletProjectile, но не сам объект!
        }
    }
}