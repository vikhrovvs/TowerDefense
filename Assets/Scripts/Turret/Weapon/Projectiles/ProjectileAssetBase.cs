using Enemy;
using UnityEngine;

namespace Turret.Weapon.Projectiles
{
    public abstract class ProjectileAssetBase : ScriptableObject
    {
        public abstract IProjectile CreateProjectile(Vector3 origin, Vector3 originForward, EnemyData enemyData);
    }
}