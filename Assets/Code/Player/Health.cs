using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Health : MonoBehaviourPunCallbacks//, IPunObservable
{
    public delegate void HealthChangeEvent(Health health, SHealthChangeInfo info);
    public HealthChangeEvent onChanged;

    public delegate void LifeChangeEvent(Health health, Player causer);
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

    private void Awake()
    {
        PhotonPeer.RegisterType(typeof(SHealthChangeInfo), (byte) 'D', SHealthChangeInfo.SerializeToPhoton, SHealthChangeInfo.DeserializeFromPhoton);
    }

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

    private void Update()
    {
        // for debug
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.Keypad3))
        {
            if (IsAlive)
            {
                photonView.RPC("Kill_Master", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);
            }
            else
            {
                photonView.RPC("Revive_Master", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);
            }
        }
    }

    [PunRPC]
    public void Kill_Master(Player killer = null)
    {
        ApplyHealthChange_Master(Mathf.NegativeInfinity);
    }

    [PunRPC]
    public void Revive_Master(Player saviour = null)
    {
        ApplyHealthChange_Master(Mathf.Infinity);
    }

    public bool ApplyHealthChange_Master(float change, Player causer = null)
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

        SHealthChangeInfo changeInfo = new SHealthChangeInfo(){
            change = change,
            newValue = newValue,
            causer = causer,
        };
        photonView.RPC("ApplyHealthChange_All", RpcTarget.All, changeInfo);
        PhotonNetwork.SendAllOutgoingCommands();

        return result;
    }

    [PunRPC]
    public void ApplyHealthChange_All(SHealthChangeInfo changeInfo)
    {
        bool wasAlive = IsAlive;
        _current = changeInfo.newValue;

        if (!IsAlive && wasAlive)
        {
            onKilled?.Invoke(this, changeInfo.causer);
        }
        else if (IsAlive && !wasAlive)
        {
            onRevived?.Invoke(this, changeInfo.causer);
        }

        onChanged?.Invoke(this, changeInfo);
    }

    // [Serializable]
    public struct SHealthChangeInfo
    {
        public float change;
        public float newValue;
        public Player causer;

        public SHealthChangeInfo(float inChange, float inNewValue)
        {
            change = inChange;
            newValue = inNewValue;
            causer = null;
        }

        public static readonly byte[] memInfo = new byte[4+4+4];
        public static short SerializeToPhoton(ExitGames.Client.Photon.StreamBuffer outStream, object customObject)
        {
            SHealthChangeInfo serializedInfo = (SHealthChangeInfo) customObject;
            Player causer = serializedInfo.causer;

            lock (memInfo)
            {
                byte[] bytes = memInfo;
                int off = 0;
                Protocol.Serialize(serializedInfo.change, bytes, ref off);
                Protocol.Serialize(serializedInfo.newValue, bytes, ref off);

                if (causer != null)
                    Protocol.Serialize(causer.ActorNumber, bytes, ref off);
                else
                    Protocol.Serialize(-1, bytes, ref off);
                
                outStream.Write(bytes, 0, bytes.Length);
                return (short)bytes.Length;
            }
        }

        public static object DeserializeFromPhoton(ExitGames.Client.Photon.StreamBuffer inStream, short length)
        {
            SHealthChangeInfo result = new SHealthChangeInfo();
            int playerID;

            lock (memInfo)
            {
                inStream.Read(memInfo, 0, length);
                int off = 0;
                Protocol.Deserialize(out result.change, memInfo, ref off);
                Protocol.Deserialize(out result.newValue, memInfo, ref off);

                Protocol.Deserialize(out playerID, memInfo, ref off);
            }

            if (PhotonNetwork.CurrentRoom != null)
            {
                Player player = PhotonNetwork.CurrentRoom.GetPlayer(playerID);
                result.causer = player;
            }

            return result;
        }
    }
}
