using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dimar.Pools;
using Photon.Realtime;
using Photon.Pun;

public class ProjectileHitscan : ProjectileBase
{
    [SerializeField] protected float maxDistance = 100;
    [SerializeField] protected float despawnDelay = 1f;

    public override void Fire(Player owner, Firearm ownerGun, float damage)
    {
        base.Fire(owner, ownerGun, damage);

        GetComponentInChildren<TrailRenderer>()?.Clear();
        _poolComp.ReturnWithDelay(despawnDelay);

        // make trace
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            if (HitPrecondition(hit))
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    DealDamage_Master(hit.collider);
                }
                // if (PhotonNetwork.LocalPlayer == _owner)
                transform.position = hit.point;

                onHit?.Invoke(hit.point, hit.normal, hit.collider.sharedMaterial);
            }
        }
        else
        {
            transform.position = transform.position + transform.forward * maxDistance;
        }
    }
    
    protected bool HitPrecondition(RaycastHit hit)
    {
        if (hit.distance.AlmostEquals(0f, float.Epsilon)) // means initial hit
            return false;
        if (hit.rigidbody && hit.rigidbody.GetComponent<Firearm>() == _ownerGun)
            return false;
        
        return true;
    }
}
