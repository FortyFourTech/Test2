using UnityEngine;
using Photon.Pun;

public class ProjectilePointDamage : ProjectileCallbacks
{
    protected override void OnHit(Collider hitCollider, Vector3 hitPoint, Vector3 hitNormal, PhysicMaterial hitMaterial)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var playerC = hitCollider.GetComponentInParent<PlayerPawn>();

            if (playerC)
            {
                var killed = playerC.Health.ApplyHealthChange_Master(-_projectile.Damage, _projectile.OwnerPlayer);
            }
        }
    }
}
