using UnityEngine;

namespace Turret.Weapon.FieldWeapon
{
    [CreateAssetMenu(menuName = "Assets/Turret Field Weapon Asset", fileName = "Turret Field Weapon Asset")]
    public class TurretFieldWeaponAsset : TurretWeaponAssetBase
    {
        public float Damage;
        public float Radius;
        public FieldSphereView m_SphereViewPrefab;
        public override ITurretWeapon GetWeapon(TurretView view)
        {
            Transform sphereTransform = view.ProjectileOrigin.transform;
            sphereTransform.localScale *= Radius;
            FieldSphereView sphereView = Instantiate(m_SphereViewPrefab, sphereTransform);
            return new TurretFieldWeapon(this, view, sphereView);
        }
    }
}