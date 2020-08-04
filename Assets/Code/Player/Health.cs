using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Health : MonoBehaviourPunCallbacks//, IPunObservable
{
    public delegate void HealthChangeEvent(float value);
    public HealthChangeEvent onChanged;

    public delegate void LifeChangeEvent();
    public LifeChangeEvent onKilled;
    public LifeChangeEvent onRevived;

    [SerializeField] private float _current = 100;
    [SerializeField] private float _max = 100;

    public float CurrentHealth { get { return _current; } }
    public float MaxHealth { get { return _max; } }
    public bool IsAlive => _current > 0f;
    public bool IsDead => _current <= 0f;

#region Private vars
    private Player _owner;
    private bool _isDamageable = true;
#endregion

    private void Start()
    {
        _owner = photonView.Owner;

        _current = _max;

        if (photonView.IsMine)
        {
            _owner?.MaxHealth(_max);
            _owner?.CurHealth(_current);
        }
    }

    public void Kill_Master()
    {
        ApplyHealthChange_Master(Mathf.NegativeInfinity);
    }

    public void Revive_Master()
    {
        ApplyHealthChange_Master(Mathf.Infinity);
    }

    public bool ApplyHealthChange_Master(float change)
    {
        if (change == 0)
            return false;

        bool result = false;

        var newValue = Mathf.Clamp(_current + change, 0, _max);

        if (newValue <= 0f && IsAlive)
        {
            result = true;
        }
        else if (newValue > 0f && IsDead)
        {
            result = true;
        }

        photonView.RPC("ApplyHealthChange_All", RpcTarget.All, change, newValue);
        PhotonNetwork.SendAllOutgoingCommands();

        return result;
    }

    [PunRPC]
    public void ApplyHealthChange_All(float change, float newValue)
    {
        bool wasAlive = IsAlive;
        _current = newValue;

        if (PhotonNetwork.LocalPlayer == photonView.Owner)
        {
            if (!IsAlive && wasAlive)
            {
                onKilled?.Invoke();
            }
            else if (IsAlive && !wasAlive)
            {
                onRevived?.Invoke();
            }
        }

        onChanged?.Invoke(change);
    }
}
