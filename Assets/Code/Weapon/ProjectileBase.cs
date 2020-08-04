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

    protected void DealDamage_Master()
    {
        // var playerC = hit.rigidbody?.GetComponent<GamePlayerController>();

        // if (playerC)
        // {
        //     var damageInfo = new SHealthChangeInfo(){
        //         change = -_damage,
        //         causer = _ownerPlayer,
        //         damagePoint = hit.point,
        //         source = _startPoint,
        //         type = EHealthChangeType.bullet,
        //         weaponIdx = (short)ownerGun.firearmIndex,
        //     };

        //     var killed = playerC.Health.ApplyChange_Server(damageInfo);

        //     GameModePicker.CurrentGameMode.PlayerHit(_ownerPlayer, playerC.GetOwner(), _damage);
        //     if (killed)
        //     {
        //         GameModePicker.CurrentGameMode.PlayerKill(_ownerPlayer, playerC.GetOwner());
        //         _ownerPlayer.References().m_Controller.OnControllerKill_Server(damageInfo);
        //         playerC.OnControllerDeath_Server(damageInfo);
        //     }
        // }
    }
}
