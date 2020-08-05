using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dimar.Pools;
using Photon.Realtime;
using Photon.Pun;

public class ProjectileFly : ProjectileBase
{
    [SerializeField] private float _velocity = 100f;
    [SerializeField] protected float despawnDelay = 1f;

    [Tooltip("Will be searched on same GO, if not filled")]
    [SerializeField] private Rigidbody _rigidbody;

    public override void Fire(Player owner, Firearm ownerGun, float damage)
    {
        base.Fire(owner, ownerGun, damage);
        
        CacheRigidbody();

        if (_rigidbody)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            
            _rigidbody.AddForce(transform.forward * _velocity, ForceMode.Impulse);
        }

        _poolComp.ReturnWithDelay(despawnDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HitPrecondition(other))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                DealDamage_Master(other);
            }

            onHit?.Invoke(other, transform.position, -transform.forward, other.sharedMaterial);

            _poolComp.ReturnToPool();
        }
    }

    private bool HitPrecondition(Collider other)
    {
        if ((transform.position - _startPoint).sqrMagnitude.AlmostEquals(0f, 0.5f)) // means initial hit
            return false;

        var otherC = other.GetComponentInParent<PlayerPawn>();
        var ownerC = OwnerPlayer.References().m_pawn;
        if (otherC && ownerC && otherC == ownerC)
            return false;
        
        return true;
    }

    private void CacheRigidbody()
    {
        if (!_rigidbody)
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
    }
}
