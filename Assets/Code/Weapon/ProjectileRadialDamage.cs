using System.Linq;
using UnityEngine;
using Photon.Pun;

public class ProjectileRadialDamage : ProjectileCallbacks
{
    [SerializeField] private float _fullDamageRadius = 1f;
    [SerializeField] private float _falloffRadius = 2f;
    
    protected override void OnHit(Collider hitCollider, Vector3 hitPoint, Vector3 hitNormal, PhysicMaterial hitMaterial)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var overlaps = Physics.OverlapSphere(_projectile.transform.position, _fullDamageRadius + _falloffRadius);

            var damageables = overlaps.Where(c => { // first filter owner objects
                    var gunC = c.GetComponentInParent<Firearm>();
                    var playerC = c.GetComponentInParent<PlayerPawn>();

                    return (gunC == null) //  || gunC != _projectile.OwnerGun // no need cause we know that weapons does not receive damage
                        && !(playerC != null && playerC.Owner == _projectile.OwnerPlayer);
                })
                .Select(collider => {
                    var health = collider.GetComponentInParent<Health>();
                    var distance = health != null // to not calculate useless values
                        ? collider.ClosestPoint(_projectile.transform.position) - _projectile.transform.position
                        : Vector3.zero;
                    return (collider, health, distance);
                })
                .Where(ch => ch.health != null);

            foreach (var damageable in damageables)
            {
                var distance = damageable.distance.magnitude;
                float damage = 0f;
                if (distance <= _fullDamageRadius)
                {
                    damage = _projectile.Damage;
                }
                else if (distance <= _fullDamageRadius + _falloffRadius)
                {
                    damage = _projectile.Damage * (distance - _fullDamageRadius) / _falloffRadius;
                }

                Debug.Log($"rocket explosion hit {damageable.health.name} with distance {distance}", damageable.health);
                if (damage > 0f)
                {
                    damageable.health.ApplyHealthChange_Master(-damage, _projectile.OwnerPlayer);
                }
            }
        }
    }
}
