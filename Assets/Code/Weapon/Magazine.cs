using System;
using Photon.Pun;
using UnityEngine;

public class Magazine : MonoBehaviour, IPunObservable
{
    public Action onRoundsChanged;
    public Action onEmpty;
    public Action onReload;
    
    [SerializeField] protected int _maxRounds;
    [SerializeField] protected int _currentRounds;

    public int MaxRounds { get { return _maxRounds; } }
    public int CurrentRounds
    {
        get => _currentRounds;
        set
        {
            if (value != _currentRounds)
            {
                _currentRounds = Mathf.Clamp(value, 0, _maxRounds);
                onRoundsChanged?.Invoke();
            }
        }
    }

    public bool Full {
        get => _currentRounds >= _maxRounds;
        set { if (value) LoadFullMag_Owner(); }
    }
    public bool Empty {
        get => _currentRounds <= 0;
        set { if (value) EjectAll_Owner(); }
    }

    public void LoadBullet_Owner()
    {
        LoadBullets_Owner(1);
    }

    public void LoadBullets_Owner(int number)
    {
        if (Full)
            return;

        CurrentRounds += number;
    }

    public void LoadFullMag_Master()
    {
        var pv = PhotonView.Get(this);
        pv.RPC("LoadFullMag_Owner", pv.Owner);
    }

    [PunRPC]
    public void LoadFullMag_Owner()
    {
        if (Full)
            return;

        CurrentRounds = _maxRounds;
        onReload?.Invoke();
    }

    public void EjectBullet_Owner()
    {
        EjectBullets_Owner(1);
    }

    public void EjectBullets_Owner(int number)
    {
        if (Empty)
            return;

        CurrentRounds -= number;
    }

    public void EjectAll_Owner()
    {
        if (Empty)
            return;

        CurrentRounds = 0;
        onEmpty?.Invoke();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
        {
            var prevCurrentRounds = _currentRounds;
            CurrentRounds = (int)stream.ReceiveNext();

            if (_currentRounds != prevCurrentRounds)
            {
                onRoundsChanged?.Invoke();

                if (prevCurrentRounds > 0 && Empty)
                {
                    onEmpty?.Invoke();
                }
                else if (prevCurrentRounds < _maxRounds && Full)
                {
                    onReload?.Invoke();
                }
            }
        }
        else
        {
            stream.SendNext(_currentRounds);
        }
    }
}
