using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviorExtended
{
    public Sprite WeaponBurstEffect;

    public GameObject Projectile;
    
    public void Shoot(Vector2 direction)
    {
        var projectile = Instantiate(Projectile, transform).GetComponent<PlayerProjectile>();
        projectile.Initialize(direction, 5);
    }

}
