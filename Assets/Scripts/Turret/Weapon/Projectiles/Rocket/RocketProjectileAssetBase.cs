using Enemy;
using UnityEngine;

namespace Turret.Weapon.Projectiles.Rocket
{
    [CreateAssetMenu(menuName = "Assets/Rocket Projectile Asset", fileName = "Rocket Projectile Asset")]
    public class RocketProjectileAssetBase : ProjectileAssetBase
    {
        public override IProjectile CreateProjectile(Vector3 origin, Vector3 originForward, EnemyData enemyData)
        {
            throw new System.NotImplementedException();
        }
    }
}