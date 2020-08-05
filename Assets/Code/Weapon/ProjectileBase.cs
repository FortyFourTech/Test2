using System;
using UnityEngine;
using Dimar.Pools;
using Photon.Realtime;

/// <summary>
/// Projectile спавнится независимо на каждом клиенте.
/// Предполагается что при спавне с одинаковым transform'ом траектория будет одинаковой.
/// Наносит урон только экземпляр, отработавший на мастер-клиенте.
/// </summary>
public abstract class ProjectileBase : MonoBehaviour
{
    public Action onFire;
    // Collider, Position, Normal, Physics material
    public Action<Collider,Vector3,Vector3,PhysicMaterial> onHit;

    protected Poolable _poolComp;
    protected Vector3 _startPoint;

    public Player OwnerPlayer {get; private set;}
    public Firearm OwnerGun {get; private set;}
    public float Damage {get; private set;}

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

        OwnerGun = ownerGun;
        OwnerPlayer = owner;
        Damage = damage;
        _startPoint = transform.position;
    }
}
