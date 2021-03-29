namespace Turret.Weapon.Projectiles
{
    public interface IProjectile
    {
        void TickApproaching();
        bool DidHit();
        void DestroyProjectile();
    }
}