using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dimar.Pools;
using Photon.Realtime;
using Photon.Pun;

/// <summary>
/// Projectile спавнится независимо на каждом клиенте.
/// Предполагается что при спавне с одинаковым transform'ом траектория будет одинаковой.
/// Наносит урон только экземпляр, отработавший на мастер-клиенте.
/// </summary>
public abstract class ProjectileBase : MonoBehaviour
{
    public Action onFire;
    // Position, Normal, Physics material
    public Action<Vector3,Vector3,PhysicMaterial> onHit;

    protected Poolable _poolComp;
    protected Player _ownerPlayer;
    protected Firearm _ownerGun;
    protected float _damage;
    protected Vector3 _startPoint;

    protected void OnEnable()
    {
        _poolComp = GetComponent<Poolable>();

        if (_poolComp)
        {
            _poolComp.onQueue += OnSpawn;
            _poolComp.onReturn += OnPool;
        }
    }

    protected void OnDisable()
    {
        if (_poolComp)
        {
            _poolComp.onQueue -= OnSpawn;
            _poolComp.onReturn -= OnPool;
        }
    }

    protected virtual void OnPool() {}

    protected virtual void OnSpawn() {}

    public virtual void Fire(Player owner, Firearm ownerGun, float damage)
    {
        onFire?.Invoke();

        _ownerGun = ownerGun;
        _ownerPlayer = owner;
        _damage = damage;
        _startPoint = transform.position;
    }

    protected void DealDamage_Master(Collider collider)
    {
        var playerC = collider.GetComponentInParent<GamePlayerController>();

        if (playerC)
        {
            var killed = playerC.Health.ApplyHealthChange_Master(-_damage);
        }
    }
}
